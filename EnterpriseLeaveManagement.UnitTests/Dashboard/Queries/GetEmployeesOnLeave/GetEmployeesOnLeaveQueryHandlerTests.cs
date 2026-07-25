using EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetEmployeesOnLeave;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.Domain.Enums;
using EnterpriseLeaveManagement.UnitTests.Common;
using Xunit;

namespace EnterpriseLeaveManagement.UnitTests.Dashboard.Queries.GetEmployeesOnLeave;

public class GetEmployeesOnLeaveQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Employees_On_Leave_Today()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var today = DateOnly.FromDateTime(DateTime.Today);

        var leaveType = new LeaveType
        {
            Id = Guid.NewGuid(),
            Name = "Annual Leave",
            Code = "AL"
        };

        var employee = new Employee
        {
            Id = Guid.NewGuid(),
            FirstName = "Gopi",
            LastName = "Krishna"
        };

        context.LeaveTypes.Add(leaveType);
        context.Employees.Add(employee);

        context.LeaveRequests.Add(new LeaveRequest
        {
            Id = Guid.NewGuid(),
            EmployeeId = employee.Id,
            Employee = employee,
            LeaveTypeId = leaveType.Id,
            LeaveType = leaveType,
            StartDate = today,
            EndDate = today,
            Status = LeaveRequestStatus.Approved
        });

        await context.SaveChangesAsync();

        var handler = new GetEmployeesOnLeaveQueryHandler(context);

        // Act
        var result = await handler.Handle(
            new GetEmployeesOnLeaveQuery(),
            CancellationToken.None);

        // Assert
        Assert.Single(result);

        var employeeOnLeave = result.Single();

        Assert.Equal(employee.Id, employeeOnLeave.EmployeeId);
        Assert.Equal("Gopi Krishna", employeeOnLeave.EmployeeName);
        Assert.Equal("Annual Leave", employeeOnLeave.LeaveType);
        Assert.Equal(today, employeeOnLeave.StartDate);
        Assert.Equal(today, employeeOnLeave.EndDate);
    }
}