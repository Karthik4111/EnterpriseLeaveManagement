using EnterpriseLeaveManagement.Application.Features.AuditLogs.Queries.GetAuditLogs;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.UnitTests.Common;
using Xunit;

namespace EnterpriseLeaveManagement.UnitTests.AuditLogs.Queries.GetAuditLogs;

public class GetAuditLogsQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_All_AuditLogs_In_Descending_Order()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var olderLog = new AuditLog
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid().ToString(),
            UserName = "Gopi",
            Action = "Create",
            EntityName = "Department",
            EntityId = Guid.NewGuid(),
            OldValues = "{}",
            NewValues = "{ \"Name\": \"IT\" }",
            CreatedOn = DateTime.UtcNow.AddDays(-1)
        };

        var newerLog = new AuditLog
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid().ToString(),
            UserName = "Gopi",
            Action = "Update",
            EntityName = "Employee",
            EntityId = Guid.NewGuid(),
            OldValues = "{ \"Name\": \"Old\" }",
            NewValues = "{ \"Name\": \"New\" }",
            CreatedOn = DateTime.UtcNow
        };

        context.AuditLogs.AddRange(olderLog, newerLog);
        await context.SaveChangesAsync();

        var handler = new GetAuditLogsQueryHandler(context);

        // Act
        var result = await handler.Handle(
            new GetAuditLogsQuery(),
            CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);

        // Ordered by CreatedOn descending
        Assert.Equal(newerLog.Id, result[0].Id);
        Assert.Equal(olderLog.Id, result[1].Id);

        Assert.Equal("Update", result[0].Action);
        Assert.Equal("Create", result[1].Action);
    }
}