using Core.Application.Account.Models;
using Core.Common.BaseClasses;
using Core.Common.Configuration;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Account.Queries.GetAccountsList
{
    public class GetAccountsListQuery : QueryHandlerBase, IRequest<List<AccountViewModel>>
    {
       
    }
}
