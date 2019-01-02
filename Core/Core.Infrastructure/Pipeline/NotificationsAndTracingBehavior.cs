using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Core.Common.Response;
using Core.Infrastructure.Notifications.PingPong.Publisher;
using MediatR;
using Serilog;

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
             * ^ PUBLISHING STRATEGY ^
             * ------------------------------------------------------
             * The default implementation of Publish, loops trough the notification handlers and awaits each one.
             * This makes sure each handler is run after one another, while each call to 'Handle' is run
             * asynchronously. Depending on your use-case for publishing notifications, you might need a
             * different strategy for handling the notifications. Maybe you want to publish all notifications
             * in parallel, or wrap each notification handler with your own exception handling logic.
             *--------------------------------------------------/




            /* ------------------------------------------------------
             * ADDITIONAL NOTES
             * ------------------------------------------------------*/
             // It is bad practice to place too much (if any) of the examples below within the pipeline.
             // It would likely be cleaner to include this logging within the commands themselves.
             // However it is important to point out the type of control you have within the MediatR pipeline
             //-----------------------------------------------------------
             
            // You can inject pipeline functionality on specific result status...
            if (typeof(TResponse).Name == "CommandResponse")
            {
                if(!(response as CommandResponse).isSuccess)
                {
                    Log.Warning(String.Concat(typeof(TRequest).Name, " attempted execution with issues: " + (response as CommandResponse).Message));

                    // ...Or send a notification...
                    //var commandFailure = new Ping { Type = typeof(TRequest).Name };
                    //await _mediatr.Publish(commandFailure);
                }
            }


            // ...As well as on specific command types with a specific result scenario:
            if (typeof(TRequest).Name == "CreateAccountCommand")
            {
                if (!(response as CommandResponse).isSuccess && (response as CommandResponse).ValidationErrors != null)
                {
                    var errors = new StringBuilder();
                    var last = (response as CommandResponse).ValidationErrors.Last();
                    foreach (var error in (response as CommandResponse).ValidationErrors)
                    {
                        errors.Append(error.ErrorMessage);
                        if(error != last)
                        {
                            errors.Append(", ");
                        }   
                    }

                    Log.Warning(String.Concat(typeof(TRequest).Name, " executed with the following validation issues: " + errors));
                }
            }

            

            return response;
        }
    }
}
