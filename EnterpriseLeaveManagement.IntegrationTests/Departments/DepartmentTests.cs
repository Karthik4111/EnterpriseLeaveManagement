using EnterpriseLeaveManagement.Application.Common.Models;
using EnterpriseLeaveManagement.Application.Features.Authentication.Commands.Login;
using EnterpriseLeaveManagement.Application.Features.Authentication.DTOs;
using EnterpriseLeaveManagement.Application.Features.Departments.Commands.CreateDepartment;
using EnterpriseLeaveManagement.Application.Features.Departments.Commands.UpdateDepartment;
using EnterpriseLeaveManagement.Application.Features.Departments.Common;
using EnterpriseLeaveManagement.IntegrationTests.Common;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Xunit;

namespace EnterpriseLeaveManagement.IntegrationTests.Departments;

public class DepartmentTests : IClassFixture<CustomWebApplicationFactory>
{
    private readonly HttpClient _client;

    public DepartmentTests(CustomWebApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateDepartment_Should_Return_Ok_When_Request_Is_Valid()
    {
        await AuthenticateAsAdminAsync();
        // Arrange
        var request = new CreateDepartmentCommand
        {
            Name = $"Human Resources {Guid.NewGuid():N}",
            Code = $"HR{Random.Shared.Next(1000, 9999)}",
            Description = "Human Resources Department"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/departments",
            request);

        var body = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(body));
    }

