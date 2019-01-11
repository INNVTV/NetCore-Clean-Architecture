using Microsoft.AspNetCore.Builder;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Infrastructure.Middleware.ExceptionHandling
{
    //-----------------------------------------------------
    // The following extension method exposes the middleware through IApplicationBuilder:
    // giving us the ability to use app.UseExceptionHandlerMiddleware() inside of: Startup.Configure(IApplicationBuilder app)
    //---------------------------------------------------------

    // Without this extension method you would just call: app.UseMiddleware(typeof(ExceptionHandlerMiddleware)); 

    public static class ExceptionHandlerMiddlewareExtension
    {
        public static IApplicationBuilder UseExceptionHandlerMiddleware(
            this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
