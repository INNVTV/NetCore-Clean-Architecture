using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Accounts.Commands
{
    public class CloseAccountCommand : IRequest<CloseAccountResponse>
    {
        public string Id { get; set; }
    }
}
