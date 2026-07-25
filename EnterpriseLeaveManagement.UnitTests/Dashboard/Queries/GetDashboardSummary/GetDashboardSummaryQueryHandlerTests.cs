using EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetDashboardSummary;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.Domain.Enums;
using EnterpriseLeaveManagement.UnitTests.Common;
using Xunit;

namespace EnterpriseLeaveManagement.UnitTests.Dashboard.Queries.GetDashboardSummary;

public class GetDashboardSummaryQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Dashboard_Summary()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var today = DateOnly.FromDateTime(DateTime.UtcNow);

        context.Departments.Add(new Department
        {
            Id = Guid.NewGuid(),
            Name = "IT",
            Code = "IT"
        });

        context.Employees.AddRange(
            new Employee
            {
                Id = Guid.NewGuid()
            },
            new Employee
            {
                Id = Guid.NewGuid()
            });

        context.LeaveRequests.AddRange(
            new LeaveRequest
            {
                Id = Guid.NewGuid(),
                Status = LeaveRequestStatus.Pending,
                StartDate = today,
                EndDate = today
            },
            new LeaveRequest
            {
                Id = Guid.NewGuid(),
                Status = LeaveRequestStatus.Approved,
                StartDate = today,
                EndDate = today
            },
            new LeaveRequest
            {
                Id = Guid.NewGuid(),
                Status = LeaveRequestStatus.Rejected,
                StartDate = today,
                EndDate = today
            });

        await context.SaveChangesAsync();

        var handler = new GetDashboardSummaryQueryHandler(context);

        // Act
        var result = await handler.Handle(
            new GetDashboardSummaryQuery(),
            CancellationToken.None);

        // Assert
        Assert.NotNull(result);

        Assert.Equal(2, result.TotalEmployees);
        Assert.Equal(1, result.TotalDepartments);
        Assert.Equal(3, result.TotalLeaveRequests);

        Assert.Equal(1, result.PendingRequests);
        Assert.Equal(1, result.ApprovedRequests);
        Assert.Equal(1, result.RejectedRequests);

        Assert.Equal(1, result.EmployeesOnLeaveToday);
    }
}