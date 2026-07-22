using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;

namespace EnterpriseLeaveManagement.Application.Features.LeaveAllocations.Commands.UpdateLeaveAllocation;

public class UpdateLeaveAllocationCommand : IRequest
{
    public Guid Id { get; set; }

    public int AllocatedDays { get; set; }

    public int Year { get; set; }
}
