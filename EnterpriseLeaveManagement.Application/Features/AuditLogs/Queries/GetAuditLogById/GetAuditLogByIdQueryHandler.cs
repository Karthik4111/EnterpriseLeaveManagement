using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnterpriseLeaveManagement.Application.Common.Exceptions;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.AuditLogs.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.AuditLogs.Queries.GetAuditLogById;

public class GetAuditLogByIdQueryHandler
    : IRequestHandler<GetAuditLogByIdQuery, AuditLogDto>
{
    private readonly IApplicationDbContext _context;

    public GetAuditLogByIdQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AuditLogDto> Handle(
        GetAuditLogByIdQuery request,
        CancellationToken cancellationToken)
    {
        var auditLog = await _context.AuditLogs
            .FirstOrDefaultAsync(
                x => x.Id == request.Id,
                cancellationToken);

        if (auditLog is null)
            throw new NotFoundException("Audit log not found.");

        return new AuditLogDto
        {
            Id = auditLog.Id,
            UserId = auditLog.UserId,
            UserName = auditLog.UserName,
            Action = auditLog.Action,
            EntityName = auditLog.EntityName,
            EntityId = auditLog.EntityId,
            OldValues = auditLog.OldValues,
            NewValues = auditLog.NewValues,
            CreatedOn = auditLog.CreatedOn
        };
    }
}