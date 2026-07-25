using EnterpriseLeaveManagement.Application.Common.Exceptions;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.Authentication.Commands.Register;
using Moq;
using Xunit;

namespace EnterpriseLeaveManagement.UnitTests.Authentication.Commands;

public class RegisterCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Success_Message_When_Registration_Is_Successful()
    {
        // Arrange
        var identityServiceMock = new Mock<IIdentityService>();

        var command = new RegisterCommand
        {
            FirstName = "Gopi",
            LastName = "Krishna",
            UserName = "gopi",
            Email = "gopi@company.com",
            Password = "Password@123",
            Role = "Employee"
        };

        identityServiceMock
            .Setup(x => x.RegisterUserAsync(
                command.FirstName,
                command.LastName,
                command.UserName,
                command.Email,
                command.Password,
                command.Role))
            .ReturnsAsync((
                true,
                Guid.NewGuid(),
                Enumerable.Empty<string>()));

        var handler = new RegisterCommandHandler(identityServiceMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.Equal("User registered successfully.", result);

        identityServiceMock.Verify(
            x => x.RegisterUserAsync(
                command.FirstName,
                command.LastName,
                command.UserName,
                command.Email,
                command.Password,
                command.Role),
            Times.Once);
    }

    [Fact]
    public async Task Handle_Should_Throw_BadRequestException_When_Registration_Fails()
    {
        // Arrange
        var identityServiceMock = new Mock<IIdentityService>();

        var command = new RegisterCommand
        {
            FirstName = "Gopi",
            LastName = "Krishna",
            UserName = "gopi",
            Email = "gopi@company.com",
            Password = "Password@123",
            Role = "Employee"
        };

        identityServiceMock
            .Setup(x => x.RegisterUserAsync(
                command.FirstName,
                command.LastName,
                command.UserName,
                command.Email,
                command.Password,
                command.Role))
            .ReturnsAsync((
                false,
                (Guid?)null,
                new[] { "User already exists." }.AsEnumerable()));

        var handler = new RegisterCommandHandler(identityServiceMock.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BadRequestException>(() =>
            handler.Handle(command, CancellationToken.None));

        Assert.Equal("User already exists.", exception.Message);

        identityServiceMock.Verify(
            x => x.RegisterUserAsync(
                command.FirstName,
                command.LastName,
                command.UserName,
                command.Email,
                command.Password,
                command.Role),
            Times.Once);
    }
}