using Core.Application.Account.Commands.Validators;
using FluentValidation.Results;
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
            var id = Guid.NewGuid();

            CreateAccountValidator validator = new CreateAccountValidator();
            ValidationResult result = validator.Validate(request);

            // When migrating to Event Sourcing you will no longer be using Document Client to store the new account
            // Instead you will create a "AccountCreated" event that will later be used as part of your aggregate to create an Accout "projection"
            // You will also be seperating your Write store from your Read store.           

            throw new NotImplementedException();
        }
    }
}
