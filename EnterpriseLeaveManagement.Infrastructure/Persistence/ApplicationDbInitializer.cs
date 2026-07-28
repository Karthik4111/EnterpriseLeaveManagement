using EnterpriseLeaveManagement.Domain.Entities;
using EnterpriseLeaveManagement.Domain.Enums;
using EnterpriseLeaveManagement.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace EnterpriseLeaveManagement.Infrastructure.Persistence;

public static class ApplicationDbInitializer
{
    public static async Task InitialiseAsync(IServiceProvider services)
    {
        using var scope = services.CreateScope();

        var context = scope.ServiceProvider
            .GetRequiredService<ApplicationDbContext>();

        await context.Database.MigrateAsync();

        var roleManager = scope.ServiceProvider
            .GetRequiredService<RoleManager<ApplicationRole>>();

        var userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<ApplicationUser>>();

        await SeedRolesAsync(roleManager);
        await SeedUsersAsync(userManager);
        var deptIds = await SeedDepartmentsAsync(context);
        var leaveTypeIds = await SeedLeaveTypesAsync(context);
        await SeedEmployeesAsync(context, userManager, deptIds);
        await SeedLeaveAllocationsAsync(context, leaveTypeIds);
        await SeedLeaveRequestsAsync(context, leaveTypeIds);
    }

