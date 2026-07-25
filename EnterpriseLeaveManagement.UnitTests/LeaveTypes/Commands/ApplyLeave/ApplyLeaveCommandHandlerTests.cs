using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.LeaveRequests.Commands.ApplyLeave;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.UnitTests.Common;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace EnterpriseLeaveManagement.UnitTests.LeaveRequests.Commands.ApplyLeave;

public class ApplyLeaveCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Apply_Leave_Successfully()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var auditServiceMock = new Mock<IAuditService>();
        var emailServiceMock = new Mock<IEmailService>();
        var loggerMock = new Mock<ILogger<ApplyLeaveCommandHandler>>();

        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            FirstName = "Gopi",
            LastName = "Krishna",
            Email = "gopi@company.com",
            EmployeeCode = "EMP001",
            DepartmentId = Guid.NewGuid(),
            IsActive = true
        };

        var leaveType = new LeaveType
        {
            Id = Guid.NewGuid(),
            Name = "Annual Leave",
            Code = "AL",
            IsActive = true
        };

        var leaveBalance = new LeaveBalance
        {
            Id = Guid.NewGuid(),
            EmployeeId = employee.Id,
            LeaveTypeId = leaveType.Id,
            TotalDays = 20,
            UsedDays = 0
        };

        context.Employees.Add(employee);
        context.LeaveTypes.Add(leaveType);
        context.LeaveBalances.Add(leaveBalance);

        await context.SaveChangesAsync();

        var command = new ApplyLeaveCommand
        {
            EmployeeId = employee.Id,
            LeaveTypeId = leaveType.Id,
            StartDate = new DateOnly(2026, 8, 1),
            EndDate = new DateOnly(2026, 8, 3),
            LeaveReason = "Vacation"
        };

        var handler = new ApplyLeaveCommandHandler(
            context,
            auditServiceMock.Object,
            emailServiceMock.Object,
            loggerMock.Object);

        // Act
        var leaveRequestId = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, leaveRequestId);

        Assert.Single(context.LeaveRequests);
        Assert.Single(context.Notifications);

        var leaveRequest = context.LeaveRequests.Single();

        Assert.Equal(employee.Id, leaveRequest.EmployeeId);
        Assert.Equal(leaveType.Id, leaveRequest.LeaveTypeId);
        Assert.Equal(3, leaveRequest.NumberOfDays);
        Assert.Equal("Vacation", leaveRequest.LeaveReason);
    }


    [Fact]
    public async Task Handle_Should_Throw_Exception_When_LeaveBalance_NotFound()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var auditServiceMock = new Mock<IAuditService>();
        var emailServiceMock = new Mock<IEmailService>();
        var loggerMock = new Mock<ILogger<ApplyLeaveCommandHandler>>();

        var handler = new ApplyLeaveCommandHandler(
            context,
            auditServiceMock.Object,
            emailServiceMock.Object,
            loggerMock.Object);

        var command = new ApplyLeaveCommand
        {
            EmployeeId = Guid.NewGuid(),
            LeaveTypeId = Guid.NewGuid(),
            StartDate = new DateOnly(2026, 8, 1),
            EndDate = new DateOnly(2026, 8, 3)
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            handler.Handle(command, CancellationToken.None));

        Assert.Equal("Leave balance not found.", exception.Message);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_Employee_NotFound()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var auditServiceMock = new Mock<IAuditService>();
        var emailServiceMock = new Mock<IEmailService>();
        var loggerMock = new Mock<ILogger<ApplyLeaveCommandHandler>>();

        var leaveTypeId = Guid.NewGuid();
        var employeeId = Guid.NewGuid();

        context.LeaveBalances.Add(new LeaveBalance
        {
            EmployeeId = employeeId,
            LeaveTypeId = leaveTypeId,
            TotalDays = 20,
            UsedDays = 0
        });

        await context.SaveChangesAsync();

        var handler = new ApplyLeaveCommandHandler(
            context,
            auditServiceMock.Object,
            emailServiceMock.Object,
            loggerMock.Object);

        var command = new ApplyLeaveCommand
        {
            EmployeeId = employeeId,
            LeaveTypeId = leaveTypeId,
            StartDate = new DateOnly(2026, 8, 1),
            EndDate = new DateOnly(2026, 8, 2)
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            handler.Handle(command, CancellationToken.None));

        Assert.Equal("Employee not found.", exception.Message);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_LeaveBalance_Is_Insufficient()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var auditServiceMock = new Mock<IAuditService>();
        var emailServiceMock = new Mock<IEmailService>();
        var loggerMock = new Mock<ILogger<ApplyLeaveCommandHandler>>();

        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid(),
            FirstName = "Gopi",
            Email = "gopi@company.com"
        };

        var leaveType = new LeaveType
        {
            Id = Guid.NewGuid(),
            Name = "Annual Leave",
            Code = "AL"
        };

        context.Employees.Add(employee);

        context.LeaveBalances.Add(new LeaveBalance
        {
            EmployeeId = employee.Id,
            LeaveTypeId = leaveType.Id,
            TotalDays = 2,
            UsedDays = 0
        });

        await context.SaveChangesAsync();

        var handler = new ApplyLeaveCommandHandler(
            context,
            auditServiceMock.Object,
            emailServiceMock.Object,
            loggerMock.Object);

        var command = new ApplyLeaveCommand
        {
            EmployeeId = employee.Id,
            LeaveTypeId = leaveType.Id,
            StartDate = new DateOnly(2026, 8, 1),
            EndDate = new DateOnly(2026, 8, 5), // 5 days requested
            LeaveReason = "Vacation"
        };

        // Act & Assert
        var exception = await Assert.ThrowsAsync<Exception>(() =>
            handler.Handle(command, CancellationToken.None));

        Assert.StartsWith("Insufficient leave balance.", exception.Message);
    }
}