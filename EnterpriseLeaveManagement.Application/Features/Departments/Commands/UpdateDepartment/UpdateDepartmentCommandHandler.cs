using EnterpriseLeaveManagement.Application.Common.Exceptions;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseLeaveManagement.Application.Features.Departments.Commands.UpdateDepartment;

public class UpdateDepartmentCommandHandler
    : IRequestHandler<UpdateDepartmentCommand, UpdateDepartmentResponse>
{
    private readonly IApplicationDbContext _context;

    public UpdateDepartmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<UpdateDepartmentResponse> Handle(UpdateDepartmentCommand request,CancellationToken cancellationToken)
    {
        var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == request.Id && d.IsActive, cancellationToken);

        if (department is null)
        {
            throw new NotFoundException("Department not found.");
        }

        var name = request.Name.Trim();
        var code = request.Code.Trim().ToUpper();

        if (await _context.Departments.AnyAsync(d => d.Id != request.Id && d.Name == name,cancellationToken))
        {
            throw new BusinessException("Department name already exists.");
        }

        if (await _context.Departments.AnyAsync(d => d.Id != request.Id && d.Code == code,cancellationToken))
        {
            throw new BusinessException("Department code already exists.");
        }

        department.Name = name;
        department.Code = code;
        department.Description = request.Description.Trim();
        department.ManagerId = request.ManagerId;

        await _context.SaveChangesAsync(cancellationToken);

        return new UpdateDepartmentResponse
        {
            DepartmentId = department.Id,
            Message = "Department updated successfully."
        };
    }
}
