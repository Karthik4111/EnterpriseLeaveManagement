using AutoMapper;
using AutoMapper.QueryableExtensions;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.LeaveAllocations.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.LeaveAllocations.Queries.GetLeaveAllocationById;

public class GetLeaveAllocationByIdQueryHandler: IRequestHandler<GetLeaveAllocationByIdQuery, LeaveAllocationDto?>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetLeaveAllocationByIdQueryHandler(IApplicationDbContext context,IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<LeaveAllocationDto?> Handle(GetLeaveAllocationByIdQuery request,CancellationToken cancellationToken)
    {
        return await _context.LeaveAllocations
            .Where(x => x.Id == request.Id)
            .Include(x => x.Employee)
            .Include(x => x.LeaveType)
            .ProjectTo<LeaveAllocationDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync(cancellationToken);
    }
}