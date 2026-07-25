using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetManagerDashboard;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.Domain.Enums;
using EnterpriseLeaveManagement.UnitTests.Common;
using Moq;
using Xunit;

namespace EnterpriseLeaveManagement.UnitTests.Dashboard.Queries.GetManagerDashboard;

public class GetManagerDashboardQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Manager_Dashboard()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var userId = Guid.NewGuid();
        var managerId = Guid.NewGuid();
        var employeeId1 = Guid.NewGuid();
        var employeeId2 = Guid.NewGuid();
        var today = DateOnly.FromDateTime(DateTime.Today);

        var manager = new Employee
        {
            Id = managerId,
            UserId = userId
        };

        context.Employees.Add(manager);

        context.Employees.AddRange(
            new Employee
            {
                Id = employeeId1,
                ManagerId = managerId
            },
            new Employee
            {
                Id = employeeId2,
                ManagerId = managerId
            });

        context.LeaveRequests.AddRange(
            new LeaveRequest
            {
                Id = Guid.NewGuid(),
                EmployeeId = employeeId1,
                Status = LeaveRequestStatus.Pending,
                StartDate = today,
                EndDate = today
            },
            new LeaveRequest
            {
                Id = Guid.NewGuid(),
                EmployeeId = employeeId1,
                Status = LeaveRequestStatus.Approved,
                StartDate = today,
                EndDate = today
            },
            new LeaveRequest
            {
                Id = Guid.NewGuid(),
                EmployeeId = employeeId2,
                Status = LeaveRequestStatus.Rejected,
                StartDate = today,
                EndDate = today
            });

        await context.SaveChangesAsync();

        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService
            .Setup(x => x.UserId)
            .Returns(userId);

        var handler = new GetManagerDashboardQueryHandler(
            context,
            currentUserService.Object);

        // Act
        var result = await handler.Handle(
            new GetManagerDashboardQuery(),
            CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.TeamSize);
        Assert.Equal(1, result.PendingApprovals);
        Assert.Equal(1, result.ApprovedThisMonth);
        Assert.Equal(1, result.RejectedThisMonth);
        Assert.Equal(1, result.EmployeesOnLeaveToday);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Manager_Profile_Not_Found()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService
            .Setup(x => x.UserId)
            .Returns(Guid.NewGuid());

        var handler = new GetManagerDashboardQueryHandler(
            context,
            currentUserService.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            handler.Handle(
                new GetManagerDashboardQuery(),
                CancellationToken.None));

        Assert.Equal("Manager profile not found.", exception.Message);
    }
}