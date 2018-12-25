using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Core.Infrastructure.Services.Email
{
    public interface IEmailService
    {
        Settings Settings { get; set; }

        Task<bool> SendEmail(IEmailMessage emailMessage);
    }

    public class Settings
    {
        public string FromEmail { get; set; }
        public string FromName { get; set; }
        public string Key { get; set; }
    }

    


}
