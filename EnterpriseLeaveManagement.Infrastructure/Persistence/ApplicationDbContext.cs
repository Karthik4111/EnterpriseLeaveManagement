using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EnterpriseLeaveManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Infrastructure.Persistence;

public class ApplicationDbContext: IdentityDbContext<ApplicationUser, ApplicationRole, Guid>, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
    }

    public DbSet<Employee> Employees => Set<Employee>();

    public DbSet<Department> Departments => Set<Department>();

    public DbSet<LeaveType> LeaveTypes => Set<LeaveType>();

    public DbSet<LeaveBalance> LeaveBalances => Set<LeaveBalance>();

    public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

}
