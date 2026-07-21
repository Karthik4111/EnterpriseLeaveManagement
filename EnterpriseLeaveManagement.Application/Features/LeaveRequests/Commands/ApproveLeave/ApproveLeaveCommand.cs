using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;


namespace EnterpriseLeaveManagement.Application.Features.LeaveRequests.Commands.ApproveLeave;

public class ApproveLeaveCommand : IRequest
{
    public Guid LeaveRequestId { get; set; }

    public string? ManagerComments { get; set; }
}