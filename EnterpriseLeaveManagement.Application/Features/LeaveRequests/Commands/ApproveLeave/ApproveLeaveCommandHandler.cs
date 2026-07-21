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

namespace EnterpriseLeaveManagement.Application.Features.LeaveRequests.Commands.ApproveLeave;

public class ApproveLeaveCommandHandler : IRequestHandler<ApproveLeaveCommand>
{
    private readonly IApplicationDbContext _context;

    public ApproveLeaveCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(ApproveLeaveCommand request,CancellationToken cancellationToken)
    {
        var leaveRequest = await _context.LeaveRequests
            .FirstOrDefaultAsync(
                x => x.Id == request.LeaveRequestId && !x.IsDeleted,
                cancellationToken);

        if (leaveRequest is null)
            throw new NotFoundException("Leave request not found.");

        if (leaveRequest.Status != LeaveRequestStatus.Pending)
            throw new BadRequestException("Only pending leave requests can be approved.");

        leaveRequest.Status = LeaveRequestStatus.Approved;
        leaveRequest.ApprovedBy = request.ApprovedBy;
        leaveRequest.ApprovedOn = DateTime.UtcNow;
        leaveRequest.ManagerComments = request.ManagerComments;

        var leaveBalance = await _context.LeaveBalances
            .FirstOrDefaultAsync(x =>
                x.EmployeeId == leaveRequest.EmployeeId &&
                x.LeaveTypeId == leaveRequest.LeaveTypeId &&
                !x.IsDeleted,
                cancellationToken);

        if (leaveBalance is null)
            throw new NotFoundException("Leave balance not found.");

        if (leaveBalance.TotalDays - leaveBalance.UsedDays < leaveRequest.NumberOfDays)
            throw new BadRequestException("Insufficient leave balance.");

        leaveBalance.UsedDays += leaveRequest.NumberOfDays;

        await _context.SaveChangesAsync(cancellationToken);
    }
}