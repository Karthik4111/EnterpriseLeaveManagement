using EnterpriseLeaveManagement.Application.Common.Exceptions;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.LeaveRequests.Commands.CancelLeave;

public class CancelLeaveCommandHandler : IRequestHandler<CancelLeaveCommand>
{
    private readonly IApplicationDbContext _context;

    public CancelLeaveCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(
        CancelLeaveCommand request,
        CancellationToken cancellationToken)
    {
        var leaveRequest = await _context.LeaveRequests
            .FirstOrDefaultAsync(
                x => x.Id == request.Id && !x.IsDeleted,
                cancellationToken);

        if (leaveRequest is null)
            throw new NotFoundException("Leave request not found.");

        if (leaveRequest.Status != LeaveRequestStatus.Pending)
            throw new BadRequestException("Only pending leave requests can be cancelled.");

        leaveRequest.Status = LeaveRequestStatus.Cancelled;

        await _context.SaveChangesAsync(cancellationToken);
    }
}