using EnterpriseLeaveManagement.Application.Common.Exceptions;
using EnterpriseLeaveManagement.Application.Features.Departments.Commands.DeleteDepartment;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.UnitTests.Common;
using Xunit;

namespace EnterpriseLeaveManagement.UnitTests.Departments.Commands;

public class DeleteDepartmentCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Delete_Department_Successfully()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var department = new Department
        {
            Id = Guid.NewGuid(),
            Name = "Information Technology",
            Code = "IT",
            Description = "IT Department",
            IsActive = true
        };

        context.Departments.Add(department);
        await context.SaveChangesAsync();

        var command = new DeleteDepartmentCommand
        {
            Id = department.Id
        };

        var handler = new DeleteDepartmentCommandHandler(context);

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Department deleted successfully.", result.Message);

        var deletedDepartment = await context.Departments.FindAsync(department.Id);

        Assert.NotNull(deletedDepartment);
        Assert.False(deletedDepartment!.IsActive);
    }


    [Fact]
    public async Task Handle_Should_Throw_NotFoundException_When_Department_Does_Not_Exist()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var command = new DeleteDepartmentCommand
        {
            Id = Guid.NewGuid()
        };

        var handler = new DeleteDepartmentCommandHandler(context);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(command, CancellationToken.None));

        Assert.Equal("Department not found.", exception.Message);
    }

}