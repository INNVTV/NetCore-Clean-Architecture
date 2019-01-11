using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Infrastructure.Middleware.ApiKeyAuthentication
{
    //-----------------------------------------------------
    // The following extension method exposes the middleware through IApplicationBuilder:
    // giving us the ability to use app.UseApiKeyAuthenticationMiddleware() inside of: Startup.Configure(IApplicationBuilder app)
    //---------------------------------------------------------

    // Without this extension method you would just call: app.UseMiddleware(typeof(ApiKeyAuthenticationMiddleware)); 

    public static class ApiKeyAuthenticationMiddlewareExtension
    {
        public static IApplicationBuilder UseApiKeyAuthenticationMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ApiKeyAuthenticationMiddleware>();
        }
    }
}
