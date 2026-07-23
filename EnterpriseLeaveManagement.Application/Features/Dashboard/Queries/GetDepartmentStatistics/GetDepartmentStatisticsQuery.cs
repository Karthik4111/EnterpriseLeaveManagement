using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnterpriseLeaveManagement.Application.Features.Dashboard.DTOs;
using MediatR;

namespace EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetDepartmentStatistics;

public record GetDepartmentStatisticsQuery: IRequest<List<DepartmentStatisticsDto>>;
