using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnterpriseLeaveManagement.Application.Features.Dashboard.Responses;
using MediatR;

namespace EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetEmployeeDashboard;

public class GetEmployeeDashboardQuery : IRequest<EmployeeDashboardResponse>
{
}