using Core.Common.Response;
using Core.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using Serilog;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Middleware.ExceptionHandling
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            try
            {
                await next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {

            if (exception.GetType() == typeof(FluentValidation.ValidationException))
            {
                Log.Information("Validation exception caught {@exception}", exception);

                var baseResponse = new BaseResponse((((FluentValidation.ValidationException)exception).Errors));

                var code = HttpStatusCode.BadRequest;
                var result = JsonConvert.SerializeObject(baseResponse);

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)code;
                return context.Response.WriteAsync(result);

            }
            else
            {
                //Track the user that ran into the exception (for our structured logs)
                var user = new User { Id = Guid.NewGuid(), Name = "John Smith" };

                // Log our exception using Serilog.
                // Use structured logging to capture the full exception object.
                Log.Error("Exception caught {@user} {@exception}", user, exception);

                var code = HttpStatusCode.InternalServerError;
                //var result = JsonConvert.SerializeObject(exception, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore } );
                var result = JsonConvert.SerializeObject( new { isSuccess = false, exceptionType = exception.GetType().ToString(), message = exception.Message });

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = (int)code;
                return context.Response.WriteAsync(result);
            }

        }
    }
}
