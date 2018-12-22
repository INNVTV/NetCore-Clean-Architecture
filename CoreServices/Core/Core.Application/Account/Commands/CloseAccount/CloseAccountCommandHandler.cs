using Core.Application.Account.Commands.Handlers.Responses;
using Core.Common.BaseClasses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Account.Commands.CloseAccount
{
    public class CloseAccountCommandHandler : CommandHandlerBase, IRequestHandler<CloseAccountCommand, CloseAccountResponse>
    {
        public async Task<CloseAccountResponse> Handle(CloseAccountCommand request, CancellationToken cancellationToken)
        {
            var response = new CloseAccountResponse();
            response.isSuccess = true;

            return response;
        }
    }
}
