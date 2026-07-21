using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseLeaveManagement.Application.Features.Authentication.DTOs;
using MediatR;

namespace EnterpriseLeaveManagement.Application.Features.Authentication.Commands.Login;

public class LoginCommand : IRequest<LoginResponseDto>
{
    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;
}