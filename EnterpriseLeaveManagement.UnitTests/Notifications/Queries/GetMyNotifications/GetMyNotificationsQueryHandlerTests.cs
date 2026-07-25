using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.Notifications.Queries.GetMyNotifications;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.UnitTests.Common;
using Moq;
using Xunit;

namespace EnterpriseLeaveManagement.UnitTests.Notifications.Queries.GetMyNotifications;

public class GetMyNotificationsQueryHandlerTests
{
    [Fact]
    public async Task Handle_Should_Return_Current_User_Notifications()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var userId = Guid.NewGuid();

        context.Notifications.AddRange(
            new Notification
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Title = "Leave Approved",
                Message = "Your leave has been approved.",
                IsRead = false,
                IsDeleted = false,
                CreatedOn = DateTime.UtcNow
            },
            new Notification
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Title = "Other User",
                Message = "Should not be returned.",
                IsRead = false,
                IsDeleted = false,
                CreatedOn = DateTime.UtcNow
            },
            new Notification
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                Title = "Deleted",
                Message = "Should not be returned.",
                IsRead = false,
                IsDeleted = true,
                CreatedOn = DateTime.UtcNow
            });

        await context.SaveChangesAsync();

        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.UserId).Returns(userId);

        var handler = new GetMyNotificationsQueryHandler(
            context,
            currentUserService.Object);

        // Act
        var result = await handler.Handle(
            new GetMyNotificationsQuery(),
            CancellationToken.None);

        // Assert
        Assert.Single(result);

        Assert.Equal("Leave Approved", result[0].Title);
        Assert.Equal("Your leave has been approved.", result[0].Message);
        Assert.False(result[0].IsRead);
    }

    [Fact]
    public async Task Handle_Should_Throw_UnauthorizedAccessException_When_User_Is_Not_Authenticated()
    {
        // Arrange
        using var context = TestDbContextFactory.Create();

        var currentUserService = new Mock<ICurrentUserService>();
        currentUserService.Setup(x => x.UserId).Returns((Guid?)null);

        var handler = new GetMyNotificationsQueryHandler(
            context,
            currentUserService.Object);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
            handler.Handle(
                new GetMyNotificationsQuery(),
                CancellationToken.None));

        Assert.Equal("User is not authenticated.", exception.Message);
    }
}