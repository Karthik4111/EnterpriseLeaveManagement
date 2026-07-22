using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnterpriseLeaveManagement.Application.Features.AuditLogs.DTOs;
using MediatR;

namespace EnterpriseLeaveManagement.Application.Features.AuditLogs.Queries.GetAuditLogById;

public record GetAuditLogByIdQuery(Guid Id) : IRequest<AuditLogDto>;