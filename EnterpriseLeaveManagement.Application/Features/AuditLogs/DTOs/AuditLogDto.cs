using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseLeaveManagement.Application.Features.AuditLogs.DTOs;

public class AuditLogDto
{
    public Guid Id { get; set; }

    public string? UserId { get; set; }

    public string? UserName { get; set; }

    public string? Action { get; set; }

    public string? EntityName { get; set; }

    public Guid EntityId { get; set; }

    public string? OldValues { get; set; }

    public string? NewValues { get; set; }

    public DateTime CreatedOn { get; set; }
}