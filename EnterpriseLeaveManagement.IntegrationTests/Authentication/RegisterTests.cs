using EnterpriseLeaveManagement.Application.Features.Authentication.Commands.Register;
using EnterpriseLeaveManagement.IntegrationTests.Common;
using System;
using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace EnterpriseLeaveManagement.IntegrationTests.Authentication;

public class RegisterTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public RegisterTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_Should_Return_Ok_When_Request_Is_Valid()
    {
        var request = new RegisterCommand
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

        var response = await _client.PostAsJsonAsync(
            "/api/authentication/register",
            request);

        var body = await response.Content.ReadAsStringAsync();

        Assert.True(
            response.IsSuccessStatusCode,
            $"Status: {response.StatusCode}\n\n{body}");
    }

    
    [Fact]
    public async Task Register_Should_Return_BadRequest_When_Email_Already_Exists()
    {
        var request = new RegisterCommand
        {
            FirstName = "Gopi",
            LastName = "Kumar",
            UserName = $"gopi{Guid.NewGuid():N}",
            Email = "duplicate@test.com",
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

        // First registration
        var firstResponse = await _client.PostAsJsonAsync(
            "/api/authentication/register",
            request);

        Assert.True(firstResponse.IsSuccessStatusCode);

        // Second registration
        var secondResponse = await _client.PostAsJsonAsync(
            "/api/authentication/register",
            request);

        var secondBody = await secondResponse.Content.ReadAsStringAsync();

        Assert.Equal(HttpStatusCode.BadRequest, secondResponse.StatusCode);
        Assert.Contains("User already exists", secondBody);
    }
}