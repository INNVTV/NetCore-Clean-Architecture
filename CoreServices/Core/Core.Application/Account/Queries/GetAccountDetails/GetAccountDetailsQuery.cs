using Core.Application.Account.Models;
using Core.Common.BaseClasses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Account.Queries.GetAccountDetails
{
    public class GetAccountDetailsQuery : QueryHandlerBase, IRequest<AccountViewModel>
    {
        public string Id { get; set; }
    }
}
