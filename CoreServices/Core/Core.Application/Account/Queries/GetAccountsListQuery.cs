using Core.Common.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Account.Queries
{
    public class GetAccountsListQuery
    {
        readonly IServiceProvider _serviceProvider;

        public GetAccountsListQuery(ServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public string GetAccountsListQueryName()
        {
            var coreConfiguration = _serviceProvider.GetService<ICoreConfiguration>();

            return coreConfiguration.Application.Name;
        }
    }
}
