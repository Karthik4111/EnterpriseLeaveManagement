using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnterpriseLeaveManagement.Application.Common.Models;
using EnterpriseLeaveManagement.Application.Features.Employees.DTOs;
using MediatR;

namespace EnterpriseLeaveManagement.Application.Features.Employees.Queries.GetEmployees;

public class GetEmployeesQuery : PaginationParameters, IRequest<PagedResult<EmployeeDto>>
{
    public string? Search { get; set; }

    public Guid? DepartmentId { get; set; }

    public bool? IsActive { get; set; }

    public string? SortBy { get; set; }

    public string? SortOrder { get; set; } = "asc";
}