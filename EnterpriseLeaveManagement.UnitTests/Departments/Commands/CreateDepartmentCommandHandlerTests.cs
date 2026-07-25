using EnterpriseLeaveManagement.Application.Common.Exceptions;
using EnterpriseLeaveManagement.Application.Features.Departments.Commands.CreateDepartment;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.UnitTests.Common;
using Xunit;

namespace EnterpriseLeaveManagement.UnitTests.Departments.Commands;

public class CreateDepartmentCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_Department_Successfully()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var command = new CreateDepartmentCommand
        {
            Name = "Information Technology",
            Code = "IT",
            Description = "IT Department",
            ManagerId = Guid.NewGuid()
        };

        var handler = new CreateDepartmentCommandHandler(context);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Department created successfully.", result.Message);

        var department = context.Departments.Single();

        Assert.Equal("Information Technology", department.Name);
        Assert.Equal("IT", department.Code);
        Assert.Equal("IT Department", department.Description);
        Assert.Equal(command.ManagerId, department.ManagerId);
        Assert.True(department.IsActive);
    }

    [Fact]
    public async Task Handle_Should_Throw_BusinessException_When_Department_Name_Already_Exists()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        context.Departments.Add(new Department
        {
            Id = Guid.NewGuid(),
            Name = "Information Technology",
            Code = "IT",
            Description = "Department",
            IsActive = true
        });

        await context.SaveChangesAsync();

        var command = new CreateDepartmentCommand
        {
            Name = "Information Technology",
            Code = "HR",
            Description = "Duplicate Name"
        };

        var handler = new CreateDepartmentCommandHandler(context);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusinessException>(() =>
            handler.Handle(command, CancellationToken.None));

        Assert.Equal("Department name already exists.", exception.Message);
    }


    [Fact]
    public async Task Handle_Should_Throw_BusinessException_When_Department_Code_Already_Exists()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        context.Departments.Add(new Department
        {
            Id = Guid.NewGuid(),
            Name = "Information Technology",
            Code = "IT",
            Description = "Department",
            IsActive = true
        });

        await context.SaveChangesAsync();

        var command = new CreateDepartmentCommand
        {
            Name = "Human Resources",
            Code = "IT",
            Description = "Duplicate Code"
        };

        var handler = new CreateDepartmentCommandHandler(context);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusinessException>(() =>
            handler.Handle(command, CancellationToken.None));

        Assert.Equal("Department code already exists.", exception.Message);
    }

}