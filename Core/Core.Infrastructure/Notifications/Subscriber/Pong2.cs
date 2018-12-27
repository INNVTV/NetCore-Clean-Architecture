using Core.Infrastructure.Notifications.Publisher;
using MediatR;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Infrastructure.Notifications.Subscriber
{
    public class Pong2 : INotificationHandler<Ping>
    {
        public Task Handle(Ping notification, CancellationToken cancellationToken)
        {
            // Sends trace statements to diagnostics/output window in Visual Studio during debugging
            Trace.WriteLine(String.Concat("Pong 2: ", notification.Message));
            return Task.CompletedTask;
        }
    }
}
