using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Cella.Blazor.Components;
using Cella.Blazor.Components.Account;
using Cella.Infrastructure;
using Cella.Models;
using Cella.Domain;


using Microsoft.AspNetCore.Components.Web;
using ElectronNET.API;
using ElectronNET.API.Entities;
using App = Cella.Blazor.Components.App;
using Syncfusion.Blazor;

namespace Cella.Blazor;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);


        var configuration = builder.Configuration;
        // Add services to the container.
        builder.Services.AddSyncfusionBlazor();
        builder.Services.AddRazorComponents()

            .AddInteractiveServerComponents();

        builder.Services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
        builder.Services.AddElectron();

        // Ensure the Syncfusion.Blazor NuGet package is installed in your project.
        // You can install it using the following command in the NuGet Package Manager Console:
        // Install-Package Syncfusion.Blazor
        builder.Services.AddCascadingAuthenticationState();
        builder.Services.AddScoped<IdentityUserAccessor>();
        builder.Services.AddScoped<IdentityRedirectManager>();
        builder.Services.AddScoped<AuthenticationStateProvider, IdentityRevalidatingAuthenticationStateProvider>();
        builder.Services.Configure<AppSettings>(configuration.GetSection("AppSettings"));

        var appSettings = configuration.GetSection("AppSettings").Get<AppSettings>();
        builder.Services.AddControllerServices(appSettings);
        builder.Services.AddAuthentication(options =>
            {
                options.DefaultScheme = IdentityConstants.ApplicationScheme;
                options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
            })
            .AddIdentityCookies();

        var connectionString = appSettings.GetDefaultConnection() ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
        builder.Services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(connectionString));
        builder.Services.AddDatabaseDeveloperPageExceptionFilter();

        builder.Services.AddIdentityCore<ApplicationUser>(options =>
            options.SignIn.RequireConfirmedAccount = false)
            .AddRoles<ApplicationRole>() // Add Role support
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        builder.Services.AddSingleton<IEmailSender<ApplicationUser>, IdentityNoOpEmailSender>();
        builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(appSettings.BaseAddress) });

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
        app.MapRazorComponents<App>().AddInteractiveServerRenderMode();
        //.AddAdditionalAssemblies(typeof(Cella.Components.Pages.stock.Index).Assembly);


        //using (var scope = app.Services.CreateScope())
        //{
        //    var services = scope.ServiceProvider;
        //    await SeedData.SeedUsersAndRoles(services);
        //}


        // Open the Electron-Window here

        app.Run();
    }
}
