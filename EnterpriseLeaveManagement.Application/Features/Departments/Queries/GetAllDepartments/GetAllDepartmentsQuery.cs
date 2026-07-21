using EnterpriseLeaveManagement.Application.Common.Models;
using EnterpriseLeaveManagement.Application.Features.Departments.Common;
using MediatR;

namespace EnterpriseLeaveManagement.Application.Features.Departments.Queries.GetAllDepartments;

public class GetAllDepartmentsQuery : IRequest<PagedResult<DepartmentDto>>
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? Search { get; set; }

    public string? SortBy { get; set; }

    public string SortOrder { get; set; } = "asc";

    public bool? IsActive { get; set; }
}