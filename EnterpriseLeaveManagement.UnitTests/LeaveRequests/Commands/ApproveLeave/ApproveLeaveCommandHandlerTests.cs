using EnterpriseLeaveManagement.Application.Common.Exceptions;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Application.Features.LeaveRequests.Commands.ApproveLeave;
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

namespace EnterpriseLeaveManagement.UnitTests.LeaveRequests.Commands.ApproveLeave
{
    public class ApproveLeaveCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Approve_Leave_Successfully()
        {
            using var context = TestDbContextFactory.Create();

            var currentUser = new Mock<ICurrentUserService>();
            currentUser.Setup(x => x.UserId).Returns(Guid.NewGuid());

            var audit = new Mock<IAuditService>();
            var email = new Mock<IEmailService>();
            var logger = new Mock<ILogger<ApproveLeaveCommandHandler>>();

            var employee = new Employee
            {
                Id = Guid.NewGuid(),
                UserId = Guid.NewGuid(),
                FirstName = "Gopi",
                Email = "gopi@company.com"
            };

            var leaveType = new LeaveType
            {
                Id = Guid.NewGuid(),
                Name = "Annual",
                Code = "AL"
            };

            var balance = new LeaveBalance
            {
                EmployeeId = employee.Id,
                LeaveTypeId = leaveType.Id,
                TotalDays = 20,
                UsedDays = 0
            };

            var request = new LeaveRequest
            {
                Id = Guid.NewGuid(),
                EmployeeId = employee.Id,
                LeaveTypeId = leaveType.Id,
                StartDate = new DateOnly(2026, 8, 1),
                EndDate = new DateOnly(2026, 8, 3),
                NumberOfDays = 3,
                Status = LeaveRequestStatus.Pending
            };

            context.Employees.Add(employee);
            context.LeaveTypes.Add(leaveType);
            context.LeaveBalances.Add(balance);
            context.LeaveRequests.Add(request);

            await context.SaveChangesAsync();

            var handler = new ApproveLeaveCommandHandler(
                context,
                currentUser.Object,
                audit.Object,
                email.Object,
                logger.Object);

            await handler.Handle(
                new ApproveLeaveCommand
                {
                    LeaveRequestId = request.Id,
                    ManagerComments = "Approved"
                },
                CancellationToken.None);

            var updated = await context.LeaveRequests.FindAsync(request.Id);

            Assert.NotNull(updated);
            Assert.Equal(LeaveRequestStatus.Approved, updated!.Status);
            Assert.Equal("Approved", updated.ManagerComments);

            var updatedBalance = context.LeaveBalances.Single();
            Assert.Equal(3, updatedBalance.UsedDays);

            Assert.Single(context.Notifications);
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFound_When_LeaveRequest_NotFound()
        {
            using var context = TestDbContextFactory.Create();

            var handler = new ApproveLeaveCommandHandler(
                context,
                Mock.Of<ICurrentUserService>(),
                Mock.Of<IAuditService>(),
                Mock.Of<IEmailService>(),
                Mock.Of<ILogger<ApproveLeaveCommandHandler>>());

            await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(
                    new ApproveLeaveCommand
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

            var handler = new ApproveLeaveCommandHandler(
                context,
                currentUser.Object,
                Mock.Of<IAuditService>(),
                Mock.Of<IEmailService>(),
                Mock.Of<ILogger<ApproveLeaveCommandHandler>>());

            var ex = await Assert.ThrowsAsync<BadRequestException>(() =>
                handler.Handle(
                    new ApproveLeaveCommand
                    {
                        LeaveRequestId = request.Id
                    },
                    CancellationToken.None));

            Assert.Equal("Only pending leave requests can be approved.", ex.Message);
        }

        [Fact]
        public async Task Handle_Should_Throw_UnauthorizedAccessException_When_User_Is_Not_Authenticated()
        {
            using var context = TestDbContextFactory.Create();

            var employee = new Employee
            {
                Id = Guid.NewGuid()
            };

            var leaveType = new LeaveType
            {
                Id = Guid.NewGuid()
            };

            context.Employees.Add(employee);

            context.LeaveBalances.Add(new LeaveBalance
            {
                EmployeeId = employee.Id,
                LeaveTypeId = leaveType.Id,
                TotalDays = 20,
                UsedDays = 0
            });

            context.LeaveRequests.Add(new LeaveRequest
            {
                Id = Guid.NewGuid(),
                EmployeeId = employee.Id,
                LeaveTypeId = leaveType.Id,
                NumberOfDays = 2,
                Status = LeaveRequestStatus.Pending
            });

            await context.SaveChangesAsync();

            var currentUser = new Mock<ICurrentUserService>();
            currentUser.Setup(x => x.UserId).Returns((Guid?)null);

            var handler = new ApproveLeaveCommandHandler(
                context,
                currentUser.Object,
                Mock.Of<IAuditService>(),
                Mock.Of<IEmailService>(),
                Mock.Of<ILogger<ApproveLeaveCommandHandler>>());

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() =>
                handler.Handle(
                    new ApproveLeaveCommand
                    {
                        LeaveRequestId = context.LeaveRequests.Single().Id
                    },
                    CancellationToken.None));
        }

