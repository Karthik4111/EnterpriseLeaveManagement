using AutoMapper;
using EnterpriseLeaveManagement.Application.Common.Exceptions;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.LeaveRequests.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.LeaveRequests.Queries.GetLeaveRequestById;

public class GetLeaveRequestByIdQueryHandler: IRequestHandler<GetLeaveRequestByIdQuery, LeaveRequestDto>
{
    private readonly IApplicationDbContext _context;
    private readonly IMapper _mapper;

    public GetLeaveRequestByIdQueryHandler(IApplicationDbContext context,IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<LeaveRequestDto> Handle(GetLeaveRequestByIdQuery request,CancellationToken cancellationToken)
    {
        var leaveRequest = await _context.LeaveRequests
            .AsNoTracking()
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && !x.IsDeleted,
                cancellationToken);

        if (leaveRequest is null)
            throw new NotFoundException("Leave request not found.");

        return _mapper.Map<LeaveRequestDto>(leaveRequest);
    }
}