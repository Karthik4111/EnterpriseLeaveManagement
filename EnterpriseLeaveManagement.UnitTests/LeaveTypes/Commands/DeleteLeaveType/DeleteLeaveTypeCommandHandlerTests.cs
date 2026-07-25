using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseLeaveManagement.Application.Common.Exceptions;
using EnterpriseLeaveManagement.Application.Features.LeaveTypes.Commands.DeleteLeaveType;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.UnitTests.Common;
using Xunit;

namespace EnterpriseLeaveManagement.UnitTests.LeaveTypes.Commands.DeleteLeaveType;

public class DeleteLeaveTypeCommandHandlerTests
{
    [Fact]
    public async Task Handle_Should_Soft_Delete_LeaveType_Successfully()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var leaveType = new LeaveType
        {
            Id = Guid.NewGuid(),
            Name = "Annual Leave",
            Code = "AL",
            IsActive = true,
            IsDeleted = false
        };

        context.LeaveTypes.Add(leaveType);
        await context.SaveChangesAsync();

        var command = new DeleteLeaveTypeCommand
        {
            Id = leaveType.Id
        };

        var handler = new DeleteLeaveTypeCommandHandler(context);

        // Act
        await handler.Handle(command, CancellationToken.None);

        // Assert
        var deleted = await context.LeaveTypes.FindAsync(leaveType.Id);

        Assert.NotNull(deleted);
        Assert.True(deleted!.IsDeleted);
        Assert.False(deleted.IsActive);
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFoundException_When_LeaveType_Does_Not_Exist()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var command = new DeleteLeaveTypeCommand
        {
            Id = Guid.NewGuid()
        };

        var handler = new DeleteLeaveTypeCommandHandler(context);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(command, CancellationToken.None));

        Assert.Equal("Leave type not found.", exception.Message);
    }
}