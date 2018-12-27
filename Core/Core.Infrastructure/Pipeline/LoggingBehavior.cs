using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Infrastructure.Pipeline
{
    public class LoggingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            //Trace.WriteLine("Pre-Pipeline");
            var response = await next();
            //Trace.WriteLine("Post-Pipeline");
            return response;
        }
    }
}
