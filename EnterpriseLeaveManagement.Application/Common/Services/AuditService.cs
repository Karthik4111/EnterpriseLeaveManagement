using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Domain.Entities;

namespace EnterpriseLeaveManagement.Application.Common.Services;

public class AuditService : IAuditService
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public AuditService(IApplicationDbContext context,ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task LogAsync(string action,string entityName,Guid entityId,string? oldValues = null,string? newValues = null)
    {
        var audit = new AuditLog
        {
            UserId = _currentUserService.UserId?.ToString() ?? string.Empty,
            UserName = _currentUserService.UserName ?? string.Empty,
            Action = action,
            EntityName = entityName,
            EntityId = entityId,
            OldValues = oldValues,
            NewValues = newValues,
            CreatedOn = DateTime.UtcNow
        };

        _context.AuditLogs.Add(audit);

        await _context.SaveChangesAsync(CancellationToken.None);
    }
}
