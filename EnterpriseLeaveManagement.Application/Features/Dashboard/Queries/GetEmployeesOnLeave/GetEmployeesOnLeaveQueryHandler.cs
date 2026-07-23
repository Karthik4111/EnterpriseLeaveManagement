using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.Dashboard.DTOs;
using EnterpriseLeaveManagement.Domain.Enums;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.Dashboard.Queries.GetEmployeesOnLeave;

public class GetEmployeesOnLeaveQueryHandler: IRequestHandler<GetEmployeesOnLeaveQuery, List<EmployeeOnLeaveDto>>
{
    private readonly IApplicationDbContext _context;

    public GetEmployeesOnLeaveQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<EmployeeOnLeaveDto>> Handle(
        GetEmployeesOnLeaveQuery request,
        CancellationToken cancellationToken)
    {
        var today = DateOnly.FromDateTime(DateTime.Today);

        return await _context.LeaveRequests
            .Where(x =>
                !x.IsDeleted &&
                x.Status == LeaveRequestStatus.Approved &&
                x.StartDate <= today &&
                x.EndDate >= today)
            .Select(x => new EmployeeOnLeaveDto
            {
                EmployeeId = x.EmployeeId,
                EmployeeName = $"{x.Employee.FirstName} {x.Employee.LastName}",
                LeaveType = x.LeaveType.Name,
                StartDate = x.StartDate,
                EndDate = x.EndDate
            })
            .ToListAsync(cancellationToken);
    }
}