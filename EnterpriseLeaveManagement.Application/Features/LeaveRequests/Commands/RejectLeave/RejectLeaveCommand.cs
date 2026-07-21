using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace EnterpriseLeaveManagement.Application.Features.LeaveRequests.Commands.RejectLeave;

public class RejectLeaveCommand : IRequest
{
    public Guid LeaveRequestId { get; set; }

    public Guid ApprovedBy { get; set; }

    public string ManagerComments { get; set; } = string.Empty;
}