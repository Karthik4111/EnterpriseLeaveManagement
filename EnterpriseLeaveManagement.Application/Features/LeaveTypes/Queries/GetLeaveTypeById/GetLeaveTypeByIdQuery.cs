using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseLeaveManagement.Application.Features.LeaveTypes.DTOs;
using MediatR;

namespace EnterpriseLeaveManagement.Application.Features.LeaveTypes.Queries.GetLeaveTypeById;

public record GetLeaveTypeByIdQuery(Guid Id) : IRequest<LeaveTypeDto>;