using Cella.Infrastructure;
using Cella.Models;
using Microsoft.AspNetCore.Http;
using System.Configuration;
using System.Text;

namespace Cella.API
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            builder.Services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

            var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
            if (appSettings == null || appSettings.JwtSecret == null || appSettings.JwtSecret == null)
            {
                throw new Exception("JWT settings are not configured correctly.");
            }
            // Add services to the container.
            builder.Services.AddBusinessServices();
            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddControllerServices(appSettings);
            var testc = appSettings.ConnectionStrings;
            var testd = appSettings.GetDefaultConnection();
            builder.Services.AddApplicationServices(appSettings.GetDefaultConnection());
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await DataSeeder.SeedRolesAndUsers(services);
            }
            app.MapOpenApi();


            app.MapIdentityApi<ApplicationUser>();
            app.AddSwaggerUI();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();



            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await SeedData.SeedUsersAndRoles(services);
            }

            app.Run();
        }
    }
}
