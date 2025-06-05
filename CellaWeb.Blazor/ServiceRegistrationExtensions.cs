using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Cella.Models;
using Cella.Infrastructure;
using System;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.Cookies;



namespace CellaWeb.Blazor
{
    public static class ServiceRegistrationExtensions
    {
        // Register DbContext and Identity with SQL Server
        public static void AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
        {
            var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();

            var connectionString = appSettings.GetDefaultConnection() ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            // Register ApplicationDbContext with SQL Server
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));

            services.AddIdentityApiEndpoints<ApplicationUser>()
           .AddRoles<ApplicationRole>()
           .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddEndpointsApiExplorer();

        }

        // Register custom application services (e.g., business logic services)
        public static void AddBusinessServices(this IServiceCollection services)
        {


        }

        // Register other services like controllers, health checks, etc.
        public static void AddControllerServices(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddControllers();
            services.AddOpenApi();

            services.AddSwaggerGen(); // Swagger registration (optional)

            var jwtSettings = appSettings.JwtSecret;
            var key = Encoding.ASCII.GetBytes(appSettings.JwtSecret);
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
              .AddCookie(options =>
             {
                 options.LoginPath = "/Account/Login";  // Path to login page
                 options.LogoutPath = "/Account/Logout";  // Path to logout
                 options.ExpireTimeSpan = TimeSpan.FromMinutes(60);  // Expiry time for cookie
                 options.SlidingExpiration = true;  // Cookie will expire after the specified time of inactivity
             });



        }

        // Register application-specific middlewares (optional)
        public static void AddApplicationMiddlewares(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://example.com")
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });
        }
    }
}
