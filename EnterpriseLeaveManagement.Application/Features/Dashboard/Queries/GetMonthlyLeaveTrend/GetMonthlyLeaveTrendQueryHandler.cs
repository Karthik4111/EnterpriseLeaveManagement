using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.Dashboard.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetMonthlyLeaveTrend;

public class GetMonthlyLeaveTrendQueryHandler: IRequestHandler<GetMonthlyLeaveTrendQuery, List<MonthlyLeaveTrendDto>>
{
    private readonly IApplicationDbContext _context;

    public GetMonthlyLeaveTrendQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<MonthlyLeaveTrendDto>> Handle(GetMonthlyLeaveTrendQuery request,CancellationToken cancellationToken)
    {
        var currentYear = DateTime.UtcNow.Year;

        var monthlyData = await _context.LeaveRequests
            .Where(x => !x.IsDeleted && x.StartDate.Year == currentYear)
            .GroupBy(x => x.StartDate.Month)
            .Select(g => new
            {
                Month = g.Key,
                Count = g.Count()
            })
            .ToListAsync(cancellationToken);

        return Enumerable.Range(1, 12)
            .Select(month => new MonthlyLeaveTrendDto
            {
                Month = new DateTime(currentYear, month, 1).ToString("MMM"),
                Count = monthlyData.FirstOrDefault(x => x.Month == month)?.Count ?? 0
            })
            .ToList();
    }
}