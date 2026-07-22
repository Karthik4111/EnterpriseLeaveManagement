using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseLeaveManagement.Application.Common.Exceptions;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using MediatR;

namespace EnterpriseLeaveManagement.Application.Features.Authentication.Commands.Register;

public class RegisterCommandHandler : IRequestHandler<RegisterCommand, string>
{

    private readonly IIdentityService _identityService;

    public RegisterCommandHandler(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    public async Task<string> Handle(
        RegisterCommand request,
        CancellationToken cancellationToken)
    {
        var result = await _identityService.RegisterUserAsync(
            request.FirstName,
            request.LastName,
            request.UserName,
            request.Email,
            request.Password,
            request.Role);

        if (!result.Succeeded)
        {
            throw new BadRequestException(string.Join(", ", result.Errors));
        }

        return "User registered successfully.";
    }
}
