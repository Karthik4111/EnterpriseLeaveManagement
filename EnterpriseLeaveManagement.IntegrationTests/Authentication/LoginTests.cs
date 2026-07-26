using System.Net;
using System.Net.Http.Json;
using EnterpriseLeaveManagement.Application.Features.Authentication.Commands.Login;
using EnterpriseLeaveManagement.Application.Features.Authentication.Commands.Register;
using EnterpriseLeaveManagement.IntegrationTests.Common;
using Xunit;

namespace EnterpriseLeaveManagement.IntegrationTests.Authentication;

public class LoginTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public LoginTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Login_Should_Return_Token_When_Credentials_Are_Valid()
    {
        // Arrange
        var register = new RegisterCommand
        {
            FirstName = "Gopi",
            LastName = "Kumar",
            UserName = $"gopi{Guid.NewGuid():N}",
            Email = $"gopi{Guid.NewGuid():N}@test.com",
            Password = "Password@123",
            ConfirmPassword = "Password@123",
            Role = "Employee",

            DepartmentId = Guid.NewGuid(),
            EmployeeCode = "EMP001",
            PhoneNumber = "9876543210",
            DateOfBirth = new DateTime(2001, 11, 4),
            DateOfJoining = DateTime.UtcNow,
            Designation = "Software Engineer"
        };

        await _client.PostAsJsonAsync(
            "/api/authentication/register",
            register);

        var login = new LoginCommand
        {
            Email = register.Email,
            Password = register.Password
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/authentication/login",
            login);

        var body = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(body));
    }


    [Fact]
    public async Task Login_Should_Return_Unauthorized_When_Password_Is_Invalid()
    {
        // Arrange
        var register = new RegisterCommand
        {
            FirstName = "Gopi",
            LastName = "Kumar",
            UserName = $"gopi{Guid.NewGuid():N}",
            Email = $"gopi{Guid.NewGuid():N}@test.com",
            Password = "Password@123",
            ConfirmPassword = "Password@123",
            Role = "Employee",

            DepartmentId = Guid.NewGuid(),
            EmployeeCode = "EMP001",
            PhoneNumber = "9876543210",
            DateOfBirth = new DateTime(2001, 11, 4),
            DateOfJoining = DateTime.UtcNow,
            Designation = "Software Engineer"
        };

        var registerResponse = await _client.PostAsJsonAsync(
            "/api/authentication/register",
            register);

        Assert.Equal(HttpStatusCode.OK, registerResponse.StatusCode);

        var login = new LoginCommand
        {
            Email = register.Email,
            Password = "WrongPassword@123"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/authentication/login",
            login);

        var body = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Contains("Invalid", body, StringComparison.OrdinalIgnoreCase);
    }

    [Fact]
    public async Task Login_Should_Return_Unauthorized_When_Email_Does_Not_Exist()
    {
        // Arrange
        var login = new LoginCommand
        {
            Email = $"unknown{Guid.NewGuid():N}@test.com",
            Password = "Password@123"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/authentication/login",
            login);

        var body = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        Assert.Contains("Invalid", body, StringComparison.OrdinalIgnoreCase);
    }



    [Fact]
    public async Task Login_Should_Return_BadRequest_When_Email_Is_Empty()
    {
        // Arrange
        var request = new LoginCommand
        {
            Email = string.Empty,
            Password = "Password@123"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/authentication/login",
            request);

        var body = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("Email", body, StringComparison.OrdinalIgnoreCase);
    }


    [Fact]
    public async Task Login_Should_Return_BadRequest_When_Password_Is_Empty()
    {
        // Arrange
        var request = new LoginCommand
        {
            Email = "gopi@test.com",
            Password = string.Empty
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/authentication/login",
            request);

        var body = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        Assert.Contains("Password", body, StringComparison.OrdinalIgnoreCase);
    }
}