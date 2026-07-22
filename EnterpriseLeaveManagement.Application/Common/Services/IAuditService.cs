using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnterpriseLeaveManagement.Domain.Entities;

namespace EnterpriseLeaveManagement.Application.Common.Interfaces;

public interface IAuditService
{
    Task LogAsync(
        string action,
        string entityName,
        Guid entityId,
        string? oldValues = null,
        string? newValues = null);
}