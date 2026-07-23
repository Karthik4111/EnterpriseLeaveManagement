using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnterpriseLeaveManagement.Application.Features.Dashboard.DTOs;
using MediatR;

namespace EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetEmployeesOnLeave;

public record GetEmployeesOnLeaveQuery: IRequest<List<EmployeeOnLeaveDto>>;