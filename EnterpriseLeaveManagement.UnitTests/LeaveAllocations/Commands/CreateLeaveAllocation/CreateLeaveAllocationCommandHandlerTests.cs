using EnterpriseLeaveManagement.Application.Features.LeaveAllocations.Commands.CreateLeaveAllocation;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.UnitTests.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EnterpriseLeaveManagement.UnitTests.LeaveAllocations.Commands.CreateLeaveAllocation
{
    public class CreateLeaveAllocationCommandHandlerTests
    {
        [Fact]
        public async Task Handle_Should_Create_LeaveAllocation_Successfully()
        {
            using var context = TestDbContextFactory.Create();

            var employee = new Employee
            {
                Id = Guid.NewGuid()
            };

            var leaveType = new LeaveType
            {
                Id = Guid.NewGuid(),
                Name = "Annual",
                Code = "AL"
            };

            context.Employees.Add(employee);
            context.LeaveTypes.Add(leaveType);

            await context.SaveChangesAsync();

            var handler = new CreateLeaveAllocationCommandHandler(context);

            var allocationId = await handler.Handle(
                new CreateLeaveAllocationCommand
                {
                    EmployeeId = employee.Id,
                    LeaveTypeId = leaveType.Id,
                    Year = 2026,
                    AllocatedDays = 24
                },
                CancellationToken.None);

            Assert.NotEqual(Guid.Empty, allocationId);

            Assert.Single(context.LeaveAllocations);
            Assert.Single(context.LeaveBalances);

            var balance = context.LeaveBalances.Single();

            Assert.Equal(24, balance.TotalDays);
            Assert.Equal(0, balance.UsedDays);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_Employee_NotFound()
        {
            using var context = TestDbContextFactory.Create();

            var leaveType = new LeaveType
            {
                Id = Guid.NewGuid(),
                Name = "Annual",
                Code = "AL"
            };

            context.LeaveTypes.Add(leaveType);
            await context.SaveChangesAsync();

            var handler = new CreateLeaveAllocationCommandHandler(context);

            var ex = await Assert.ThrowsAsync<Exception>(() =>
                handler.Handle(
                    new CreateLeaveAllocationCommand
                    {
                        EmployeeId = Guid.NewGuid(),
                        LeaveTypeId = leaveType.Id,
                        Year = 2026,
                        AllocatedDays = 20
                    },
                    CancellationToken.None));

            Assert.Equal("Employee not found.", ex.Message);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_LeaveType_NotFound()
        {
            using var context = TestDbContextFactory.Create();

            var employee = new Employee
            {
                Id = Guid.NewGuid()
            };

            context.Employees.Add(employee);
            await context.SaveChangesAsync();

            var handler = new CreateLeaveAllocationCommandHandler(context);

            var ex = await Assert.ThrowsAsync<Exception>(() =>
                handler.Handle(
                    new CreateLeaveAllocationCommand
                    {
                        EmployeeId = employee.Id,
                        LeaveTypeId = Guid.NewGuid(),
                        Year = 2026,
                        AllocatedDays = 20
                    },
                    CancellationToken.None));

            Assert.Equal("Leave type not found.", ex.Message);
        }

        [Fact]
        public async Task Handle_Should_Throw_Exception_When_Allocation_Already_Exists()
        {
            using var context = TestDbContextFactory.Create();

            var employee = new Employee
            {
                Id = Guid.NewGuid()
            };

            var leaveType = new LeaveType
            {
                Id = Guid.NewGuid(),
                Name = "Annual",
                Code = "AL"
            };

            context.Employees.Add(employee);
            context.LeaveTypes.Add(leaveType);

            context.LeaveAllocations.Add(new LeaveAllocation
            {
                EmployeeId = employee.Id,
                LeaveTypeId = leaveType.Id,
                Year = 2026,
                AllocatedDays = 20
            });

            await context.SaveChangesAsync();

            var handler = new CreateLeaveAllocationCommandHandler(context);

            var ex = await Assert.ThrowsAsync<Exception>(() =>
                handler.Handle(
                    new CreateLeaveAllocationCommand
                    {
                        EmployeeId = employee.Id,
                        LeaveTypeId = leaveType.Id,
                        Year = 2026,
                        AllocatedDays = 24
                    },
                    CancellationToken.None));

            Assert.Equal("Leave allocation already exists for this year.", ex.Message);
        }

        [Fact]
        public async Task Handle_Should_Update_Existing_LeaveBalance()
        {
            using var context = TestDbContextFactory.Create();

            var employee = new Employee
            {
                Id = Guid.NewGuid()
            };

            var leaveType = new LeaveType
            {
                Id = Guid.NewGuid(),
                Name = "Annual",
                Code = "AL"
            };

            context.Employees.Add(employee);
            context.LeaveTypes.Add(leaveType);

            context.LeaveBalances.Add(new LeaveBalance
            {
                EmployeeId = employee.Id,
                LeaveTypeId = leaveType.Id,
                TotalDays = 20,
                UsedDays = 5
            });

            await context.SaveChangesAsync();

            var handler = new CreateLeaveAllocationCommandHandler(context);

            await handler.Handle(
                new CreateLeaveAllocationCommand
                {
                    EmployeeId = employee.Id,
                    LeaveTypeId = leaveType.Id,
                    Year = 2026,
                    AllocatedDays = 30
                },
                CancellationToken.None);

            var balance = context.LeaveBalances.Single();

            Assert.Equal(30, balance.TotalDays);
            Assert.Equal(5, balance.UsedDays);
        }
    }
}
