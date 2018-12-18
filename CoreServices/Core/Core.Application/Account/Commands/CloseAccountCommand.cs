using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Account.Commands
{
    public class CloseAccountCommand : IRequest<bool>
    {
        public string Id { get; set; }
    }
}
