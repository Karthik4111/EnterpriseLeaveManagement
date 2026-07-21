using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseLeaveManagement.Application.Common.Exceptions;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.LeaveTypes.Commands.DeleteLeaveType;

public class DeleteLeaveTypeCommandHandler : IRequestHandler<DeleteLeaveTypeCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteLeaveTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteLeaveTypeCommand request, CancellationToken cancellationToken)
    {
        var leaveType = await _context.LeaveTypes.FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (leaveType == null)
        {
            throw new NotFoundException("Leave type not found.");
        }

        // Soft Delete
        leaveType.IsDeleted = true;
        leaveType.IsActive = false;

        await _context.SaveChangesAsync(cancellationToken);
    }
}