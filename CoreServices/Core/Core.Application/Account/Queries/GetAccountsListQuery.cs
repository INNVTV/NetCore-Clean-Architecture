using Core.Common.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Account.Queries
{
    public class GetAccountsListQuery
    {
        readonly ICoreConfiguration coreConfiguration;

        public GetAccountsListQuery(IServiceProvider serviceProvider)
        {
            coreConfiguration = serviceProvider.GetService<ICoreConfiguration>();
        }

        public string GetAccountsListQueryName()
        {
           return coreConfiguration.Application.Name;
        }
    }
}
