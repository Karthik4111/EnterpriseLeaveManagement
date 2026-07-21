using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.Domain.Enums;
using MediatR;

namespace EnterpriseLeaveManagement.Application.Features.LeaveRequests.Commands.ApplyLeave;

public class ApplyLeaveCommandHandler : IRequestHandler<ApplyLeaveCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public ApplyLeaveCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(ApplyLeaveCommand request,CancellationToken cancellationToken)
    {
        var numberOfDays =
            request.EndDate.DayNumber -
            request.StartDate.DayNumber + 1;

        var leaveRequest = new LeaveRequest
        {
            EmployeeId = request.EmployeeId,
            LeaveTypeId = request.LeaveTypeId,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            NumberOfDays = numberOfDays,
            LeaveReason = request.LeaveReason,
            Status = LeaveRequestStatus.Pending
        };

        _context.LeaveRequests.Add(leaveRequest);

        await _context.SaveChangesAsync(cancellationToken);

        return leaveRequest.Id;
    }
}