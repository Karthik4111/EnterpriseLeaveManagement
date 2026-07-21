using EnterpriseLeaveManagement.Application.Common.Exceptions;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseLeaveManagement.Application.Common.Exceptions;

namespace EnterpriseLeaveManagement.Application.Features.Departments.Commands.CreateDepartment;

public class CreateDepartmentCommandHandler : IRequestHandler<CreateDepartmentCommand, CreateDepartmentResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateDepartmentCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CreateDepartmentResponse> Handle(CreateDepartmentCommand request,CancellationToken cancellationToken)
    {
        // Normalize input
        var name = request.Name.Trim();
        var code = request.Code.Trim().ToUpper();
        var description = request.Description?.Trim() ?? string.Empty;

        // Business Validation - Department Name
        if (await _context.Departments.AnyAsync(x => x.Name == name, cancellationToken))
        {
            throw new BusinessException("Department name already exists.");
        }

        // Business Validation - Department Code
        if (await _context.Departments.AnyAsync(x => x.Code == code, cancellationToken))
        {
            throw new BusinessException("Department code already exists.");
        }

        // Create Entity
        var department = new Department
        {
            Name = name,
            Code = code,
            Description = description,
            ManagerId = request.ManagerId,
            IsActive = true
        };

        // Save
        await _context.Departments.AddAsync(department, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        return new CreateDepartmentResponse
        {
            DepartmentId = department.Id,
            Message = "Department created successfully."
        };
    }
}
