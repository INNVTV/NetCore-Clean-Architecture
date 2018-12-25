using Core.Common.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Accounts.Commands
{
    public class CloseAccountCommand : IRequest<CommandResponse>
    {
        public string Id { get; set; }
    }
}
