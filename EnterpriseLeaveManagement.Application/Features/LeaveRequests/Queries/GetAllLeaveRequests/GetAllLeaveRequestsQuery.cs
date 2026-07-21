using EnterpriseLeaveManagement.Application.Common.Models;
using EnterpriseLeaveManagement.Application.Features.LeaveRequests.DTOs;
using EnterpriseLeaveManagement.Domain.Enums;
using MediatR;

namespace EnterpriseLeaveManagement.Application.Features.LeaveRequests.Queries.GetAllLeaveRequests;

public class GetAllLeaveRequestsQuery : IRequest<PagedResult<LeaveRequestDto>>
{
    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;

    public string? Search { get; set; }

    public LeaveRequestStatus? Status { get; set; }

    public Guid? EmployeeId { get; set; }

    public string SortBy { get; set; } = "StartDate";

    public string SortOrder { get; set; } = "desc";
}