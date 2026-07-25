using EnterpriseLeaveManagement.Application.Common.Exceptions;
using EnterpriseLeaveManagement.Application.Features.LeaveTypes.Commands.UpdateLeaveType;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.UnitTests.Common;
using Xunit;

namespace EnterpriseLeaveManagement.UnitTests.LeaveTypes.Commands.UpdateLeaveType;

public class UpdateLeaveTypeCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Update_LeaveType_Successfully()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var leaveType = new LeaveType
        {
            Id = Guid.NewGuid(),
            Name = "Annual Leave",
            Code = "AL",
            Description = "Annual Leave",
            DefaultDays = 20,
            IsPaidLeave = true,
            CarryForwardAllowed = true,
            MaximumCarryForwardDays = 5,
            RequiresApproval = true,
            IsActive = true
        };

        context.LeaveTypes.Add(leaveType);
        await context.SaveChangesAsync();

        var command = new UpdateLeaveTypeCommand
        {
            Id = leaveType.Id,
            Name = "Casual Leave",
            Code = "CL",
            Description = "Casual Leave",
            DefaultDays = 12,
            IsPaidLeave = true,
            CarryForwardAllowed = false,
            MaximumCarryForwardDays = 0,
            RequiresApproval = false,
            IsActive = true
        };

        var handler = new UpdateLeaveTypeCommandHandler(context);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        var updated = await context.LeaveTypes.FindAsync(leaveType.Id);

        Assert.NotNull(updated);
        Assert.Equal("Casual Leave", updated!.Name);
        Assert.Equal("CL", updated.Code);
        Assert.Equal("Casual Leave", updated.Description);
        Assert.Equal(12, updated.DefaultDays);
        Assert.False(updated.CarryForwardAllowed);
        Assert.False(updated.RequiresApproval);
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFoundException_When_LeaveType_Does_Not_Exist()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var command = new UpdateLeaveTypeCommand
        {
            Id = Guid.NewGuid(),
            Name = "Annual Leave",
            Code = "AL"
        };

        var handler = new UpdateLeaveTypeCommandHandler(context);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(command, CancellationToken.None));

        Assert.Equal("Leave type not found.", exception.Message);
    }
}