using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Domain.Entities;
using MediatR;

namespace EnterpriseLeaveManagement.Application.Features.LeaveTypes.Commands.CreateLeaveType;

public class CreateLeaveTypeCommandHandler: IRequestHandler<CreateLeaveTypeCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateLeaveTypeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(CreateLeaveTypeCommand request,CancellationToken cancellationToken)
    {
        var leaveType = new LeaveType
        {
            Name = request.Name,
            Code = request.Code,
            Description = request.Description,
            DefaultDays = request.DefaultDays,
            IsPaidLeave = request.IsPaidLeave,
            CarryForwardAllowed = request.CarryForwardAllowed,
            MaximumCarryForwardDays = request.MaximumCarryForwardDays,
            RequiresApproval = request.RequiresApproval,
            IsActive = request.IsActive
        };

        _context.LeaveTypes.Add(leaveType);

        await _context.SaveChangesAsync(cancellationToken);

        return leaveType.Id;
    }
}