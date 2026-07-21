using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using EnterpriseLeaveManagement.Application.Common.Models;
using EnterpriseLeaveManagement.Application.Features.LeaveTypes.DTOs;
using MediatR;

namespace EnterpriseLeaveManagement.Application.Features.LeaveTypes.Queries.GetAllLeaveTypes;

public class GetAllLeaveTypesQuery : IRequest<PagedResult<LeaveTypeDto>>
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? Search { get; set; }

    public string? SortBy { get; set; }

    public string SortOrder { get; set; } = "asc";

    public bool? IsActive { get; set; }
}