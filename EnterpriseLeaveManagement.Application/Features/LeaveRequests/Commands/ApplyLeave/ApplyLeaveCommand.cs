using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace EnterpriseLeaveManagement.Application.Features.LeaveRequests.Commands.ApplyLeave;

public class ApplyLeaveCommand : IRequest<Guid>
{
    public Guid EmployeeId { get; set; }

    public Guid LeaveTypeId { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public string LeaveReason { get; set; } = string.Empty;
}