using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnterpriseLeaveManagement.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.LeaveAllocations.Commands.UpdateLeaveAllocation;

public class UpdateLeaveAllocationCommandHandler: IRequestHandler<UpdateLeaveAllocationCommand>
{
    private readonly IApplicationDbContext _context;

    public UpdateLeaveAllocationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task Handle(UpdateLeaveAllocationCommand request,CancellationToken cancellationToken)
    {
        var allocation = await _context.LeaveAllocations
            .FirstOrDefaultAsync(x => x.Id == request.Id, cancellationToken);

        if (allocation == null)
            throw new Exception("Leave allocation not found.");

        allocation.AllocatedDays = request.AllocatedDays;
        allocation.Year = request.Year;

        await _context.SaveChangesAsync(cancellationToken);
    }
}