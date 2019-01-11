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

namespace Core.Infrastructure.Middleware.ApiKeyAuthentication
{
    public class ApiKeyAuthenticationMiddleware
    {
        private const string APIKeyToCheck = "k1234567891011121314151617181920";
        private readonly RequestDelegate next;

        public ApiKeyAuthenticationMiddleware(RequestDelegate next)
        {

            this.next = next;
        }

        public async Task Invoke(HttpContext context /* other dependencies */)
        {
            bool validKey = false;
            var apiKeyExists = context.Request.Headers.ContainsKey("APIKey");
            if(apiKeyExists)
            {
                if(context.Request.Headers["APIKey"].Equals(APIKeyToCheck))
                {
                    validKey = true;
                }
            }
            else
            {
                context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                await context.Response.WriteAsync("Please add an APIKey to you request header");
            }

            if(!validKey)
            {
                context.Response.StatusCode = (int)HttpStatusCode.Forbidden;
                await context.Response.WriteAsync("Invalid API Key");
            }
            else
            {
                await next.Invoke(context);
            }
        }
    }
}
