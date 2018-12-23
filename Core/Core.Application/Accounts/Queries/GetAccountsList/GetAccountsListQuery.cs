using Core.Application.Accounts.Models;
using Core.Common.BaseClasses;
using Core.Common.Configuration;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Accounts.Queries
{
    public class GetAccountsListQuery : IRequest<List<AccountViewModel>>
    {
       
    }
}
