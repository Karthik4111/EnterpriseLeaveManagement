using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseLeaveManagement.Application.Features.LeaveAllocations.DTOs;
using MediatR;

namespace EnterpriseLeaveManagement.Application.Features.LeaveAllocations.Queries.GetAllLeaveAllocations;

public class GetAllLeaveAllocationsQuery : IRequest<List<LeaveAllocationDto>>
{
}