using EnterpriseLeaveManagement.Application.Common.Exceptions;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.LeaveRequests.Commands.RejectLeave;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.Domain.Enums;
using EnterpriseLeaveManagement.UnitTests.Common;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseLeaveManagement.UnitTests.LeaveRequests.Commands.RejectLeave
{
    public class RejectLeaveCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Reject_Leave_Successfully()
        {
            using var context = TestDbContextFactory.Create();

            var currentUser = new Mock<ICurrentUserService>();
            currentUser.Setup(x => x.UserId).Returns(Guid.NewGuid());

            var handler = new RejectLeaveCommandHandler(
                context,
                currentUser.Object,
                Mock.Of<IAuditService>(),
                Mock.Of<IEmailService>(),
                Mock.Of<ILogger<RejectLeaveCommandHandler>>());

            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                FirstName = "Gopi",
                Email = "gopi@company.com"
            };

            var request = new LeaveRequest
            {
                Id = Guid.NewGuid(),
                EmployeeId = employee.Id,
                StartDate = new DateOnly(2026, 8, 1),
                EndDate = new DateOnly(2026, 8, 3),
                NumberOfDays = 3,
                Status = LeaveRequestStatus.Pending
            };

            context.Employees.Add(employee);
            context.LeaveRequests.Add(request);

            await context.SaveChangesAsync();

            await handler.Handle(
                new RejectLeaveCommand
                {
                    LeaveRequestId = request.Id,
                    ManagerComments = "Project deadline"
                },
                CancellationToken.None);

            var updated = await context.LeaveRequests.FindAsync(request.Id);

            Assert.NotNull(updated);
            Assert.Equal(LeaveRequestStatus.Rejected, updated!.Status);
            Assert.Equal("Project deadline", updated.ManagerComments);

            Assert.Single(context.Notifications);
        }


        [Fact]
        public async Task Handle_Should_Throw_NotFoundException_When_LeaveRequest_NotFound()
        {
            using var context = TestDbContextFactory.Create();

            var handler = new RejectLeaveCommandHandler(
                context,
                Mock.Of<ICurrentUserService>(),
                Mock.Of<IAuditService>(),
                Mock.Of<IEmailService>(),
                Mock.Of<ILogger<RejectLeaveCommandHandler>>());

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(
                    new RejectLeaveCommand
                    {
                        LeaveRequestId = Guid.NewGuid()
                    },
                    CancellationToken.None));
        }


        [Fact]
        public async Task Handle_Should_Throw_BadRequestException_When_Request_Is_Not_Pending()
        {
            using var context = TestDbContextFactory.Create();

            var currentUser = new Mock<ICurrentUserService>();
            currentUser.Setup(x => x.UserId).Returns(Guid.NewGuid());

            var request = new LeaveRequest
            {
                Id = Guid.NewGuid(),
                Status = LeaveRequestStatus.Approved
            };

            context.LeaveRequests.Add(request);
            await context.SaveChangesAsync();

            var handler = new RejectLeaveCommandHandler(
                context,
                currentUser.Object,
                Mock.Of<IAuditService>(),
                Mock.Of<IEmailService>(),
                Mock.Of<ILogger<RejectLeaveCommandHandler>>());

            var ex = await Assert.ThrowsAsync<BadRequestException>(() =>
                handler.Handle(
                    new RejectLeaveCommand
                    {
                        LeaveRequestId = request.Id
                    },
                    CancellationToken.None));

            Assert.Equal("Only pending leave requests can be rejected.", ex.Message);
        }


        [Fact]
        public async Task Handle_Should_Throw_UnauthorizedAccessException_When_User_Is_Not_Authenticated()
        {
            using var context = TestDbContextFactory.Create();

            var employee = new Employee
            {
                Id = Guid.NewGuid()
            };

            var request = new LeaveRequest
            {
                Id = Guid.NewGuid(),
                EmployeeId = employee.Id,
                Status = LeaveRequestStatus.Pending
            };

            context.Employees.Add(employee);
            context.LeaveRequests.Add(request);

            await context.SaveChangesAsync();

            var currentUser = new Mock<ICurrentUserService>();
            currentUser.Setup(x => x.UserId).Returns((Guid?)null);

            var handler = new RejectLeaveCommandHandler(
                context,
                currentUser.Object,
                Mock.Of<IAuditService>(),
                Mock.Of<IEmailService>(),
                Mock.Of<ILogger<RejectLeaveCommandHandler>>());

            var ex = await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                handler.Handle(
                    new RejectLeaveCommand
                    {
                        LeaveRequestId = request.Id
                    },
                    CancellationToken.None));

            Assert.Equal("User is not authenticated.", ex.Message);
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFoundException_When_Employee_NotFound()
        {
            using var context = TestDbContextFactory.Create();

            var currentUser = new Mock<ICurrentUserService>();
            currentUser.Setup(x => x.UserId).Returns(Guid.NewGuid());

            var request = new LeaveRequest
            {
                Id = Guid.NewGuid(),
                EmployeeId = Guid.NewGuid(),
                Status = LeaveRequestStatus.Pending
            };

            context.LeaveRequests.Add(request);

            await context.SaveChangesAsync();

            var handler = new RejectLeaveCommandHandler(
                context,
                currentUser.Object,
                Mock.Of<IAuditService>(),
                Mock.Of<IEmailService>(),
                Mock.Of<ILogger<RejectLeaveCommandHandler>>());

            var ex = await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(
                    new RejectLeaveCommand
                    {
                        LeaveRequestId = request.Id
                    },
                    CancellationToken.None));

            Assert.Equal("Employee not found.", ex.Message);
        }


    }
}
