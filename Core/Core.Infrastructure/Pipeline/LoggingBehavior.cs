using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Infrastructure.Pipeline
{
    // Unlike Performance and Tracing behavior, this behavior will ONLY run as a pre-processor.
    // Please note the differences in the way the classes are written.

    // Also note that you will need to assign a logging mechanism in your DI container.
    // Current implentation will only log to your local Output window during Visual Studio debugging.

    // Make sure you use .ConfigureLogging in your Program.CreateWebHostBuilder.CreateDefaultBuilder (for WebApps/APIs)
    // logging.AddConsole(); Will allow you to debug logs locally with Visual Studio

    // For Console Apps:

    public class LoggingBehavior<TRequest> : IRequestPreProcessor<TRequest>
    {
        private readonly ILogger _logger;

        public LoggingBehavior(ILogger<TRequest> logger)
        {
            _logger = logger;
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var name = typeof(TRequest).Name;

            // TODO: Add User Details

            _logger.LogInformation("Request: {Name} {@Request}", name, request);

            return Task.CompletedTask;
        }
    }
}
