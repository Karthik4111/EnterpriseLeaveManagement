using EnterpriseLeaveManagement.Application.Common.Interfaces;
using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace EnterpriseLeaveManagement.Infrastructure.Persistence;

public class ApplicationDbContext: IdentityDbContext<ApplicationUser, ApplicationRole, Guid>,IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options): base(options)
    {
    }

    public DbSet<Employee> Employees => Set<Employee>();

    public DbSet<Department> Departments => Set<Department>();

    public DbSet<LeaveType> LeaveTypes => Set<LeaveType>();

    public DbSet<LeaveBalance> LeaveBalances => Set<LeaveBalance>();

    public DbSet<LeaveRequest> LeaveRequests => Set<LeaveRequest>();

    public DbSet<LeaveAllocation> LeaveAllocations => Set<LeaveAllocation>();

    public DbSet<Notification> Notifications => Set<Notification>();

    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);

        // Leave Allocation
        modelBuilder.Entity<LeaveAllocation>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.AllocatedDays)
                  .IsRequired();

            entity.Property(x => x.Year)
                  .IsRequired();

            entity.HasOne(x => x.Employee)
                  .WithMany(x => x.LeaveAllocations)
                  .HasForeignKey(x => x.EmployeeId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(x => x.LeaveType)
                  .WithMany(x => x.LeaveAllocations)
                  .HasForeignKey(x => x.LeaveTypeId)
                  .OnDelete(DeleteBehavior.Restrict);

            entity.HasIndex(x => new
            {
                x.EmployeeId,
                x.LeaveTypeId,
                x.Year
            }).IsUnique();
        });

        // Notification
        modelBuilder.Entity<Notification>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Title)
                  .HasMaxLength(200)
                  .IsRequired();

            entity.Property(x => x.Message)
                  .HasMaxLength(1000)
                  .IsRequired();

            entity.Property(x => x.IsRead)
                  .HasDefaultValue(false);

            entity.Property(x => x.CreatedOn)
                  .IsRequired();
        });
    }
}