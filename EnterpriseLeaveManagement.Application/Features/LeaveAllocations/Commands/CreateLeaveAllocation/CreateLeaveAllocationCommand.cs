using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace EnterpriseLeaveManagement.Application.Features.LeaveAllocations.Commands.CreateLeaveAllocation;

public class CreateLeaveAllocationCommand : IRequest<Guid>
{
    public Guid EmployeeId { get; set; }

    public Guid LeaveTypeId { get; set; }

    public int Year { get; set; }

    public int AllocatedDays { get; set; }
}