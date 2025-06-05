using Cella.Infrastructure;
using Cella.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public static class SeedData
{
    public static async Task SeedUsersAndRoles(IServiceProvider serviceProvider)
    {
        var dbContext = serviceProvider.GetRequiredService<ApplicationDbContext>();
        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // 0. Remove all users and roles
        // Remove users
        var allUsers = await dbContext.Users.ToListAsync();
        foreach (var user in allUsers)
        {
            await userManager.DeleteAsync(user);
        }

        // Remove roles
        var allRoles = await dbContext.Roles.ToListAsync();
        foreach (var role in allRoles)
        {
            await roleManager.DeleteAsync(role);
        }

        // Save changes to ensure clean state
        await dbContext.SaveChangesAsync();

        // 1. Define roles
        var roles = new List<ApplicationRole>
        {
            new ApplicationRole { Name = "Admin", Description = "Admin role" },
            new ApplicationRole { Name = "User", Description = "User role" },
            new ApplicationRole { Name = "Owner", Description = "Owner role" },
            new ApplicationRole { Name = "ReadOnly", Description = "ReadOnly role" },
            new ApplicationRole { Name = "Write", Description = "Write role" },
            new ApplicationRole { Name = "Agent", Description = "Agent role" }
        };

        // 2. Create roles
        foreach (var role in roles)
        {
            await roleManager.CreateAsync(role);
        }

        // 3. Define users and their roles
        var users = new List<(string Email, string Password, string[] Roles)>
        {
            ("admin@example.com", "Admin123!", new[] { "Admin", "Owner", "Write" }),
            ("user@example.com", "User123!", new[] { "User", "ReadOnly" }),
            ("manager@example.com", "Manager123!", new[] { "Admin", "Write" }),
            ("owner@example.com", "Owner123!", new[] { "Owner" }),
            ("readonly@example.com", "ReadOnly123!", new[] { "ReadOnly" }),
            ("contributor@example.com", "Contributor123!", new[] { "Write", "ReadOnly" }),
            ("agent@example.com", "Agent123!", new[] { "Agent" })
        };

        // 4. Create users and assign roles
        foreach (var (email, password, assignedRoles) in users)
        {
            var user = new ApplicationUser
            {
                UserName = email,
                Email = email,
                EmailConfirmed = true
            };

            var result = await userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                await userManager.AddToRolesAsync(user, assignedRoles);
                Console.WriteLine($"User '{email}' created and assigned to roles: {string.Join(", ", assignedRoles)}");
            }
            else
            {
                Console.WriteLine($"Failed to create user '{email}': {string.Join(", ", result.Errors.Select(e => e.Description))}");
            }
        }
    }
}