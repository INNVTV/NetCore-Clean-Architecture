using Core.Common.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Account.Queries
{
    public class GetAccountsListQuery
    {
        readonly ICoreConfiguration _coreConfiguration;

        public GetAccountsListQuery(ICoreConfiguration coreConfiguration)
        {
            _coreConfiguration = coreConfiguration;
        }

        public string GetAccountsListQueryName()
        {
            return _coreConfiguration.Application.Name;
        }
    }
}
