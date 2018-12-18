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
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, AccountDocumentModel>
    {
        public Task<AccountDocumentModel> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            var id = Guid.NewGuid();

            CreateAccountValidator validator = new CreateAccountValidator();
            ValidationResult result = validator.Validate(request);

            //==========================================================================
            // EVENT SOURCING - REFACTORING NOTES
            //=========================================================================
            // When migrating to Event Sourcing you will no longer be using Document Client to store the new account
            // Instead you will create a "AccountCreated" event (events are names in the past tense)
            // THis event will later be used as part of your aggregate to create an Accout "projection" or "aggregate"
            // You will also be seperating your Write store from your Read store as this is the           

            throw new NotImplementedException();
        }
    }
}
