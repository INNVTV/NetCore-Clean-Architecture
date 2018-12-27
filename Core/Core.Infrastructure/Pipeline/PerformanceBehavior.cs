using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Infrastructure.Pipeline
{
    public class PerformanceBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        // We run a stopwatch on every request and log a warning for any requests that exceed our threshold.

        // Note: You will need to assign a logging mechanism in your DI container.
        // Current implentation will only log to your local Output window during Visual Studio debugging.

        private readonly Stopwatch _timer;
        private readonly ILogger _logger;

        public PerformanceBehavior(ILogger logger)
        {
            _timer = new Stopwatch();
            _logger = logger; //<-- using Microsoft.Extensions.Logging
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            _timer.Start();

            var response = await next();

            _timer.Stop();

            if (_timer.ElapsedMilliseconds > 1)
            {
                var name = typeof(TRequest).Name;

                // TODO: Add User Details

                _logger.LogWarning("Long Running Request: {Name} ({ElapsedMilliseconds} milliseconds) {@Request}", name, _timer.ElapsedMilliseconds, request);
            }

            return response;
        }
    }
}
