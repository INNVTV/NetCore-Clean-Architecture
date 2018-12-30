using Core.Infrastructure.Notifications.PingPong.Publisher;
using MediatR;
using Serilog;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Infrastructure.Notifications.PingPong.Subscribers
{
    public class Pong1 : INotificationHandler<Ping>
    {
        public Task Handle(Ping notification, CancellationToken cancellationToken)
        {
            // Sends trace statements to diagnostics/output window in Visual Studio during debugging
            Trace.WriteLine(String.Concat("Pong 1: ", notification.Message));

            // Log via Serilog:
            Log.Information(String.Concat("Pong 1 Notification Called: ", notification.Message));

            return Task.CompletedTask;
        }
    }
}
