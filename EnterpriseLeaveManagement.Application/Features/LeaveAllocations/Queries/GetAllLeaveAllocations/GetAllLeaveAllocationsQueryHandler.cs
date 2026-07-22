using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using AutoMapper;
using AutoMapper.QueryableExtensions;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.LeaveAllocations.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.LeaveAllocations.Queries.GetAllLeaveAllocations;

public class GetAllLeaveAllocationsQueryHandler: IRequestHandler<GetAllLeaveAllocationsQuery, List<LeaveAllocationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetAllLeaveAllocationsQueryHandler(IApplicationDbContext context,IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<List<LeaveAllocationDto>> Handle(
        GetAllLeaveAllocationsQuery request,
        CancellationToken cancellationToken)
    {
        return await _context.LeaveAllocations
            .Include(x => x.Employee)
            .Include(x => x.LeaveType)
            .ProjectTo<LeaveAllocationDto>(_mapper.ConfigurationProvider)
            .ToListAsync(cancellationToken);
    }
}