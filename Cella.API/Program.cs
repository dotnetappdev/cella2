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

            app.MapOpenApi();


            app.MapIdentityApi<ApplicationUser>();
            app.AddAuthorizationMiddleware();
            app.AddSwaggerUI();

            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();




            app.Run();
        }
    }
}
