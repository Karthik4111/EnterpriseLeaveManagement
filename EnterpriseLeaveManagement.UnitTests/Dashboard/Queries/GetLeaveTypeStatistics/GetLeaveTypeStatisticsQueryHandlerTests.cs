using EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetLeaveTypeStatistics;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.UnitTests.Common;
using Xunit;

namespace EnterpriseLeaveManagement.UnitTests.Dashboard.Queries.GetLeaveTypeStatistics;

public class GetLeaveTypeStatisticsQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_LeaveType_Statistics()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var annual = new LeaveType
        {
            Id = Guid.NewGuid(),
            Name = "Annual Leave",
            Code = "AL"
        };

        var sick = new LeaveType
        {
            Id = Guid.NewGuid(),
            Name = "Sick Leave",
            Code = "SL"
        };

        context.LeaveTypes.AddRange(annual, sick);

        context.LeaveRequests.AddRange(
            new LeaveRequest
            {
                Id = Guid.NewGuid(),
                LeaveTypeId = annual.Id
            },
            new LeaveRequest
            {
                Id = Guid.NewGuid(),
                LeaveTypeId = annual.Id
            },
            new LeaveRequest
            {
                Id = Guid.NewGuid(),
                LeaveTypeId = sick.Id
            });

        await context.SaveChangesAsync();

        var handler = new GetLeaveTypeStatisticsQueryHandler(context);

        // Act
        var result = await handler.Handle(
            new GetLeaveTypeStatisticsQuery(),
            CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(2, result.Count);

        var annualResult = result.Single(x => x.LeaveType == "Annual Leave");
        var sickResult = result.Single(x => x.LeaveType == "Sick Leave");

        Assert.Equal(2, annualResult.Count);
        Assert.Equal(1, sickResult.Count);
    }
}