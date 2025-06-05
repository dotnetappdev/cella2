using Cella.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading.Tasks;

public static class SeedData
{
    public static async Task SeedUsersAndRoles(IServiceProvider serviceProvider)
    {
        var roleManager = serviceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
        var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

        // ✅ Create roles directly using RoleManager
        var roles = new[] { "Admin", "Readonly", "Write" };
        foreach (var role in roles)
        {
            var roleExists = await roleManager.RoleExistsAsync(role);
            if (!roleExists)
            {
                var newRole = new ApplicationRole
                {
                    Name = role,
                    Description = $"{role} role"
                };
                await roleManager.CreateAsync(newRole);
            }
        }

        // ✅ Create admin user if it doesn't exist
        if (await userManager.FindByEmailAsync("admin@example.com") == null)
        {
            var user = new ApplicationUser
            {
                UserName = "admin@example.com",
                Email = "admin@example.com",
                FullName = "Admin User"
            };

            // ✅ Create the user with a hashed password
            var result = await userManager.CreateAsync(user, "Admin@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Admin");
            }
            else
            {
                // Log any errors if user creation fails
                Console.WriteLine($"Error creating admin: {string.Join(", ", result.Errors)}");
            }
        }

        // ✅ Create readonly user if it doesn't exist
        if (await userManager.FindByEmailAsync("readonly@example.com") == null)
        {
            var user = new ApplicationUser
            {
                UserName = "readonly@example.com",
                Email = "readonly@example.com",
                FullName = "Read Only User"
            };

            // ✅ Create the user with a hashed password
            var result = await userManager.CreateAsync(user, "Readonly@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Readonly");
            }
            else
            {
                // Log any errors if user creation fails
                Console.WriteLine($"Error creating readonly user: {string.Join(", ", result.Errors)}");
            }
        }

        // ✅ Create write user if it doesn't exist
        if (await userManager.FindByEmailAsync("write@example.com") == null)
        {
            var user = new ApplicationUser
            {
                UserName = "write@example.com",
                Email = "write@example.com",
                FullName = "Write User"
            };

            // ✅ Create the user with a hashed password
            var result = await userManager.CreateAsync(user, "Write@123");
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(user, "Write");
            }
            else
            {
                // Log any errors if user creation fails
                Console.WriteLine($"Error creating write user: {string.Join(", ", result.Errors)}");
            }
        }
    }
}
