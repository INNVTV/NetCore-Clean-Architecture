using Core.Application.Account.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Account.Commands.Handlers
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, AccountModel>
    {
        public Task<AccountModel> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
