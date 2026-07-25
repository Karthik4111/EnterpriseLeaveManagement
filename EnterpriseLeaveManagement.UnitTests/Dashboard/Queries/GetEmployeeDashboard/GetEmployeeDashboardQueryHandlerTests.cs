using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetEmployeeDashboard;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.Domain.Enums;
using EnterpriseLeaveManagement.UnitTests.Common;
using Moq;
using Xunit;

namespace EnterpriseLeaveManagement.UnitTests.Dashboard.Queries.GetEmployeeDashboard;

public class GetEmployeeDashboardQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Employee_Dashboard()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var userId = Guid.NewGuid();
        var employeeId = Guid.NewGuid();
        var today = DateOnly.FromDateTime(DateTime.Today);

        var employee = new Employee
        {
            Id = employeeId,
            UserId = userId
        };

        context.Employees.Add(employee);

        context.LeaveBalances.Add(new LeaveBalance
        {
            Id = Guid.NewGuid(),
            EmployeeId = employeeId,
            TotalDays = 20,
            UsedDays = 5
        });

        context.LeaveRequests.AddRange(
            new LeaveRequest
            {
                Id = Guid.NewGuid(),
                EmployeeId = employeeId,
                Status = LeaveRequestStatus.Pending,
                StartDate = today,
                EndDate = today
            },
            new LeaveRequest
            {
                Id = Guid.NewGuid(),
                EmployeeId = employeeId,
                Status = LeaveRequestStatus.Approved,
                StartDate = today.AddDays(5),
                EndDate = today.AddDays(6)
            },
            new LeaveRequest
            {
                Id = Guid.NewGuid(),
                EmployeeId = employeeId,
                Status = LeaveRequestStatus.Rejected,
                StartDate = today,
                EndDate = today
            });

        await context.SaveChangesAsync();

        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.UserId).Returns(userId);

        var handler = new GetEmployeeDashboardQueryHandler(
            context,
            currentUserService.Object);

        // Act
        var result = await handler.Handle(
            new GetEmployeeDashboardQuery(),
            CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.PendingLeaveRequests);
        Assert.Equal(1, result.ApprovedLeaveRequests);
        Assert.Equal(1, result.RejectedLeaveRequests);
        Assert.Equal(1, result.UpcomingLeaves);
        Assert.Equal(15, result.RemainingLeaveBalance);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Employee_Profile_Not_Found()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService
                        .Setup(x => x.UserId)
                        .Returns(Guid.NewGuid());

        var handler = new GetEmployeeDashboardQueryHandler(
            context,
            currentUserService.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            handler.Handle(
                new GetEmployeeDashboardQuery(),
                CancellationToken.None));

        Assert.Equal("Employee profile not found.", exception.Message);
    }
}