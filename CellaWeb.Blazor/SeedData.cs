using Cella.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public static class SeedData
{
    public static async Task SeedUsersAndRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

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

        // 2. Create roles if they don't exist
        foreach (var role in roles)
        {
            if (!await roleManager.RoleExistsAsync(role.Name))
            {
                await roleManager.CreateAsync(role);
            }
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
            if (await userManager.FindByEmailAsync(email) == null)
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
}