using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.Dashboard.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetLeaveTypeStatistics;

public class GetLeaveTypeStatisticsQueryHandler: IRequestHandler<GetLeaveTypeStatisticsQuery, List<LeaveTypeStatisticsDto>>
{
    private readonly IApplicationDbContext _context;

    public GetLeaveTypeStatisticsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<LeaveTypeStatisticsDto>> Handle(GetLeaveTypeStatisticsQuery request,CancellationToken cancellationToken)
    {
        return await _context.LeaveTypes
            .Where(x => !x.IsDeleted)
            .Select(x => new LeaveTypeStatisticsDto
            {
                LeaveType = x.Name,
                Count = _context.LeaveRequests.Count(l =>
                    !l.IsDeleted &&
                    l.LeaveTypeId == x.Id)
            })
            .ToListAsync(cancellationToken);
    }
}