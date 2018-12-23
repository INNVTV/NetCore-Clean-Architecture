using Core.Common.BaseClasses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Accounts.Commands
{
    public class CloseAccountCommandHandler : IRequestHandler<CloseAccountCommand, CloseAccountResponse>
    {
        public async Task<CloseAccountResponse> Handle(CloseAccountCommand request, CancellationToken cancellationToken)
        {
            var response = new CloseAccountResponse();
            response.isSuccess = true;

            return response;
        }
    }
}
