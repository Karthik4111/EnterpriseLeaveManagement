using EnterpriseLeaveManagement.Application.Features.Notifications.DTOs;
using MediatR;

namespace EnterpriseLeaveManagement.Application.Features.Notifications.Queries.GetMyNotifications;

public record GetMyNotificationsQuery(): IRequest<List<NotificationDto>>;