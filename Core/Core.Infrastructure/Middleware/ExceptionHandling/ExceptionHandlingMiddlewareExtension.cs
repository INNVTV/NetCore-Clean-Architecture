using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Infrastructure.Middleware.ExceptionHandling
{
    //-----------------------------------------------------
    // The following extension method exposes the middleware through IApplicationBuilder:
    // giving us the ability to use app.UseExceptionHandlingMiddleware() inside of: Startup.Configure(IApplicationBuilder app)
    //---------------------------------------------------------

    // Without this extension method you would just call: app.UseMiddleware(typeof(ExceptionHandlingMiddleware)); 

    public static class ExceptionHandlingMiddlewareExtension
    {
        public static IApplicationBuilder UseExceptionHandlingMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlingMiddleware>();
        }
    }
}
