using EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetLeaveStatusStatistics;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.Domain.Enums;
using EnterpriseLeaveManagement.UnitTests.Common;
using Xunit;

namespace EnterpriseLeaveManagement.UnitTests.Dashboard.Queries.GetLeaveStatusStatistics;

public class GetLeaveStatusStatisticsQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Leave_Status_Statistics()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        context.LeaveRequests.AddRange(
            new LeaveRequest
            {
                Id = Guid.NewGuid(),
                Status = LeaveRequestStatus.Pending
            },
            new LeaveRequest
            {
                Id = Guid.NewGuid(),
                Status = LeaveRequestStatus.Pending
            },
            new LeaveRequest
            {
                Id = Guid.NewGuid(),
                Status = LeaveRequestStatus.Approved
            },
            new LeaveRequest
            {
                Id = Guid.NewGuid(),
                Status = LeaveRequestStatus.Rejected
            });

        await context.SaveChangesAsync();

        var handler = new GetLeaveStatusStatisticsQueryHandler(context);

        // Act
        var result = await handler.Handle(
            new GetLeaveStatusStatisticsQuery(),
            CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(3, result.Count);

        var pending = result.Single(x => x.Status == LeaveRequestStatus.Pending.ToString());
        var approved = result.Single(x => x.Status == LeaveRequestStatus.Approved.ToString());
        var rejected = result.Single(x => x.Status == LeaveRequestStatus.Rejected.ToString());

        Assert.Equal(2, pending.Count);
        Assert.Equal(1, approved.Count);
        Assert.Equal(1, rejected.Count);
    }
}