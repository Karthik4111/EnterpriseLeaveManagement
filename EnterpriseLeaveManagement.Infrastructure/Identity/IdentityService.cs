using EnterpriseLeaveManagement.Application.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EnterpriseLeaveManagement.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IJwtTokenService _jwtTokenService;

    public IdentityService(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<ApplicationRole> roleManager,
        IJwtTokenService jwtTokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<(bool Succeeded, IEnumerable<string> Errors)> RegisterUserAsync(
        string firstName,
        string lastName,
        string userName,
        string email,
        string password,
        string role)
    {
        var existingUser = await _userManager.FindByEmailAsync(email);

        if (existingUser != null)
        {
            return (false, new[] { "User already exists." });
        }

        var user = new ApplicationUser
        {
            FirstName = firstName,
            LastName = lastName,
            UserName = userName,
            Email = email
        };

        var result = await _userManager.CreateAsync(user, password);

        if (!result.Succeeded)
        {
            return (false, result.Errors.Select(e => e.Description));
        }

        if (!await _roleManager.RoleExistsAsync(role))
        {
            await _roleManager.CreateAsync(new ApplicationRole
            {
                Name = role
            });
        }

        await _userManager.AddToRoleAsync(user, role);

        return (true, Enumerable.Empty<string>());
    }

    public async Task<(bool Succeeded, string? Token, IEnumerable<string> Errors)> LoginAsync(
        string email,
        string password)
    {
        var user = await _userManager.FindByEmailAsync(email);

        if (user is null)
        {
            return (false, null, new[] { "Invalid email or password." });
        }

        var result = await _signInManager.CheckPasswordSignInAsync(
            user,
            password,
            lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            return (false, null, new[] { "Invalid email or password." });
        }

        var roles = await _userManager.GetRolesAsync(user);

        var token = await _jwtTokenService.GenerateTokenAsync(
            user.Id,
            user.UserName ?? string.Empty,
            user.Email ?? string.Empty,
            roles);

        return (true, token, Enumerable.Empty<string>());
    }
}