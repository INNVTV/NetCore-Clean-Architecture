using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Infrastructure.Services.Email
{
    public interface IEmailMessage
    {
        string ToEmail { get; set; }
        string ToName { get; set; }

        string FromEmail { get; set; }
        string FromName { get; set; }

        string Subject { get; set; }

        string TextContent { get; set; }
        string HtmlContent { get; set; }
    }
}