    private static async Task SeedRolesAsync(
        RoleManager<ApplicationRole> roleManager)
    {
        var roles = new[]
        {
            "Admin",
            "Manager",
            "Employee"
        };

        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role))
            {
                await roleManager.CreateAsync(new ApplicationRole
                {
                    Name = role
                });
            }
        }
    }

    private static async Task SeedUsersAsync(
        UserManager<ApplicationUser> userManager)
    {
        // =========================
        // Admin User
        // =========================

        var users = userManager.Users.ToList();

        Console.WriteLine($"Total Users: {users.Count}");

        foreach (var user in users)
        {
            Console.WriteLine($"{user.Email} | {user.Id}");
        }

        var admin = await userManager.FindByEmailAsync("admin@company.com");

        if (admin == null)
        {
            admin = new ApplicationUser
            {
                UserName = "admin",
                Email = "admin@company.com",
                FirstName = "System",
                LastName = "Admin",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(
                admin,
                "Admin@123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(admin, "Admin");
            }
        }

        // =========================
        // Manager User
        // =========================

        var manager = await userManager.FindByEmailAsync("manager@company.com");

        if (manager == null)
        {
            manager = new ApplicationUser
            {
                UserName = "manager",
                Email = "manager@company.com",
                FirstName = "John",
                LastName = "Manager",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(
                manager,
                "Manager@123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(manager, "Manager");
            }
        }

        // =========================
        // Employee User
        // =========================

        var employee = await userManager.FindByEmailAsync("employee@company.com");

        if (employee == null)
        {
            employee = new ApplicationUser
            {
                UserName = "employee",
                Email = "employee@company.com",
                FirstName = "Default",
                LastName = "Employee",
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(
                employee,
                "Employee@123");

            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(employee, "Employee");
            }
        }
    }

    private static async Task<Dictionary<string, Guid>> SeedDepartmentsAsync(ApplicationDbContext context)
    {
        var deptIds = new Dictionary<string, Guid>();

        var existing = await context.Departments.ToListAsync();
        foreach (var d in existing) deptIds[d.Code] = d.Id;

        var toAdd = new[]
        {
            new Department { Id = Guid.NewGuid(), Name = "Information Technology",  Code = "IT",  Description = "Software, infrastructure and IT support",           IsActive = true },
            new Department { Id = Guid.NewGuid(), Name = "Human Resources",         Code = "HR",  Description = "Recruitment, payroll and employee relations",        IsActive = true },
            new Department { Id = Guid.NewGuid(), Name = "Finance & Accounting",    Code = "FIN", Description = "Financial reporting, budgets and accounts",          IsActive = true },
            new Department { Id = Guid.NewGuid(), Name = "Sales & Marketing",       Code = "SAL", Description = "Customer acquisition and brand management",          IsActive = true },
            new Department { Id = Guid.NewGuid(), Name = "Operations",              Code = "OPS", Description = "Day-to-day business operations and logistics",       IsActive = true },
        };

        foreach (var dept in toAdd)
        {
            if (!deptIds.ContainsKey(dept.Code))
            {
                context.Departments.Add(dept);
                deptIds[dept.Code] = dept.Id;
            }
        }

        await context.SaveChangesAsync();
        return deptIds;
    }

    private static async Task<Dictionary<string, Guid>> SeedLeaveTypesAsync(ApplicationDbContext context)
    {
        var ids = new Dictionary<string, Guid>();

        var existing = await context.LeaveTypes.ToListAsync();
        foreach (var lt in existing) ids[lt.Code] = lt.Id;

        var toAdd = new[]
        {
            new LeaveType { Id = Guid.NewGuid(), Name = "Annual Leave",       Code = "AL",  Description = "Paid annual vacation leave",              DefaultDays = 20, IsPaidLeave = true,  CarryForwardAllowed = true,  MaximumCarryForwardDays = 5,  RequiresApproval = true,  IsActive = true },
            new LeaveType { Id = Guid.NewGuid(), Name = "Sick Leave",         Code = "SL",  Description = "Leave due to illness or medical reasons",  DefaultDays = 10, IsPaidLeave = true,  CarryForwardAllowed = false, MaximumCarryForwardDays = 0,  RequiresApproval = false, IsActive = true },
            new LeaveType { Id = Guid.NewGuid(), Name = "Casual Leave",       Code = "CL",  Description = "Short personal leave",                     DefaultDays = 7,  IsPaidLeave = true,  CarryForwardAllowed = false, MaximumCarryForwardDays = 0,  RequiresApproval = true,  IsActive = true },
            new LeaveType { Id = Guid.NewGuid(), Name = "Maternity Leave",    Code = "ML",  Description = "Leave for new mothers",                    DefaultDays = 90, IsPaidLeave = true,  CarryForwardAllowed = false, MaximumCarryForwardDays = 0,  RequiresApproval = true,  IsActive = true },
            new LeaveType { Id = Guid.NewGuid(), Name = "Unpaid Leave",       Code = "UL",  Description = "Leave without pay",                        DefaultDays = 30, IsPaidLeave = false, CarryForwardAllowed = false, MaximumCarryForwardDays = 0,  RequiresApproval = true,  IsActive = true },
        };

        foreach (var lt in toAdd)
        {
            if (!ids.ContainsKey(lt.Code))
            {
                context.LeaveTypes.Add(lt);
                ids[lt.Code] = lt.Id;
            }
        }

        await context.SaveChangesAsync();
        return ids;
    }

    private static async Task SeedEmployeesAsync(
        ApplicationDbContext context,
        UserManager<ApplicationUser> userManager,
        Dictionary<string, Guid> deptIds)
    {
        if (await context.Employees.AnyAsync()) return;

        // Resolve existing identity users
        var mgr  = await userManager.FindByEmailAsync("manager@company.com");
        var emp  = await userManager.FindByEmailAsync("employee@company.com");
        var admin= await userManager.FindByEmailAsync("admin@company.com");

        var itId  = deptIds.GetValueOrDefault("IT");
        var hrId  = deptIds.GetValueOrDefault("HR");
        var finId = deptIds.GetValueOrDefault("FIN");
        var salId = deptIds.GetValueOrDefault("SAL");
        var opsId = deptIds.GetValueOrDefault("OPS");

        // Seed additional identity users for employees
        var seedUsers = new[]
        {
            ("alice.johnson", "alice@company.com", "Alice", "Johnson", "Employee@123", "Employee"),
            ("bob.smith",     "bob@company.com",   "Bob",   "Smith",   "Employee@123", "Employee"),
            ("carol.white",   "carol@company.com", "Carol", "White",   "Employee@123", "Employee"),
            ("david.kumar",   "david@company.com", "David", "Kumar",   "Employee@123", "Employee"),
        };

        var userIds = new Dictionary<string, Guid>();

        if (admin != null) userIds["admin@company.com"]    = admin.Id;
        if (mgr  != null) userIds["manager@company.com"]   = mgr.Id;
        if (emp  != null) userIds["employee@company.com"]  = emp.Id;

        foreach (var (username, email, first, last, pwd, role) in seedUsers)
        {
            var existing = await userManager.FindByEmailAsync(email);
            if (existing == null)
            {
                var u = new ApplicationUser
                {
                    UserName = username, Email = email,
                    FirstName = first, LastName = last,
                    EmailConfirmed = true
                };
                var r = await userManager.CreateAsync(u, pwd);
                if (r.Succeeded)
                {
                    await userManager.AddToRoleAsync(u, role);
                    userIds[email] = u.Id;
                }
            }
            else
            {
                userIds[email] = existing.Id;
            }
        }

        var today = DateTime.Today;
        var employees = new List<Employee>();

        void AddEmp(string code, string first, string last, string email, string phone, string designation, Guid deptId, DateTime join, Guid? managerId = null)
        {
            if (!userIds.TryGetValue(email, out var uid)) return;
            employees.Add(new Employee
            {
                Id           = Guid.NewGuid(),
                EmployeeCode = code,
                FirstName    = first,
                LastName     = last,
                Email        = email,
                PhoneNumber  = phone,
                Designation  = designation,
                DepartmentId = deptId,
                ManagerId    = managerId,
                UserId       = uid,
                DateOfJoining = join,
                IsActive     = true,
                CreatedOn    = today,
            });
        }

        // Manager user as an employee record
        if (mgr != null)
            AddEmp("EMP001", mgr.FirstName, mgr.LastName, "manager@company.com",
                   "+1-555-0100", "Engineering Manager", itId, today.AddYears(-3));

        AddEmp("EMP002", "Alice",  "Johnson", "alice@company.com",  "+1-555-0101", "Software Engineer",     itId,  today.AddYears(-2));
        AddEmp("EMP003", "Bob",    "Smith",   "bob@company.com",    "+1-555-0102", "QA Engineer",           itId,  today.AddYears(-1));
        AddEmp("EMP004", "Carol",  "White",   "carol@company.com",  "+1-555-0103", "HR Manager",            hrId,  today.AddYears(-3));
        AddEmp("EMP005", "David",  "Kumar",   "david@company.com",  "+1-555-0104", "Financial Analyst",     finId, today.AddYears(-2));

        // Seeded "employee@company.com" default user
        if (emp != null)
            AddEmp("EMP006", emp.FirstName, emp.LastName, "employee@company.com",
                   "+1-555-0105", "Junior Developer", itId, today.AddMonths(-6));

        context.Employees.AddRange(employees);
        await context.SaveChangesAsync();
    }

    private static async Task SeedLeaveAllocationsAsync(
        ApplicationDbContext context,
        Dictionary<string, Guid> leaveTypeIds)
    {
        if (await context.LeaveAllocations.AnyAsync()) return;

        var employees = await context.Employees.ToListAsync();
        var year = DateTime.Today.Year;
        var allocations = new List<LeaveAllocation>();

        foreach (var emp in employees)
        {
            foreach (var (code, days) in new[] { ("AL", 20), ("SL", 10), ("CL", 7) })
            {
                if (!leaveTypeIds.TryGetValue(code, out var ltId)) continue;
                allocations.Add(new LeaveAllocation
                {
                    Id           = Guid.NewGuid(),
                    EmployeeId   = emp.Id,
                    LeaveTypeId  = ltId,
                    Year         = year,
                    AllocatedDays = days,
                    CreatedOn    = DateTime.Today,
                });
            }
        }

        context.LeaveAllocations.AddRange(allocations);
        await context.SaveChangesAsync();
    }

    private static async Task SeedLeaveRequestsAsync(
        ApplicationDbContext context,
        Dictionary<string, Guid> leaveTypeIds)
    {
        if (await context.LeaveRequests.AnyAsync()) return;

        var employees = await context.Employees.Take(4).ToListAsync();
        if (!employees.Any()) return;

        leaveTypeIds.TryGetValue("AL", out var alId);
        leaveTypeIds.TryGetValue("SL", out var slId);
        leaveTypeIds.TryGetValue("CL", out var clId);

        var today = DateOnly.FromDateTime(DateTime.Today);
        var requests = new List<LeaveRequest>();

        if (employees.Count > 0 && alId != Guid.Empty)
        {
            requests.Add(new LeaveRequest
            {
                Id           = Guid.NewGuid(),
                EmployeeId   = employees[0].Id,
                LeaveTypeId  = alId,
                StartDate    = today.AddDays(5),
                EndDate      = today.AddDays(9),
                NumberOfDays = 5,
                LeaveReason  = "Family vacation",
                Status       = LeaveRequestStatus.Pending,
                CreatedOn    = DateTime.Now,
            });
        }

        if (employees.Count > 1 && slId != Guid.Empty)
        {
            requests.Add(new LeaveRequest
            {
                Id           = Guid.NewGuid(),
                EmployeeId   = employees[1].Id,
                LeaveTypeId  = slId,
                StartDate    = today.AddDays(-3),
                EndDate      = today.AddDays(-1),
                NumberOfDays = 3,
                LeaveReason  = "Fever and flu",
                Status       = LeaveRequestStatus.Approved,
                CreatedOn    = DateTime.Now.AddDays(-5),
            });
        }

        if (employees.Count > 2 && clId != Guid.Empty)
        {
            requests.Add(new LeaveRequest
            {
                Id           = Guid.NewGuid(),
                EmployeeId   = employees[2].Id,
                LeaveTypeId  = clId,
                StartDate    = today.AddDays(1),
                EndDate      = today.AddDays(2),
                NumberOfDays = 2,
                LeaveReason  = "Personal work",
                Status       = LeaveRequestStatus.Pending,
                CreatedOn    = DateTime.Now,
            });
        }

        if (employees.Count > 3 && alId != Guid.Empty)
        {
            requests.Add(new LeaveRequest
            {
                Id           = Guid.NewGuid(),
                EmployeeId   = employees[3].Id,
                LeaveTypeId  = alId,
                StartDate    = today.AddDays(-10),
                EndDate      = today.AddDays(-6),
                NumberOfDays = 5,
                LeaveReason  = "Annual leave",
                Status       = LeaveRequestStatus.Rejected,
                CreatedOn    = DateTime.Now.AddDays(-14),
            });
        }

        if (!requests.Any()) return;

        try
        {
            context.LeaveRequests.AddRange(requests);
            await context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            // Column may not exist if schema is ahead of migration snapshot.
            // Rest of seed data is already committed — skip leave requests gracefully.
            Console.WriteLine($"[Seed] Skipped LeaveRequests: {ex.Message}");
        }
    }
}