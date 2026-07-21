using AutoMapper;
using AutoMapper.QueryableExtensions;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Common.Models;
using EnterpriseLeaveManagement.Application.Features.LeaveRequests.DTOs;
using EnterpriseLeaveManagement.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.LeaveRequests.Queries.GetAllLeaveRequests;

public class GetAllLeaveRequestsQueryHandler
    : IRequestHandler<GetAllLeaveRequestsQuery, PagedResult<LeaveRequestDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllLeaveRequestsQueryHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResult<LeaveRequestDto>> Handle(
        GetAllLeaveRequestsQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.LeaveRequests
            .AsNoTracking()
            .Where(x => !x.IsDeleted);

        if (request.EmployeeId.HasValue)
        {
            query = query.Where(x => x.EmployeeId == request.EmployeeId);
        }

        if (request.Status.HasValue)
        {
            query = query.Where(x => x.Status == request.Status);
        }

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(x =>
                x.LeaveReason.Contains(request.Search));
        }

        query = (request.SortBy.ToLower(), request.SortOrder.ToLower()) switch
        {
            ("startdate", "asc") => query.OrderBy(x => x.StartDate),
            ("startdate", "desc") => query.OrderByDescending(x => x.StartDate),

            ("enddate", "asc") => query.OrderBy(x => x.EndDate),
            ("enddate", "desc") => query.OrderByDescending(x => x.EndDate),

            ("status", "asc") => query.OrderBy(x => x.Status),
            ("status", "desc") => query.OrderByDescending(x => x.Status),

            _ => query.OrderByDescending(x => x.StartDate)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<LeaveRequestDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PagedResult<LeaveRequestDto>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}