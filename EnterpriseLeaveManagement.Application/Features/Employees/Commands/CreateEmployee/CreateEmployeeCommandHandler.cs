using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using EnterpriseLeaveManagement.Application.Common.Exceptions;

namespace EnterpriseLeaveManagement.Application.Features.Employees.Commands.CreateEmployee;

public class CreateEmployeeCommandHandler: IRequestHandler<CreateEmployeeCommand, CreateEmployeeResponse>
{
    private readonly IApplicationDbContext _context;

    public CreateEmployeeCommandHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<CreateEmployeeResponse> Handle(CreateEmployeeCommand request,CancellationToken cancellationToken)
    {
        // Normalize input
        var employeeCode = request.EmployeeCode.Trim();
        var email = request.Email.Trim().ToLowerInvariant();

        // Business Validation - Employee Code
        var employeeCodeExists = await _context.Employees.AnyAsync(
            x => x.EmployeeCode == employeeCode,
            cancellationToken);

        if (employeeCodeExists)
        {
            throw new BusinessException("Employee Code already exists.");
        }

        // Business Validation - Email
        var emailExists = await _context.Employees.AnyAsync(
            x => x.Email.ToLower() == email,
            cancellationToken);

        if (emailExists)
        {
            throw new BusinessException("Email already exists.");
        }

        // Business Validation - Department
        var departmentExists = await _context.Departments.AnyAsync(
            x => x.Id == request.DepartmentId,
            cancellationToken);

        if (!departmentExists)
        {
            throw new BusinessException("Department does not exist.");
        }

        // Map Command to Domain Entity
        var employee = new Employee
        {
            EmployeeCode = employeeCode,
            FirstName = request.FirstName.Trim(),
            LastName = request.LastName.Trim(),
            Email = email,
            PhoneNumber = request.PhoneNumber.Trim(),
            DateOfBirth = request.DateOfBirth,
            DateOfJoining = request.DateOfJoining,
            DepartmentId = request.DepartmentId,
            ManagerId = request.ManagerId,
            Designation = request.Designation.Trim(),
            IsActive = true
        };

        // Persist Entity
        await _context.Employees.AddAsync(employee, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);

        // Return Response
        return new CreateEmployeeResponse
        {
            EmployeeId = employee.Id,
            Message = "Employee created successfully."
        };
    }
}