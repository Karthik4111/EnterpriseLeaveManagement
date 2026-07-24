using EnterpriseLeaveManagement.Application.Common.Exceptions;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.Authentication.DTOs;
using EnterpriseLeaveManagement.Application.Interfaces;
using MediatR;

namespace EnterpriseLeaveManagement.Application.Features.Authentication.Commands.RefreshToken;

public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, LoginResponseDto>
{
    private readonly IRefreshTokenService _refreshTokenService;
    private readonly IIdentityService _identityService;
    private readonly IJwtTokenService _jwtTokenService;

    public RefreshTokenCommandHandler(
        IRefreshTokenService refreshTokenService,
        IIdentityService identityService,
        IJwtTokenService jwtTokenService)
    {
        _refreshTokenService = refreshTokenService;
        _identityService = identityService;
        _jwtTokenService = jwtTokenService;
    }

    public async Task<LoginResponseDto> Handle(
        RefreshTokenCommand request,
        CancellationToken cancellationToken)
    {
        var existingRefreshToken =
            await _refreshTokenService.GetRefreshTokenAsync(request.RefreshToken);

        if (existingRefreshToken is null ||
            !existingRefreshToken.IsActive)
        {
            throw new BadRequestException("Invalid or expired refresh token.");
        }

        var user = await _identityService.GetUserAsync(existingRefreshToken.UserId);

        if (user is null)
        {
            throw new BadRequestException("User not found.");
        }

        var roles = await _identityService.GetUserRolesAsync(user.Value.Id);

        var tokenResponse = await _jwtTokenService.GenerateTokenAsync(
            user.Value.Id,
            user.Value.UserName,
            user.Value.Email,
            roles);

        // Revoke the refresh token that was used
        await _refreshTokenService.RevokeRefreshTokenAsync(
            existingRefreshToken.Token);

        // Revoke any other active refresh tokens for this user
        await _refreshTokenService.RevokeAllUserRefreshTokensAsync(
            user.Value.Id);

        // Generate and save a fresh refresh token
        var newRefreshToken =
            _refreshTokenService.GenerateRefreshToken(user.Value.Id);

        await _refreshTokenService.SaveRefreshTokenAsync(newRefreshToken);

        return new LoginResponseDto
        {
            AccessToken = tokenResponse.AccessToken,
            RefreshToken = newRefreshToken.Token,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60)
        };
    }
}