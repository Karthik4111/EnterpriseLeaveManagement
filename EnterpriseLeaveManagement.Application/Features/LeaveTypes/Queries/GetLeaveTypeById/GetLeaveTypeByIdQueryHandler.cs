using AutoMapper;
using EnterpriseLeaveManagement.Application.Common.Exceptions;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.LeaveTypes.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.LeaveTypes.Queries.GetLeaveTypeById;

public class GetLeaveTypeByIdQueryHandler: IRequestHandler<GetLeaveTypeByIdQuery, LeaveTypeDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetLeaveTypeByIdQueryHandler(IApplicationDbContext context,IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<LeaveTypeDto> Handle(GetLeaveTypeByIdQuery request,CancellationToken cancellationToken)
    {
        var leaveType = await _context.LeaveTypes
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && !x.IsDeleted,
                cancellationToken);

        if (leaveType == null)
        {
            throw new NotFoundException("Leave type not found.");
        }

        return _mapper.Map<LeaveTypeDto>(leaveType);
    }
}