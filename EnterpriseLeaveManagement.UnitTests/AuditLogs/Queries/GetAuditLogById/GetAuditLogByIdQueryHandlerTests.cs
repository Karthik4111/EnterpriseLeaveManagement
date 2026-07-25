using EnterpriseLeaveManagement.Application.Common.Exceptions;
using EnterpriseLeaveManagement.Application.Features.AuditLogs.Queries.GetAuditLogById;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.UnitTests.Common;
using Xunit;

namespace EnterpriseLeaveManagement.UnitTests.AuditLogs.Queries.GetAuditLogById;

public class GetAuditLogByIdQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_AuditLog_By_Id()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var auditLog = new AuditLog
        {
            Id = Guid.NewGuid(),
            UserId = Guid.NewGuid().ToString(),
            UserName = "Gopi",
            Action = "Create",
            EntityName = "Department",
            EntityId = Guid.NewGuid(),
            OldValues = "{}",
            NewValues = "{ \"Name\": \"IT\" }",
            CreatedOn = DateTime.UtcNow
        };

        context.AuditLogs.Add(auditLog);
        await context.SaveChangesAsync();

        var handler = new GetAuditLogByIdQueryHandler(context);

        // Act
        var result = await handler.Handle(
            new GetAuditLogByIdQuery(auditLog.Id),
            CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(auditLog.Id, result.Id);
        Assert.Equal("Gopi", result.UserName);
        Assert.Equal("Create", result.Action);
        Assert.Equal("Department", result.EntityName);
        Assert.Equal(auditLog.EntityId, result.EntityId);
        Assert.Equal(auditLog.OldValues, result.OldValues);
        Assert.Equal(auditLog.NewValues, result.NewValues);
    }

    [Fact]
    public async Task Handle_Should_Throw_NotFoundException_When_AuditLog_Not_Found()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var handler = new GetAuditLogByIdQueryHandler(context);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<NotFoundException>(() =>
            handler.Handle(
                new GetAuditLogByIdQuery(Guid.NewGuid()),
                CancellationToken.None));

        Assert.Equal("Audit log not found.", exception.Message);
    }
}