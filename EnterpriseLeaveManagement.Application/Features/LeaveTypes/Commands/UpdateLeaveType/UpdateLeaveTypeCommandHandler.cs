using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseLeaveManagement.Application.Common.Exceptions;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.LeaveTypes.Commands.UpdateLeaveType;

public class UpdateLeaveTypeCommandHandler
    : IRequestHandler<UpdateLeaveTypeCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateLeaveTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        var leaveType = await _context.LeaveTypes
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (leaveType == null)
        {
            throw new NotFoundException("Leave type not found.");
        }

        leaveType.Name = request.Name;
        leaveType.Code = request.Code;
        leaveType.Description = request.Description;
        leaveType.DefaultDays = request.DefaultDays;
        leaveType.IsPaidLeave = request.IsPaidLeave;
        leaveType.CarryForwardAllowed = request.CarryForwardAllowed;
        leaveType.MaximumCarryForwardDays = request.MaximumCarryForwardDays;
        leaveType.RequiresApproval = request.RequiresApproval;
        leaveType.IsActive = request.IsActive;

        await _context.SaveChangesAsync(cancellationToken);
    }
}