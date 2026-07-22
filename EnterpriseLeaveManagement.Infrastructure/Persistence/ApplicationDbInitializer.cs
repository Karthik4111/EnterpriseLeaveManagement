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

        await SeedDepartmentsAsync(context);
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

    private static async Task SeedDepartmentsAsync(ApplicationDbContext context)
    {
        if (await context.Departments.AnyAsync())
            return;

        context.Departments.Add(new Domain.Entities.Department
        {
            Id = Guid.NewGuid(),
            Name = "Information Technology",
            Code = "IT",
            Description = "IT Department",
            IsActive = true
        });

        await context.SaveChangesAsync();
    }
}