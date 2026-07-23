using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Common.Models;
using EnterpriseLeaveManagement.Application.Features.Employees.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.Employees.Queries.GetEmployees;

public class GetEmployeesQueryHandler : IRequestHandler<GetEmployeesQuery, PagedResult<EmployeeDto>>
{
    private readonly IApplicationDbContext _context;

    public GetEmployeesQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PagedResult<EmployeeDto>> Handle(GetEmployeesQuery request,CancellationToken cancellationToken)
    {
        var query = _context.Employees
            .Where(e => !e.IsDeleted);

        // Search
        if (!string.IsNullOrWhiteSpace(request.Search))
        {
            query = query.Where(e =>
                e.FirstName.Contains(request.Search) ||
                e.LastName.Contains(request.Search) ||
                e.EmployeeCode.Contains(request.Search) ||
                e.Email.Contains(request.Search));
        }

        // Filter by Department
        if (request.DepartmentId.HasValue)
        {
            query = query.Where(e => e.DepartmentId == request.DepartmentId.Value);
        }

        // Filter by Active Status
        if (request.IsActive.HasValue)
        {
            query = query.Where(e => e.IsActive == request.IsActive.Value);
        }

        // Sorting
        query = request.SortBy?.ToLower() switch
        {
            "firstname" => request.SortOrder?.ToLower() == "desc"
                ? query.OrderByDescending(e => e.FirstName)
                : query.OrderBy(e => e.FirstName),

            "lastname" => request.SortOrder?.ToLower() == "desc"
                ? query.OrderByDescending(e => e.LastName)
                : query.OrderBy(e => e.LastName),

            "employeecode" => request.SortOrder?.ToLower() == "desc"
                ? query.OrderByDescending(e => e.EmployeeCode)
                : query.OrderBy(e => e.EmployeeCode),

            "email" => request.SortOrder?.ToLower() == "desc"
                ? query.OrderByDescending(e => e.Email)
                : query.OrderBy(e => e.Email),

            _ => query.OrderBy(e => e.FirstName)
        };

        var totalCount = await query.CountAsync(cancellationToken);

        var employees = await query
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(e => new EmployeeDto
            {
                Id = e.Id,
                EmployeeCode = e.EmployeeCode,
                FirstName = e.FirstName,
                LastName = e.LastName,
                Email = e.Email,
                PhoneNumber = e.PhoneNumber,
                Designation = e.Designation,
                DepartmentName = e.Department.Name,
                IsActive = e.IsActive
            })
            .ToListAsync(cancellationToken);

        return new PagedResult<EmployeeDto>
        {
            Items = employees,
            PageNumber = request.PageNumber,
            PageSize = request.PageSize,
            TotalCount = totalCount
        };
    }
}