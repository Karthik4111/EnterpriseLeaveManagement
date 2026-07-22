using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MediatR;
using Microsoft.EntityFrameworkCore;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Domain.Entities;

namespace EnterpriseLeaveManagement.Application.Features.LeaveAllocations.Commands.CreateLeaveAllocation;

public class CreateLeaveAllocationCommandHandler: IRequestHandler<CreateLeaveAllocationCommand, Guid>
{
    private readonly IApplicationDbContext _context;

    public CreateLeaveAllocationCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<Guid> Handle(
        CreateLeaveAllocationCommand request,
        CancellationToken cancellationToken)
    {
        // Employee exists
        var employeeExists = await _context.Employees
            .AnyAsync(x => x.Id == request.EmployeeId, cancellationToken);

        if (!employeeExists)
            throw new Exception("Employee not found.");

        // Leave Type exists
        var leaveTypeExists = await _context.LeaveTypes
            .AnyAsync(x => x.Id == request.LeaveTypeId, cancellationToken);

        if (!leaveTypeExists)
            throw new Exception("Leave type not found.");

        // Prevent duplicate allocation
        var allocationExists = await _context.LeaveAllocations
            .AnyAsync(x =>
                x.EmployeeId == request.EmployeeId &&
                x.LeaveTypeId == request.LeaveTypeId &&
                x.Year == request.Year,
                cancellationToken);

        if (allocationExists)
            throw new Exception("Leave allocation already exists for this year.");

        var allocation = new LeaveAllocation
        {
            EmployeeId = request.EmployeeId,
            LeaveTypeId = request.LeaveTypeId,
            Year = request.Year,
            AllocatedDays = request.AllocatedDays
        };

        _context.LeaveAllocations.Add(allocation);

        var leaveBalance = await _context.LeaveBalances
                            .FirstOrDefaultAsync(x =>
                                x.EmployeeId == request.EmployeeId &&
                                x.LeaveTypeId == request.LeaveTypeId,
                                cancellationToken);

        if (leaveBalance == null)
        {
            leaveBalance = new LeaveBalance
            {
                EmployeeId = request.EmployeeId,
                LeaveTypeId = request.LeaveTypeId,
                TotalDays = request.AllocatedDays,
                UsedDays = 0
            };

            _context.LeaveBalances.Add(leaveBalance);
        }
        else
        {
            leaveBalance.TotalDays = request.AllocatedDays;
        }

        await _context.SaveChangesAsync(cancellationToken);

        return allocation.Id;
    }
}