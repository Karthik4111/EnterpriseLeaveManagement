using EnterpriseLeaveManagement.Application.Features.Authentication.Commands.Login;
using EnterpriseLeaveManagement.Application.Features.Authentication.Commands.Register;
using EnterpriseLeaveManagement.Application.Features.Authentication.DTOs;
using EnterpriseLeaveManagement.Application.Features.Authentication.Commands.RefreshToken;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace EnterpriseLeaveManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthenticationController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register(RegisterCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpPost("login")]
    public async Task<ActionResult<LoginResponseDto>> Login(
    LoginCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }

    [HttpPost("refresh-token")]
    public async Task<ActionResult<LoginResponseDto>> RefreshToken(RefreshTokenCommand command)
    {
        var result = await _mediator.Send(command);

        return Ok(result);
    }
}