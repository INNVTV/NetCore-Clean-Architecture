using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Infrastructure.Notifications.PingPong.Publisher
{
    public class Ping : INotification {
        public string Message { get; set; }
    }
}
