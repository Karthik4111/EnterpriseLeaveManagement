using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnterpriseLeaveManagement.Application.Common.Exceptions;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.Authentication.DTOs;
using MediatR;

namespace EnterpriseLeaveManagement.Application.Features.Authentication.Commands.Login;

public class LoginCommandHandler : IRequestHandler<LoginCommand, LoginResponseDto>
{
    private readonly IIdentityService _identityService;

    public LoginCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<LoginResponseDto> Handle(
        LoginCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _identityService.LoginAsync(
            request.Email,
            request.Password);

        if (!result.Succeeded)
        {
            throw new BadRequestException(
                string.Join(", ", result.Errors));
        }

        return new LoginResponseDto
        {
            AccessToken = result.Token!.AccessToken,
            RefreshToken = result.Token.RefreshToken,
            ExpiresAt = DateTime.UtcNow.AddMinutes(60)
        };
    }
}