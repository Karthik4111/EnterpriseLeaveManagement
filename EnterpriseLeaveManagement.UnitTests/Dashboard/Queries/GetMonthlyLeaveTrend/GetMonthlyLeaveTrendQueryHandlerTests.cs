using EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetMonthlyLeaveTrend;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.UnitTests.Common;
using Xunit;

namespace EnterpriseLeaveManagement.UnitTests.Dashboard.Queries.GetMonthlyLeaveTrend;

public class GetMonthlyLeaveTrendQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Monthly_Leave_Trend()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var currentYear = DateTime.UtcNow.Year;

        context.LeaveRequests.AddRange(
            new LeaveRequest
            {
                Id = Guid.NewGuid(),
                StartDate = new DateOnly(currentYear, 1, 10)
            },
            new LeaveRequest
            {
                Id = Guid.NewGuid(),
                StartDate = new DateOnly(currentYear, 1, 20)
            },
            new LeaveRequest
            {
                Id = Guid.NewGuid(),
                StartDate = new DateOnly(currentYear, 3, 5)
            });

        await context.SaveChangesAsync();

        var handler = new GetMonthlyLeaveTrendQueryHandler(context);

        // Act
        var result = await handler.Handle(
            new GetMonthlyLeaveTrendQuery(),
            CancellationToken.None);

        // Assert
        Assert.Equal(12, result.Count);

        Assert.Equal(2, result.Single(x => x.Month == "Jan").Count);
        Assert.Equal(0, result.Single(x => x.Month == "Feb").Count);
        Assert.Equal(1, result.Single(x => x.Month == "Mar").Count);
        Assert.Equal(0, result.Single(x => x.Month == "Apr").Count);
        Assert.Equal(0, result.Single(x => x.Month == "May").Count);
        Assert.Equal(0, result.Single(x => x.Month == "Jun").Count);
        Assert.Equal(0, result.Single(x => x.Month == "Jul").Count);
        Assert.Equal(0, result.Single(x => x.Month == "Aug").Count);
        Assert.Equal(0, result.Single(x => x.Month == "Sep").Count);
        Assert.Equal(0, result.Single(x => x.Month == "Oct").Count);
        Assert.Equal(0, result.Single(x => x.Month == "Nov").Count);
        Assert.Equal(0, result.Single(x => x.Month == "Dec").Count);
    }
}