using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnterpriseLeaveManagement.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.LeaveAllocations.Commands.DeleteLeaveAllocation;

public class DeleteLeaveAllocationCommandHandler: IRequestHandler<DeleteLeaveAllocationCommand>
{
    private readonly IApplicationDbContext _context;

    public DeleteLeaveAllocationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(DeleteLeaveAllocationCommand request,CancellationToken cancellationToken)
    {
        var allocation = await _context.LeaveAllocations
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (allocation == null)
            throw new Exception("Leave allocation not found.");

        _context.LeaveAllocations.Remove(allocation);

        await _context.SaveChangesAsync(cancellationToken);
    }
}
