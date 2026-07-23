using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.Dashboard.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetDepartmentStatistics;

public class GetDepartmentStatisticsQueryHandler: IRequestHandler<GetDepartmentStatisticsQuery, List<DepartmentStatisticsDto>>
{
    private readonly IApplicationDbContext _context;

    public GetDepartmentStatisticsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<DepartmentStatisticsDto>> Handle(GetDepartmentStatisticsQuery request,CancellationToken cancellationToken)
    {
        return await _context.Departments
            .Where(d => !d.IsDeleted)
            .Select(d => new DepartmentStatisticsDto
            {
                DepartmentName = d.Name,
                EmployeeCount = d.Employees.Count(e => !e.IsDeleted)
            })
            .ToListAsync(cancellationToken);
    }
}