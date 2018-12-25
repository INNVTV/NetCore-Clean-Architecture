using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Infrastructure.Services.Email
{
    public class EmailMessage : IEmailMessage
    {
        public string ToEmail { get; set; }
        public string ToName { get; set; }

        public string FromEmail { get; set; }
        public string FromName { get; set; }

        public string Subject { get; set; }

        public string TextContent { get; set; }
        public string HtmlContent { get; set; }
    }
}
