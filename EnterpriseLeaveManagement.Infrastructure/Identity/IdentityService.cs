using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.Authentication.DTOs;
using EnterpriseLeaveManagement.Application.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace EnterpriseLeaveManagement.Infrastructure.Identity;

public class IdentityService : IIdentityService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IJwtTokenService _jwtTokenService;
    private readonly IRefreshTokenService _refreshTokenService;

    public IdentityService(
    UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    RoleManager<ApplicationRole> roleManager,
    IJwtTokenService jwtTokenService,
    IRefreshTokenService refreshTokenService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
        _jwtTokenService = jwtTokenService;
        _refreshTokenService = refreshTokenService;
    }

    public async Task<(bool Succeeded, Guid? UserId, IEnumerable<string> Errors)> RegisterUserAsync(
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
            return (false, null, new[] { "User already exists." });
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
            return (false, null, result.Errors.Select(e => e.Description));
        }

        if (!await _roleManager.RoleExistsAsync(role))
        {
            await _roleManager.CreateAsync(new ApplicationRole
            {
                Name = role
            });
        }

        await _userManager.AddToRoleAsync(user, role);

        return (true, user.Id, Enumerable.Empty<string>());
    }

    public async Task<(bool Succeeded, TokenResponseDto? Token, IEnumerable<string> Errors)> LoginAsync(
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

        var tokenResponse = await _jwtTokenService.GenerateTokenAsync(
                            user.Id,
                            user.UserName!,
                            user.Email!,
                            roles);

        // Revoke all previously active refresh tokens
        await _refreshTokenService.RevokeAllUserRefreshTokensAsync(user.Id);

        // Generate and save a new refresh token
        var refreshToken = _refreshTokenService.GenerateRefreshToken(user.Id);

        await _refreshTokenService.SaveRefreshTokenAsync(refreshToken);

        tokenResponse.RefreshToken = refreshToken.Token;

        return (true, tokenResponse, Enumerable.Empty<string>());
    }

    public async Task<bool> UserExistsAsync(Guid userId)
    {
        return await _userManager.FindByIdAsync(userId.ToString()) != null;
    }

    public async Task<IList<string>> GetUserRolesAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            return new List<string>();
        }

        return await _userManager.GetRolesAsync(user);
    }

    public async Task<(Guid Id, string UserName, string Email)?> GetUserAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());

        if (user is null)
        {
            return null;
        }

        return (user.Id, user.UserName!, user.Email!);
    }
}