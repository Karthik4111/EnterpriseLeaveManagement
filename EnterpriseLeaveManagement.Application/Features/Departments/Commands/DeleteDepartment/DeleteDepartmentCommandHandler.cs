using EnterpriseLeaveManagement.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseLeaveManagement.Application.Common.Exceptions;

namespace EnterpriseLeaveManagement.Application.Features.Departments.Commands.DeleteDepartment;

public class DeleteDepartmentCommandHandler: IRequestHandler<DeleteDepartmentCommand, DeleteDepartmentResponse>
{
    private readonly IApplicationDbContext _context;

    public DeleteDepartmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<DeleteDepartmentResponse> Handle(DeleteDepartmentCommand request,CancellationToken cancellationToken)
    {
        var department = await _context.Departments.FirstOrDefaultAsync(d => d.Id == request.Id && d.IsActive, cancellationToken);

        if (department is null)
        {
            throw new NotFoundException("Department not found.");
        }

        department.IsActive = false;

        await _context.SaveChangesAsync(cancellationToken);

        return new DeleteDepartmentResponse
        {
            Message = "Department deleted successfully."
        };
    }
}