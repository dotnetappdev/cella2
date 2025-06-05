using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

using Cella.Infrastructure;
using Cella.Models;



using Microsoft.AspNetCore.Components.Web;


using App = CellaWeb.Blazor.Components.App;
using CellaWeb.Blazor.Components.Account;

namespace CellaWeb.Blazor

{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            // Add services to the container.
            builder.Services.AddRazorComponents().AddInteractiveServerComponents();
            var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
            builder.Services.AddControllerServices(appSettings);

            builder.Services.AddCascadingAuthenticationState();
            builder.Services.AddScoped<IdentityUserAccessor>();
            builder.Services.AddScoped<IdentityRedirectManager>();
            builder.Services.AddHttpClient("Default", client =>
            {
                client.BaseAddress = new Uri(appSettings.BaseAddress);
            });
            builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Default"));
            builder.Services.AddAuthentication(options =>
                {
                    options.DefaultScheme = IdentityConstants.ApplicationScheme;
                    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
                });

            builder.Services.AddAuthorization();

            var connectionString = appSettings.GetDefaultConnection() ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddApplicationServices(configuration);

            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            //builder.Services.AddIdentityCore<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = true)
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddSignInManager()
            //    .AddDefaultTokenProviders();



            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseAntiforgery();

            app.MapStaticAssets();
            app.MapRazorComponents<App>().AddInteractiveServerRenderMode(); // Enables server render mode globally


            // Add additional endpoints required by the Identity /Account Razor components.
            app.MapAdditionalIdentityEndpoints();

            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                await SeedData.SeedUsersAndRoles(services); // Ensure SeedData is properly referenced
            }
            app.Run();
        }
    }
}
