using EnterpriseLeaveManagement.Application.Common.Exceptions;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.Employees.Commands.CreateEmployee;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.UnitTests.Common;
using Moq;
using Xunit;

namespace EnterpriseLeaveManagement.UnitTests.Employees.Commands;

public class CreateEmployeeCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Employee_Successfully()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var identityServiceMock = new Mock<IIdentityService>();

        var department = new Department
        {
            Id = Guid.NewGuid(),
            Name = "Information Technology",
            Code = "IT",
            IsActive = true
        };

        context.Departments.Add(department);
        await context.SaveChangesAsync();

        var userId = Guid.NewGuid();

        identityServiceMock
            .Setup(x => x.UserExistsAsync(userId))
            .ReturnsAsync(true);

        var command = new CreateEmployeeCommand
        {
            EmployeeCode = "EMP001",
            FirstName = "Gopi",
            LastName = "Krishna",
            Email = "gopi@company.com",
            PhoneNumber = "9876543210",
            DateOfJoining = DateTime.Today,
            DepartmentId = department.Id,
            UserId = userId,
            Designation = "Software Engineer"
        };

        var handler = new CreateEmployeeCommandHandler(
            context,
            identityServiceMock.Object);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Employee created successfully.", result.Message);

        var employee = context.Employees.Single();

        Assert.Equal("EMP001", employee.EmployeeCode);
        Assert.Equal("Gopi", employee.FirstName);
        Assert.Equal("Krishna", employee.LastName);
        Assert.Equal("gopi@company.com", employee.Email);
        Assert.Equal(department.Id, employee.DepartmentId);
        Assert.Equal(userId, employee.UserId);

        identityServiceMock.Verify(
            x => x.UserExistsAsync(userId),
            Times.Once);
    }


    [Fact]
    public async Task Handle_Should_Throw_BusinessException_When_User_Is_Already_Linked()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var identityServiceMock = new Mock<IIdentityService>();

        var department = new Department
        {
            Id = Guid.NewGuid(),
            Name = "IT",
            Code = "IT",
            IsActive = true
        };

        var userId = Guid.NewGuid();

        context.Departments.Add(department);

        context.Employees.Add(new Employee
        {
            Id = Guid.NewGuid(),
            EmployeeCode = "EMP001",
            FirstName = "Existing",
            LastName = "Employee",
            Email = "existing@company.com",
            UserId = userId,
            DepartmentId = department.Id,
            IsActive = true
        });

        await context.SaveChangesAsync();

        identityServiceMock
            .Setup(x => x.UserExistsAsync(userId))
            .ReturnsAsync(true);

        var command = new CreateEmployeeCommand
        {
            EmployeeCode = "EMP002",
            FirstName = "Gopi",
            LastName = "Krishna",
            Email = "gopi@company.com",
            DepartmentId = department.Id,
            UserId = userId
        };

        var handler = new CreateEmployeeCommandHandler(
            context,
            identityServiceMock.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusinessException>(() =>
            handler.Handle(command, CancellationToken.None));

        Assert.Equal("User is already linked to another employee.", exception.Message);
    }


    [Fact]
    public async Task Handle_Should_Throw_BusinessException_When_User_Does_Not_Exist()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var identityServiceMock = new Mock<IIdentityService>();

        var department = new Department
        {
            Id = Guid.NewGuid(),
            Name = "IT",
            Code = "IT",
            IsActive = true
        };

        context.Departments.Add(department);
        await context.SaveChangesAsync();

        var userId = Guid.NewGuid();

        identityServiceMock
            .Setup(x => x.UserExistsAsync(userId))
            .ReturnsAsync(false);

        var command = new CreateEmployeeCommand
        {
            EmployeeCode = "EMP001",
            FirstName = "Gopi",
            LastName = "Krishna",
            Email = "gopi@company.com",
            DepartmentId = department.Id,
            UserId = userId
        };

        var handler = new CreateEmployeeCommandHandler(
            context,
            identityServiceMock.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusinessException>(() =>
            handler.Handle(command, CancellationToken.None));

        Assert.Equal("User does not exist.", exception.Message);
    }

}