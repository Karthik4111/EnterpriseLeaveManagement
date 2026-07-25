using EnterpriseLeaveManagement.Application.Features.LeaveAllocations.Commands.UpdateLeaveAllocation;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.UnitTests.Common;
using Xunit;

namespace EnterpriseLeaveManagement.UnitTests.LeaveAllocations.Commands.UpdateLeaveAllocation;

public class UpdateLeaveAllocationCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Update_LeaveAllocation_Successfully()
    {
        using var context = TestDbContextFactory.Create();

        var allocation = new LeaveAllocation
        {
            Id = Guid.NewGuid(),
            EmployeeId = Guid.NewGuid(),
            LeaveTypeId = Guid.NewGuid(),
            Year = 2026,
            AllocatedDays = 20
        };

        context.LeaveAllocations.Add(allocation);
        await context.SaveChangesAsync();

        var handler = new UpdateLeaveAllocationCommandHandler(context);

        await handler.Handle(
            new UpdateLeaveAllocationCommand
            {
                Id = allocation.Id,
                AllocatedDays = 30,
                Year = 2027
            },
            CancellationToken.None);

        var updated = await context.LeaveAllocations.FindAsync(allocation.Id);

        Assert.NotNull(updated);
        Assert.Equal(30, updated!.AllocatedDays);
        Assert.Equal(2027, updated.Year);
    }



    [Fact]
    public async Task Handle_Should_Throw_Exception_When_LeaveAllocation_NotFound()
    {
        using var context = TestDbContextFactory.Create();

        var handler = new UpdateLeaveAllocationCommandHandler(context);

        var ex = await Assert.ThrowsAsync<Exception>(() =>
            handler.Handle(
                new UpdateLeaveAllocationCommand
                {
                    Id = Guid.NewGuid(),
                    AllocatedDays = 30,
                    Year = 2027
                },
                CancellationToken.None));

        Assert.Equal("Leave allocation not found.", ex.Message);
    }
}

