using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace CellaWeb.Blazor
{
    public static class MiddlewareRegistrationExtensions
    {
        // Extension method to add CORS configuration
        public static void AddCorsPolicies(this IApplicationBuilder app)
        {

        }

        // Extension method to add Swagger UI
        public static void AddSwaggerUI(this IApplicationBuilder app)
        {


            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = "api"; // Set the Swagger UI to be served at /api
            });
        }

        public static void AddAuthorizationMiddleware(this IApplicationBuilder app)
        {
            app.UseAuthorization();
            //      app.MapGroup("/admin").MapIdentityApi<ApplicationUser>();

        }

    }
}

