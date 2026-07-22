using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.Notifications.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Application.Features.Notifications.Queries.GetMyNotifications;

public class GetMyNotificationsQueryHandler: IRequestHandler<GetMyNotificationsQuery, List<NotificationDto>>
{
    private readonly IApplicationDbContext _context;
    private readonly ICurrentUserService _currentUserService;

    public GetMyNotificationsQueryHandler(IApplicationDbContext context,ICurrentUserService currentUserService)
    {
        _context = context;
        _currentUserService = currentUserService;
    }

    public async Task<List<NotificationDto>> Handle(
        GetMyNotificationsQuery request,
        CancellationToken cancellationToken)
    {
        if (_currentUserService.UserId is null)
            throw new UnauthorizedAccessException("User is not authenticated.");

        return await _context.Notifications
            .Where(x =>
                x.UserId == _currentUserService.UserId &&
                !x.IsDeleted)
            .OrderByDescending(x => x.CreatedOn)
            .Select(x => new NotificationDto
            {
                Id = x.Id,
                Title = x.Title,
                Message = x.Message,
                IsRead = x.IsRead,
                CreatedOn = x.CreatedOn
            })
            .ToListAsync(cancellationToken);
    }
}