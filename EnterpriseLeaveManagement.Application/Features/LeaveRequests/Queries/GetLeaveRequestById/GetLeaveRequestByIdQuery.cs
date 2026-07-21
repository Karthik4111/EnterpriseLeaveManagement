using EnterpriseLeaveManagement.Application.Features.LeaveRequests.DTOs;
using MediatR;

namespace EnterpriseLeaveManagement.Application.Features.LeaveRequests.Queries.GetLeaveRequestById;

public record GetLeaveRequestByIdQuery(Guid Id) : IRequest<LeaveRequestDto>;