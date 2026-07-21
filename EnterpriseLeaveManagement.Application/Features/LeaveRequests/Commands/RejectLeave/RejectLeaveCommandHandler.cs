using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnterpriseLeaveManagement.Application.Common.Exceptions;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.LeaveRequests.Commands.RejectLeave;

public class RejectLeaveCommandHandler : IRequestHandler<RejectLeaveCommand>
{
    private readonly IApplicationDbContext _context;

    public RejectLeaveCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(RejectLeaveCommand request,CancellationToken cancellationToken)
    {
        var leaveRequest = await _context.LeaveRequests
            .FirstOrDefaultAsync(
                x => x.Id == request.LeaveRequestId && !x.IsDeleted,
                cancellationToken);

        if (leaveRequest is null)
            throw new NotFoundException("Leave request not found.");

        if (leaveRequest.Status != LeaveRequestStatus.Pending)
            throw new BadRequestException("Only pending leave requests can be rejected.");

        leaveRequest.Status = LeaveRequestStatus.Rejected;
        leaveRequest.ApprovedBy = request.ApprovedBy;
        leaveRequest.ApprovedOn = DateTime.UtcNow;
        leaveRequest.ManagerComments = request.ManagerComments;

        await _context.SaveChangesAsync(cancellationToken);
    }
}