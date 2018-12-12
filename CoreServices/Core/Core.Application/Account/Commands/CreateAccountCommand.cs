using Core.Common.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Core.Common.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Account.Commands
{
    public class CreateAccountCommand
    {
        readonly ICoreConfiguration coreConfiguration;
        readonly ICoreLogger coreLogger;

        public CreateAccountCommand(IServiceProvider serviceProvider)
        {
            coreConfiguration = serviceProvider.GetService<ICoreConfiguration>();
            coreLogger = serviceProvider.GetService<ICoreLogger>();
        }
    }
}
