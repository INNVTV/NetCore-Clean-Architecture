using Core.Application.Account.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Account.Queries
{
    public class GetAccountDetailQuery : IRequest<AccountViewModel>
    {
        public string Id { get; set; }
    }
}
