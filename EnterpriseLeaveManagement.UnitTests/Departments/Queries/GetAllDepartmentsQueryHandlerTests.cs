using EnterpriseLeaveManagement.Application.Features.Departments.Queries.GetAllDepartments;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.UnitTests.Common;
using Xunit;

namespace EnterpriseLeaveManagement.UnitTests.Departments.Queries;

public class GetAllDepartmentsQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_All_Departments()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        context.Departments.AddRange(
            new Department
            {
                Id = Guid.NewGuid(),
                Name = "Finance",
                Code = "FIN",
                Description = "Finance Department",
                IsActive = true
            },
            new Department
            {
                Id = Guid.NewGuid(),
                Name = "Information Technology",
                Code = "IT",
                Description = "IT Department",
                IsActive = true
            });

        await context.SaveChangesAsync();

        var handler = new GetAllDepartmentsQueryHandler(context);

        var query = new GetAllDepartmentsQuery();

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.TotalCount);
        Assert.Equal(2, result.Items.Count);
    }

    [Fact]
    public async Task Handle_Should_Filter_Active_Departments()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        context.Departments.AddRange(
            new Department
            {
                Id = Guid.NewGuid(),
                Name = "Finance",
                Code = "FIN",
                IsActive = true
            },
            new Department
            {
                Id = Guid.NewGuid(),
                Name = "HR",
                Code = "HR",
                IsActive = false
            });

        await context.SaveChangesAsync();

        var handler = new GetAllDepartmentsQueryHandler(context);

        var query = new GetAllDepartmentsQuery
        {
            IsActive = true
        };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Single(result.Items);
        Assert.Equal("Finance", result.Items.First().Name);
    }

    [Fact]
    public async Task Handle_Should_Search_Departments()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        context.Departments.AddRange(
            new Department
            {
                Id = Guid.NewGuid(),
                Name = "Finance",
                Code = "FIN",
                IsActive = true
            },
            new Department
            {
                Id = Guid.NewGuid(),
                Name = "Information Technology",
                Code = "IT",
                IsActive = true
            });

        await context.SaveChangesAsync();

        var handler = new GetAllDepartmentsQueryHandler(context);

        var query = new GetAllDepartmentsQuery
        {
            Search = "Finance"
        };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Single(result.Items);
        Assert.Equal("Finance", result.Items.First().Name);
    }

    [Fact]
    public async Task Handle_Should_Return_Paged_Result()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        for (int i = 1; i <= 5; i++)
        {
            context.Departments.Add(new Department
            {
                Id = Guid.NewGuid(),
                Name = $"Department {i}",
                Code = $"D{i}",
                IsActive = true
            });
        }

        await context.SaveChangesAsync();

        var handler = new GetAllDepartmentsQueryHandler(context);

        var query = new GetAllDepartmentsQuery
        {
            PageNumber = 2,
            PageSize = 2
        };

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.Equal(5, result.TotalCount);
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(2, result.PageNumber);
        Assert.Equal(2, result.PageSize);
    }
}