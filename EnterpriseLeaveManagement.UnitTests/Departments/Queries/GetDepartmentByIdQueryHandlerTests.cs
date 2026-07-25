using EnterpriseLeaveManagement.Application.Features.Departments.Queries.GetDepartmentById;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.UnitTests.Common;
using Xunit;

namespace EnterpriseLeaveManagement.UnitTests.Departments.Queries;

public class GetDepartmentByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Department_When_Department_Exists()
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

        var handler = new GetDepartmentByIdQueryHandler(context);

        var query = new GetDepartmentByIdQuery(department.Id);

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(department.Id, result!.Id);
        Assert.Equal("Information Technology", result.Name);
        Assert.Equal("IT", result.Code);
        Assert.Equal("IT Department", result.Description);
        Assert.True(result.IsActive);
    }

    [Fact]
    public async Task Handle_Should_Return_Null_When_Department_Does_Not_Exist()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var handler = new GetDepartmentByIdQueryHandler(context);

        var query = new GetDepartmentByIdQuery(Guid.NewGuid());

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Null(result);
    }
}