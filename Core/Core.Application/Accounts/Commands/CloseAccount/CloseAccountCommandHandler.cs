using Core.Common.Response;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Accounts.Commands
{
    public class CloseAccountCommandHandler : IRequestHandler<CloseAccountCommand, CommandResponse>
    {
        public async Task<CommandResponse> Handle(CloseAccountCommand request, CancellationToken cancellationToken)
        {
            var response = new CommandResponse();
            response.isSuccess = true;

            return response;
        }
    }
}
