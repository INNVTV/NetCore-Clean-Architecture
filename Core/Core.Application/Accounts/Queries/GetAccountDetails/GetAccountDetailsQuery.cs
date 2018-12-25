using Core.Application.Accounts.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Accounts.Queries
{
    public class GetAccountDetailsQuery : IRequest<AccountViewModel>
    {
        public string Id { get; set; }
    }
}
