using EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetDepartmentStatistics;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.UnitTests.Common;
using Xunit;

namespace EnterpriseLeaveManagement.UnitTests.Dashboard.Queries.GetDepartmentStatistics;

public class GetDepartmentStatisticsQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Department_Statistics()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var department1 = new Department
        {
            Id = Guid.NewGuid(),
            Name = "IT",
            Code = "IT"
        };

        var department2 = new Department
        {
            Id = Guid.NewGuid(),
            Name = "HR",
            Code = "HR"
        };

        context.Departments.AddRange(department1, department2);

        context.Employees.AddRange(
            new Employee
            {
                Id = Guid.NewGuid(),
                DepartmentId = department1.Id
            },
            new Employee
            {
                Id = Guid.NewGuid(),
                DepartmentId = department1.Id
            },
            new Employee
            {
                Id = Guid.NewGuid(),
                DepartmentId = department2.Id
            });

        await context.SaveChangesAsync();

        var handler = new GetDepartmentStatisticsQueryHandler(context);

        // Act
        var result = await handler.Handle(
            new GetDepartmentStatisticsQuery(),
            CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        var it = result.Single(x => x.DepartmentName == "IT");
        var hr = result.Single(x => x.DepartmentName == "HR");

        Assert.Equal(2, it.EmployeeCount);
        Assert.Equal(1, hr.EmployeeCount);
    }
}