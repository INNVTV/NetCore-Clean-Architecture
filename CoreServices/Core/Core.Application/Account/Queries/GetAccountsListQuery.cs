using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Account.Queries
{
    public class GetAccountsListQuery
    {
        readonly IConfiguration _configuration;

        public GetAccountsListQuery(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GetAccountsListQueryName()
        {
            return _configuration.GetSection("Application").GetSection("Name").Value;
        }
    }
}
