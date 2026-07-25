using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnterpriseLeaveManagement.Application.Features.LeaveTypes.Commands.CreateLeaveType;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.UnitTests.Common;
using Xunit;

namespace EnterpriseLeaveManagement.UnitTests.LeaveTypes.Commands.CreateLeaveType;

public class CreateLeaveTypeCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Create_LeaveType_Successfully()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var command = new CreateLeaveTypeCommand
        {
            Name = "Annual Leave",
            Code = "AL",
            Description = "Annual Paid Leave",
            DefaultDays = 24,
            IsPaidLeave = true,
            CarryForwardAllowed = true,
            MaximumCarryForwardDays = 10,
            RequiresApproval = true,
            IsActive = true
        };

        var handler = new CreateLeaveTypeCommandHandler(context);

        // Act
        var leaveTypeId = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.NotEqual(Guid.Empty, leaveTypeId);

        var leaveType = await context.LeaveTypes.FindAsync(leaveTypeId);

        Assert.NotNull(leaveType);
        Assert.Equal(command.Name, leaveType!.Name);
        Assert.Equal(command.Code, leaveType.Code);
        Assert.Equal(command.Description, leaveType.Description);
        Assert.Equal(command.DefaultDays, leaveType.DefaultDays);
        Assert.Equal(command.IsPaidLeave, leaveType.IsPaidLeave);
        Assert.Equal(command.CarryForwardAllowed, leaveType.CarryForwardAllowed);
        Assert.Equal(command.MaximumCarryForwardDays, leaveType.MaximumCarryForwardDays);
        Assert.Equal(command.RequiresApproval, leaveType.RequiresApproval);
        Assert.True(leaveType.IsActive);
    }
}
