using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;

namespace Core.Infrastructure.Pipeline
{
    public class TracingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            Trace.WriteLine(String.Concat("Handling: PreProcessor for ", typeof(TRequest).Name));
            var response = await next();
            Trace.WriteLine(String.Concat("Handled: PostProcessor for ", typeof(TRequest).Name));
            return response;
        }
    }
}
