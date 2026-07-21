using MediatR;

namespace EnterpriseLeaveManagement.Application.Features.LeaveRequests.Commands.CancelLeave;

public record CancelLeaveCommand(Guid Id) : IRequest;