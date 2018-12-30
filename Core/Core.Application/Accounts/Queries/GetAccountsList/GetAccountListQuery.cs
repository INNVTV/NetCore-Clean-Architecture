using Core.Application.Accounts.Models;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Accounts.Queries
{
    public class GetAccountListQuery : IRequest<List<AccountViewModel>>
    {
       
    }
}
