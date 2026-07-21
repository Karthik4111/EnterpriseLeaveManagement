using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace EnterpriseLeaveManagement.Infrastructure.Authentication;

public class JwtTokenService : IJwtTokenService
{
    private readonly JwtSettings _jwtSettings;
    private readonly UserManager<ApplicationUser> _userManager;

    public JwtTokenService(IOptions<JwtSettings> jwtSettings,UserManager<ApplicationUser> userManager)
    {
        _jwtSettings = jwtSettings.Value;
        _userManager = userManager;
    }

    public async Task<string> GenerateTokenAsync(Guid userId,string userName,string email,IList<string> roles)
    {
        var claims = new List<Claim>
        {
            new(JwtRegisteredClaimNames.Sub, userId.ToString()),
            new(JwtRegisteredClaimNames.Email, email),
            new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),

            new(ClaimTypes.NameIdentifier, userId.ToString()),
            new(ClaimTypes.Name, userName),
            new(ClaimTypes.Email, email)
        };

        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(_jwtSettings.SecretKey));

        var credentials = new SigningCredentials(
            key,
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _jwtSettings.Issuer,
            audience: _jwtSettings.Audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(_jwtSettings.ExpiryMinutes),
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}