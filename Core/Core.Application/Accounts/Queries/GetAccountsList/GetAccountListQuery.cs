using Core.Application.Accounts.Models;
using Core.Application.Accounts.Queries.Enums;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Accounts.Queries
{
    public class GetAccountListQuery : IRequest<AccountListViewModel>
    {
        public int Page { get; set; }
        public int PageSize { get; set; }
        public OrderBy OrderBy { get; set; }
        public OrderDirection OrderDirection { get; set; }
    }
}
