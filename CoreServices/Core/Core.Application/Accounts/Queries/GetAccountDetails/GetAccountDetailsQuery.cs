using Core.Application.Accounts.Models;
using Core.Common.BaseClasses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Accounts.Queries.GetAccountDetails
{
    public class GetAccountDetailsQuery : QueryHandlerBase, IRequest<AccountViewModel>
    {
        public string Id { get; set; }
    }
}
