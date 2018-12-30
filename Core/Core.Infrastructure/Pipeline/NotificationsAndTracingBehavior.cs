using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Infrastructure.Notifications.PingPong.Publisher;
using MediatR;

namespace Core.Infrastructure.Pipeline
{
    public class NotificationsAndTracingBehavior<TRequest, TResponse> : IPipelineBehavior<TRequest, TResponse>
    {
        // Here we simply trace to our Output window for local Visual Studio debugging.

        // MediatR property and constructor for TracingBehavior are not mandatory in cases where you don't need a dependancy such as MediatR injected.
        // Here we need MediatR in order to send notifications after processing the handler.

        private readonly IMediator _mediatr;

        public NotificationsAndTracingBehavior(IMediator mediatr)
        {
            _mediatr = mediatr;
        }

        public async Task<TResponse> Handle(TRequest request, CancellationToken cancellationToken, RequestHandlerDelegate<TResponse> next)
        {
            /* ------------------------------------------------------
             * PRE/POST PIPELINE BEHAVIORS
             * ------------------------------------------------------*/
            // Sends trace statements to diagnostics/output window in Visual Studio during debugging

            Trace.WriteLine(String.Concat("Handling: PreProcessor for ", typeof(TRequest).Name));
            var response = await next(); //<-- Send request to the requested handler
            Trace.WriteLine(String.Concat("Handled: PostProcessor for ", typeof(TRequest).Name));

            /* ------------------------------------------------------
             * NOTIFICATIONS (Pub/Sub)
             * ------------------------------------------------------*/
            // Send test 'ping' notification after all handlers completed processing.

            var ping = new Ping { Message = "Ping..." };
            await _mediatr.Publish(ping);

            /* -----------------------------------------------------
             * PUBLISHING STRATEGY 
             * ------------------------------------------------------
             * The default implementation of Publish, loops trough the notification handlers and awaits each one.
             * This makes sure each handler is run after one another, while each call to 'Handle' is run
             * asynchronously. Depending on your use-case for publishing notifications, you might need a
             * different strategy for handling the notifications. Maybe you want to publish all notifications
             * in parallel, or wrap each notification handler with your own exception handling logic.
             */

            return response;
        }
    }
}
