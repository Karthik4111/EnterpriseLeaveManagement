using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Common.Models;
using EnterpriseLeaveManagement.Application.Features.LeaveTypes.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.LeaveTypes.Queries.GetAllLeaveTypes;

public class GetAllLeaveTypesQueryHandler
    : IRequestHandler<GetAllLeaveTypesQuery, PagedResult<LeaveTypeDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllLeaveTypesQueryHandler(
        IApplicationDbContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<PagedResult<LeaveTypeDto>> Handle(
        GetAllLeaveTypesQuery request,
        CancellationToken cancellationToken)
    {
        var query = _context.LeaveTypes
            .AsNoTracking()
            .Where(x => !x.IsDeleted)
            .AsQueryable();

        if (request.IsActive.HasValue)
        {
            query = query.Where(x => x.IsActive == request.IsActive.Value);
        }

        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(x =>
                x.Name.Contains(request.Search) ||
                x.Code.Contains(request.Search));
        }

        query = (request.SortBy?.ToLower(), request.SortOrder.ToLower()) switch
        {
            ("name", "desc") => query.OrderByDescending(x => x.Name),
            ("code", "asc") => query.OrderBy(x => x.Code),
            ("code", "desc") => query.OrderByDescending(x => x.Code),
            ("defaultdays", "asc") => query.OrderBy(x => x.DefaultDays),
            ("defaultdays", "desc") => query.OrderByDescending(x => x.DefaultDays),
            (_, "desc") => query.OrderByDescending(x => x.Name),
            _ => query.OrderBy(x => x.Name)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var items = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ProjectTo<LeaveTypeDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);

        return new PagedResult<LeaveTypeDto>
        {
            Items = items,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}