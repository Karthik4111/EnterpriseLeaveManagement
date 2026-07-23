using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.Dashboard.DTOs;
using EnterpriseLeaveManagement.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetLeaveStatusStatistics;

public class GetLeaveStatusStatisticsQueryHandler: IRequestHandler<GetLeaveStatusStatisticsQuery, List<LeaveStatusDto>>
{
    private readonly IApplicationDbContext _context;

    public GetLeaveStatusStatisticsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<LeaveStatusDto>> Handle(GetLeaveStatusStatisticsQuery request,CancellationToken cancellationToken)
    {
        return new List<LeaveStatusDto>
        {
            new()
            {
                Status = LeaveRequestStatus.Pending.ToString(),
                Count = await _context.LeaveRequests
                    .CountAsync(x =>
                        !x.IsDeleted &&
                        x.Status == LeaveRequestStatus.Pending,
                        cancellationToken)
            },

            new()
            {
                Status = LeaveRequestStatus.Approved.ToString(),
                Count = await _context.LeaveRequests
                    .CountAsync(x =>
                        !x.IsDeleted &&
                        x.Status == LeaveRequestStatus.Approved,
                        cancellationToken)
            },

            new()
            {
                Status = LeaveRequestStatus.Rejected.ToString(),
                Count = await _context.LeaveRequests
                    .CountAsync(x =>
                        !x.IsDeleted &&
                        x.Status == LeaveRequestStatus.Rejected,
                        cancellationToken)
            }
        };
    }
}