    [Fact]
    public async Task CreateDepartment_Should_Return_BadRequest_When_Name_Is_Empty()
    {
        await AuthenticateAsAdminAsync();
        // Arrange
        var request = new CreateDepartmentCommand
        {
            Name = string.Empty,
            Code = "HR001",
            Description = "Department"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/departments",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task GetDepartments_Should_Return_All_Departments()
    {
        await AuthenticateAsAdminAsync();
        // Arrange
        var request = new CreateDepartmentCommand
        {
            Name = $"Finance {Guid.NewGuid():N}",
            Code = $"FIN{Random.Shared.Next(1000, 9999)}",
            Description = "Finance Department"
        };

        await _client.PostAsJsonAsync(
            "/api/departments",
            request);

        // Act
        var response = await _client.GetAsync("/api/departments");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var result = await response.Content.ReadFromJsonAsync<PagedResult<DepartmentDto>>();

        Assert.NotNull(result);
        Assert.NotEmpty(result!.Items);
    }





    [Fact]
    public async Task GetDepartmentById_Should_Return_Department_When_Exists()
    {
        await AuthenticateAsAdminAsync();
        // Arrange
        var create = new CreateDepartmentCommand
        {
            Name = $"IT {Guid.NewGuid():N}",
            Code = $"IT{Random.Shared.Next(1000, 9999)}",
            Description = "Information Technology"
        };

        var createResponse = await _client.PostAsJsonAsync(
            "/api/departments",
            create);

        Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);

        var createdDepartment =
            await createResponse.Content.ReadFromJsonAsync<CreateDepartmentResponse>();

        Assert.NotNull(createdDepartment);

        // Act
        var response = await _client.GetAsync(
            $"/api/departments/{createdDepartment!.DepartmentId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        var department =
            await response.Content.ReadFromJsonAsync<DepartmentDto>();

        Assert.NotNull(department);
        Assert.Equal(create.Name, department!.Name);
        Assert.Equal(create.Code, department.Code);
    }

    [Fact]
    public async Task GetDepartmentById_Should_Return_NotFound_When_Department_Does_Not_Exist()
    {
        await AuthenticateAsAdminAsync();
        // Act
        var response = await _client.GetAsync(
            $"/api/departments/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task UpdateDepartment_Should_Return_Ok_When_Request_Is_Valid()
    {
        await AuthenticateAsAdminAsync();
        // Arrange
        var create = new CreateDepartmentCommand
        {
            Name = $"Sales {Guid.NewGuid():N}",
            Code = $"SAL{Random.Shared.Next(1000, 9999)}",
            Description = "Sales Department"
        };

        var createResponse = await _client.PostAsJsonAsync(
            "/api/departments",
            create);

        Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);

        var createdDepartment =
            await createResponse.Content.ReadFromJsonAsync<CreateDepartmentResponse>();

        Assert.NotNull(createdDepartment);

        var update = new UpdateDepartmentCommand
        {
            Id = createdDepartment!.DepartmentId,
            Name = "Updated Sales",
            Code = "SAL999",
            Description = "Updated Description"
        };

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/departments/{update.Id}",
            update);

        var body = await response.Content.ReadAsStringAsync();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.False(string.IsNullOrWhiteSpace(body));
    }

    [Fact]
    public async Task UpdateDepartment_Should_Return_BadRequest_When_RouteId_Does_Not_Match_RequestId()
    {
        await AuthenticateAsAdminAsync();
        // Arrange
        var request = new UpdateDepartmentCommand
        {
            Id = Guid.NewGuid(),
            Name = "Finance",
            Code = "FIN001",
            Description = "Finance Department"
        };

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/departments/{Guid.NewGuid()}",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }


    [Fact]
    public async Task DeleteDepartment_Should_Return_Ok_When_Department_Exists()
    {
        await AuthenticateAsAdminAsync();
        // Arrange
        var create = new CreateDepartmentCommand
        {
            Name = $"Testing {Guid.NewGuid():N}",
            Code = $"TST{Random.Shared.Next(1000, 9999)}",
            Description = "Testing Department"
        };

        var createResponse = await _client.PostAsJsonAsync(
            "/api/departments",
            create);

        Assert.Equal(HttpStatusCode.OK, createResponse.StatusCode);

        var createdDepartment =
            await createResponse.Content.ReadFromJsonAsync<CreateDepartmentResponse>();

        Assert.NotNull(createdDepartment);

        // Act
        var response = await _client.DeleteAsync(
            $"/api/departments/{createdDepartment!.DepartmentId}");

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task DeleteDepartment_Should_Return_NotFound_When_Department_Does_Not_Exist()
    {
        await AuthenticateAsAdminAsync();
        // Act
        var response = await _client.DeleteAsync(
            $"/api/departments/{Guid.NewGuid()}");

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateDepartment_Should_Return_Conflict_When_Department_Already_Exists()
    {
        await AuthenticateAsAdminAsync();
        // Arrange
        var request = new CreateDepartmentCommand
        {
            Name = "Human Resources",
            Code = "HR001",
            Description = "Human Resources Department"
        };

        var firstResponse = await _client.PostAsJsonAsync(
            "/api/departments",
            request);

        Assert.Equal(HttpStatusCode.OK, firstResponse.StatusCode);

        // Act
        var secondResponse = await _client.PostAsJsonAsync(
            "/api/departments",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.Conflict, secondResponse.StatusCode);
    }

    [Fact]
    public async Task UpdateDepartment_Should_Return_NotFound_When_Department_Does_Not_Exist()
    {
        await AuthenticateAsAdminAsync();
        // Arrange
        var request = new UpdateDepartmentCommand
        {
            Id = Guid.NewGuid(),
            Name = "Finance",
            Code = "FIN001",
            Description = "Finance Department"
        };

        // Act
        var response = await _client.PutAsJsonAsync(
            $"/api/departments/{request.Id}",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task CreateDepartment_Should_Return_BadRequest_When_Code_Is_Empty()
    {
        await AuthenticateAsAdminAsync();
        // Arrange
        var request = new CreateDepartmentCommand
        {
            Name = "Finance",
            Code = string.Empty,
            Description = "Finance Department"
        };

        // Act
        var response = await _client.PostAsJsonAsync(
            "/api/departments",
            request);

        // Assert
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }




    private bool _isAuthenticated;

    private async Task AuthenticateAsAdminAsync()
    {
        if (_isAuthenticated)
            return;

        var login = new LoginCommand
        {
            Email = "admin@company.com",
            Password = "Admin@123"
        };

        var response = await _client.PostAsJsonAsync(
            "/api/authentication/login",
            login);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<LoginResponseDto>();

        Assert.NotNull(result);

        _client.DefaultRequestHeaders.Authorization =
            new AuthenticationHeaderValue("Bearer", result!.AccessToken);

        _isAuthenticated = true;
    }
}