        [Fact]
        public async Task Handle_Should_Throw_NotFoundException_When_LeaveBalance_NotFound()
        {
            using var context = TestDbContextFactory.Create();

            var employee = new Employee
            {
                Id = Guid.NewGuid()
            };

            var leaveType = new LeaveType
            {
                Id = Guid.NewGuid()
            };

            context.Employees.Add(employee);

            context.LeaveRequests.Add(new LeaveRequest
            {
                Id = Guid.NewGuid(),
                EmployeeId = employee.Id,
                LeaveTypeId = leaveType.Id,
                NumberOfDays = 2,
                Status = LeaveRequestStatus.Pending
            });

            await context.SaveChangesAsync();

            var currentUser = new Mock<ICurrentUserService>();
            currentUser.Setup(x => x.UserId).Returns(Guid.NewGuid());

            var handler = new ApproveLeaveCommandHandler(
                context,
                currentUser.Object,
                Mock.Of<IAuditService>(),
                Mock.Of<IEmailService>(),
                Mock.Of<ILogger<ApproveLeaveCommandHandler>>());

            var ex = await Assert.ThrowsAsync<NotFoundException>(() =>
                handler.Handle(
                    new ApproveLeaveCommand
                    {
                        LeaveRequestId = context.LeaveRequests.Single().Id
                    },
                    CancellationToken.None));

            Assert.Equal("Leave balance not found.", ex.Message);
        }

        [Fact]
        public async Task Handle_Should_Throw_BadRequestException_When_LeaveBalance_Is_Insufficient()
        {
            using var context = TestDbContextFactory.Create();

            var employee = new Employee
            {
                Id = Guid.NewGuid()
            };

            var leaveType = new LeaveType
            {
                Id = Guid.NewGuid()
            };

            context.Employees.Add(employee);

            context.LeaveBalances.Add(new LeaveBalance
            {
                EmployeeId = employee.Id,
                LeaveTypeId = leaveType.Id,
                TotalDays = 2,
                UsedDays = 0
            });

            context.LeaveRequests.Add(new LeaveRequest
            {
                Id = Guid.NewGuid(),
                EmployeeId = employee.Id,
                LeaveTypeId = leaveType.Id,
                NumberOfDays = 5,
                Status = LeaveRequestStatus.Pending
            });

            await context.SaveChangesAsync();

            var currentUser = new Mock<ICurrentUserService>();
            currentUser.Setup(x => x.UserId).Returns(Guid.NewGuid());

            var handler = new ApproveLeaveCommandHandler(
                context,
                currentUser.Object,
                Mock.Of<IAuditService>(),
                Mock.Of<IEmailService>(),
                Mock.Of<ILogger<ApproveLeaveCommandHandler>>());

            var ex = await Assert.ThrowsAsync<BadRequestException>(() =>
                handler.Handle(
                    new ApproveLeaveCommand
                    {
                        LeaveRequestId = context.LeaveRequests.Single().Id
                    },
                    CancellationToken.None));

            Assert.Equal("Insufficient leave balance.", ex.Message);
        }
    }
}
