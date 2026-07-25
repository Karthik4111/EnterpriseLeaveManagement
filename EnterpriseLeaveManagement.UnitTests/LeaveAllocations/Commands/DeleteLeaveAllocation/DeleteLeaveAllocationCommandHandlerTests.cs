using EnterpriseLeaveManagement.Application.Features.LeaveAllocations.Commands.DeleteLeaveAllocation;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.UnitTests.Common;
using Xunit;

namespace EnterpriseLeaveManagement.UnitTests.LeaveAllocations.Commands.DeleteLeaveAllocation;

public class DeleteLeaveAllocationCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Delete_LeaveAllocation_Successfully()
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

        var handler = new DeleteLeaveAllocationCommandHandler(context);

        // Act
        await handler.Handle(
            new DeleteLeaveAllocationCommand
            {
                Id = allocation.Id
            },
            CancellationToken.None);

        // Assert
        Assert.Empty(context.LeaveAllocations);
    }

    [Fact]
    public async Task Handle_Should_Throw_Exception_When_LeaveAllocation_NotFound()
    {
        using var context = TestDbContextFactory.Create();

        var handler = new DeleteLeaveAllocationCommandHandler(context);

        var ex = await Assert.ThrowsAsync<Exception>(() =>
            handler.Handle(
                new DeleteLeaveAllocationCommand
                {
                    Id = Guid.NewGuid()
                },
                CancellationToken.None));

        Assert.Equal("Leave allocation not found.", ex.Message);
    }
}