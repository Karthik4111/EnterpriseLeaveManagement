using EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetAdminDashboard;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.Domain.Enums;
using EnterpriseLeaveManagement.UnitTests.Common;
using Xunit;

namespace EnterpriseLeaveManagement.UnitTests.Dashboard.Queries.GetAdminDashboard;

public class GetAdminDashboardQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Admin_Dashboard()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var today = DateOnly.FromDateTime(DateTime.Today);

        context.Departments.Add(new Department
        {
            Id = Guid.NewGuid(),
            Name = "IT",
            Code = "IT"
        });

        context.Employees.AddRange(
            new Employee { Id = Guid.NewGuid() },
            new Employee { Id = Guid.NewGuid() });

        context.LeaveTypes.AddRange(
            new LeaveType
            {
                Id = Guid.NewGuid(),
                Name = "Annual Leave",
                Code = "AL"
            },
            new LeaveType
            {
                Id = Guid.NewGuid(),
                Name = "Sick Leave",
                Code = "SL"
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

        var handler = new GetAdminDashboardQueryHandler(context);

        // Act
        var result = await handler.Handle(
            new GetAdminDashboardQuery(),
            CancellationToken.None);

        // Assert
        Assert.NotNull(result);

        Assert.Equal(2, result.TotalEmployees);
        Assert.Equal(1, result.TotalDepartments);
        Assert.Equal(2, result.TotalLeaveTypes);
        Assert.Equal(3, result.TotalLeaveRequests);

        Assert.Equal(1, result.PendingLeaveRequests);
        Assert.Equal(1, result.ApprovedLeaveRequests);
        Assert.Equal(1, result.RejectedLeaveRequests);

        Assert.Equal(1, result.EmployeesOnLeaveToday);
    }
}