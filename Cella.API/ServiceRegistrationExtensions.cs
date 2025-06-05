using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Cella.Models;
using Cella.Infrastructure;
using Cella.Domain.Interfaces;
using System;
using System.Text;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Cella.Domain.Services;



namespace Cella.API
{
    public static class ServiceRegistrationExtensions
    {
        // Register DbContext and Identity with SQL Server
        public static void AddApplicationServices(this IServiceCollection services, string connectionstring)
        {
            // Register ApplicationDbContext with SQL Server
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionstring));

            services.AddIdentityApiEndpoints<ApplicationUser>()
           .AddRoles<IdentityRole>()
           .AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddEndpointsApiExplorer();

        }

        // Register custom application services (e.g., business logic services)
        public static void AddBusinessServices(this IServiceCollection services)
        {
            services.AddTransient<IStockInterface, StockService>();


        }

        // Register other services like controllers, health checks, etc.
        public static void AddControllerServices(this IServiceCollection services, AppSettings appSettings)
        {
            services.AddControllers();
            services.AddOpenApi();
            // Add Swagger with Bearer token support
            services.AddSwaggerGen(options =>
            {

                options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
                {
                    Title = "Cella API",
                    Version = "v1"
                });

                options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
                {
                    In = Microsoft.OpenApi.Models.ParameterLocation.Header,
                    Description = "Enter Bearer token in the format 'Bearer {token}'",
                    Name = "Authorization",
                    Type = Microsoft.OpenApi.Models.SecuritySchemeType.Http,
                    Scheme = "bearer"
                });

                options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
            });


            var jwtSettings = appSettings.ConnectionStrings.FirstOrDefault();
            var key = Encoding.ASCII.GetBytes(appSettings.JwtSecret);
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = appSettings.Issuer,
                    ValidAudience = appSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
            });
            //.AddGoogle(googleOptions =>
            //{
            //    googleOptions.ClientId = appSettings.GoogleAuth.ClientId;
            //    googleOptions.ClientSecret = appSettings.GoogleAuth.ClientSecret;
            //})
            //.AddMicrosoftAccount(microsoftOptions =>
            //{
            //    microsoftOptions.ClientId = appSettings.MicrosoftAuth.ClientId;
            //    microsoftOptions.ClientSecret = appSettings.MicrosoftAuth.ClientSecret;
            //});
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
