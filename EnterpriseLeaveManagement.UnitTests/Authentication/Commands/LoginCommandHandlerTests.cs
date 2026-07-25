using EnterpriseLeaveManagement.Application.Common.Exceptions;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.Authentication.Commands.Login;
using EnterpriseLeaveManagement.Application.Features.Authentication.DTOs;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
namespace EnterpriseLeaveManagement.UnitTests.Authentication.Commands;

public class LoginCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_LoginResponse_When_Credentials_Are_Valid()
    {
        // Arrange
        var identityServiceMock = new Mock<IIdentityService>();

        var loginCommand = new LoginCommand
        {
            Email = "gopi@company.com",
            Password = "Password@123"
        };

        var expectedToken = new TokenResponseDto
        {
            AccessToken = "access-token",
            RefreshToken = "refresh-token"
        };

        identityServiceMock
            .Setup(x => x.LoginAsync(
                loginCommand.Email,
                loginCommand.Password))
            .ReturnsAsync((
                true,
                expectedToken,
                Enumerable.Empty<string>()));

        var handler = new LoginCommandHandler(
            identityServiceMock.Object);

        // Act
        var result = await handler.Handle(
            loginCommand,
            CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("access-token", result.AccessToken);
        Assert.Equal("refresh-token", result.RefreshToken);

        identityServiceMock.Verify(
            x => x.LoginAsync(
                loginCommand.Email,
                loginCommand.Password),
            Times.Once);
    }



    [Fact]
    public async Task Handle_Should_Throw_BadRequestException_When_Credentials_Are_Invalid()
    {
        // Arrange
        var identityServiceMock = new Mock<IIdentityService>();

        var loginCommand = new LoginCommand
        {
            Email = "gopi@company.com",
            Password = "WrongPassword"
        };

        identityServiceMock
            .Setup(x => x.LoginAsync(
                loginCommand.Email,
                loginCommand.Password))
            .ReturnsAsync((
                false,
                null,
                new[] { "Invalid email or password." }.AsEnumerable()));

        var handler = new LoginCommandHandler(identityServiceMock.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BadRequestException>(() =>
            handler.Handle(loginCommand, CancellationToken.None));

        Assert.Equal("Invalid email or password.", exception.Message);

        identityServiceMock.Verify(
            x => x.LoginAsync(
                loginCommand.Email,
                loginCommand.Password),
            Times.Once);
    }

}