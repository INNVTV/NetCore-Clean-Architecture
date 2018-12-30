using MediatR;
using MediatR.Pipeline;
using Microsoft.Extensions.Logging;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Infrastructure.Pipeline
{
    // Unlike Performance and Tracing behavior, this behavior will ONLY run as a pre-processor.
    // Please note the differences in the way the classes are written.

    public class LoggingBehavior<TRequest> : IRequestPreProcessor<TRequest>
    {
        public LoggingBehavior()
        {
        }

        public Task Process(TRequest request, CancellationToken cancellationToken)
        {
            var name = typeof(TRequest).Name;

            // TODO: Add User/Caller Details, or include in Command

            // Uses Serilog's global, statically accessible logger, is set via Log.Logger in the startup/entrypoint of the host solution/project.
            // Sinks, enrichers, and minimum logging level are set up in the entry point.
            // Dependency Injection is not required. We are using a global, statically accessible logger 
            Log.Information("Request: {Name} {@Request}", name, request);

            return Task.CompletedTask;
        }
    }
}
