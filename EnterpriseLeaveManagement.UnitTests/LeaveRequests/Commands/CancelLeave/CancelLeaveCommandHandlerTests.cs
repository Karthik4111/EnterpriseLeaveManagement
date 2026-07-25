using EnterpriseLeaveManagement.Application.Common.Exceptions;
using EnterpriseLeaveManagement.Application.Features.LeaveRequests.Commands.CancelLeave;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.Domain.Enums;
using EnterpriseLeaveManagement.UnitTests.Common;
using Xunit;

namespace EnterpriseLeaveManagement.UnitTests.LeaveRequests.Commands.CancelLeave;

public class CancelLeaveCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Cancel_Leave_Successfully()
    {
        using var context = TestDbContextFactory.Create();

        var leaveRequest = new LeaveRequest
        {
            Id = Guid.NewGuid(),
            Status = LeaveRequestStatus.Pending
        };

        context.LeaveRequests.Add(leaveRequest);
        await context.SaveChangesAsync();

        var handler = new CancelLeaveCommandHandler(context);

        await handler.Handle(
            new CancelLeaveCommand(leaveRequest.Id),
            CancellationToken.None);

        var updated = await context.LeaveRequests.FindAsync(leaveRequest.Id);

        Assert.NotNull(updated);
        Assert.Equal(LeaveRequestStatus.Cancelled, updated!.Status);
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFoundException_When_LeaveRequest_NotFound()
    {
        using var context = TestDbContextFactory.Create();

        var handler = new CancelLeaveCommandHandler(context);

        var ex = await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(
                new CancelLeaveCommand(Guid.NewGuid()),
                CancellationToken.None));

        Assert.Equal("Leave request not found.", ex.Message);
    }


    [Fact]
    public async Task Handle_Should_Throw_BadRequestException_When_LeaveRequest_Is_Not_Pending()
    {
        using var context = TestDbContextFactory.Create();

        var leaveRequest = new LeaveRequest
        {
            Id = Guid.NewGuid(),
            Status = LeaveRequestStatus.Approved
        };

        context.LeaveRequests.Add(leaveRequest);
        await context.SaveChangesAsync();

        var handler = new CancelLeaveCommandHandler(context);

        var ex = await Assert.ThrowsAsync<BadRequestException>(() =>
            handler.Handle(
                new CancelLeaveCommand(leaveRequest.Id),
                CancellationToken.None));

        Assert.Equal(
            "Only pending leave requests can be cancelled.",
            ex.Message);
    }
}