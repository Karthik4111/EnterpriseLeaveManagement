using EnterpriseLeaveManagement.Application.Common.Exceptions;
using EnterpriseLeaveManagement.Application.Features.Departments.Commands.UpdateDepartment;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.UnitTests.Common;
using Xunit;

namespace EnterpriseLeaveManagement.UnitTests.Departments.Commands;

public class UpdateDepartmentCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Update_Department_Successfully()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var department = new Department
        {
            Id = Guid.NewGuid(),
            Name = "HR",
            Code = "HR",
            Description = "Human Resources",
            IsActive = true
        };

        context.Departments.Add(department);
        await context.SaveChangesAsync();

        var command = new UpdateDepartmentCommand
        {
            Id = department.Id,
            Name = "Information Technology",
            Code = "IT",
            Description = "Updated Department",
            ManagerId = Guid.NewGuid()
        };

        var handler = new UpdateDepartmentCommandHandler(context);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(department.Id, result.DepartmentId);
        Assert.Equal("Department updated successfully.", result.Message);

        var updatedDepartment = await context.Departments.FindAsync(department.Id);

        Assert.NotNull(updatedDepartment);
        Assert.Equal("Information Technology", updatedDepartment!.Name);
        Assert.Equal("IT", updatedDepartment.Code);
        Assert.Equal("Updated Department", updatedDepartment.Description);
        Assert.Equal(command.ManagerId, updatedDepartment.ManagerId);
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFoundException_When_Department_Does_Not_Exist()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var command = new UpdateDepartmentCommand
        {
            Id = Guid.NewGuid(),
            Name = "Information Technology",
            Code = "IT",
            Description = "Updated Department"
        };

        var handler = new UpdateDepartmentCommandHandler(context);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(command, CancellationToken.None));

        Assert.Equal("Department not found.", exception.Message);
    }

    [Fact]
    public async Task Handle_Should_Throw_BusinessException_When_Department_Name_Already_Exists()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var department1 = new Department
        {
            Id = Guid.NewGuid(),
            Name = "HR",
            Code = "HR",
            Description = "Human Resources",
            IsActive = true
        };

        var department2 = new Department
        {
            Id = Guid.NewGuid(),
            Name = "Finance",
            Code = "FIN",
            Description = "Finance",
            IsActive = true
        };

        context.Departments.AddRange(department1, department2);
        await context.SaveChangesAsync();

        var command = new UpdateDepartmentCommand
        {
            Id = department2.Id,
            Name = "HR",
            Code = "FIN",
            Description = "Updated"
        };

        var handler = new UpdateDepartmentCommandHandler(context);

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

        var department1 = new Department
        {
            Id = Guid.NewGuid(),
            Name = "HR",
            Code = "HR",
            Description = "Human Resources",
            IsActive = true
        };

        var department2 = new Department
        {
            Id = Guid.NewGuid(),
            Name = "Finance",
            Code = "FIN",
            Description = "Finance",
            IsActive = true
        };

        context.Departments.AddRange(department1, department2);
        await context.SaveChangesAsync();

        var command = new UpdateDepartmentCommand
        {
            Id = department2.Id,
            Name = "Finance",
            Code = "HR",
            Description = "Updated"
        };

        var handler = new UpdateDepartmentCommandHandler(context);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<BusinessException>(() =>
            handler.Handle(command, CancellationToken.None));

        Assert.Equal("Department code already exists.", exception.Message);
    }

}