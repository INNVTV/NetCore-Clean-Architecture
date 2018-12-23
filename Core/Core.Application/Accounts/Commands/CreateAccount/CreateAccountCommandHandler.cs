using FluentValidation.Results;
using Core.Application.Accounts.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Common.Configuration;
using Core.Common.Exceptions;
using Core.Common.BaseClasses;
using Core.Domain.Entities;

namespace Core.Application.Accounts.Commands.CreateAccount
{
    public class CreateAccountCommandHandler : CommandHandlerBase, IRequestHandler<CreateAccountCommand, AccountViewModel>
    {
        //private readonly NorthwindDbContext _context;
        //private readonly INotificationService _notificationService;
        private readonly ICoreConfiguration _coreConfiguration;

        public CreateAccountCommandHandler(
            ICoreConfiguration coreConfiguration)
           // NorthwindDbContext context,
            //INotificationService notificationService)
        {
            _coreConfiguration = coreConfiguration;
            //_context = context;
            //_notificationService = notificationService;
        }

        public async Task<AccountViewModel> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            // Prepare our domain model to be returned
            var newAccount = new Account();

            // Create the new account Id
            var id = Guid.NewGuid();

            CreateAccountValidator validator = new CreateAccountValidator();
            ValidationResult validationResult = validator.Validate(request);

            //==========================================================================
            // EVENT SOURCING - REFACTORING NOTES
            //=========================================================================
            // When migrating to Event Sourcing you will no longer be using Document Client to store the new account
            // Instead you will create a "AccountCreated" event (events are names in the past tense)
            // THis event will later be used as part of your aggregate to create an Accout "projection" or "aggregate"
            // You will also be seperating your Write store from your Read store as this is the           

            var accountDocumentModel = new AccountDocumentModel();
            accountDocumentModel.Id = id.ToString();
            accountDocumentModel.Name = request.Name;
            accountDocumentModel.NameKey = Common.Transformations.NameKey.Transform(request.Name);
            accountDocumentModel.CreatedDate = DateTime.UtcNow;

            var result = await _coreConfiguration.Azure.CosmosDb.Client.CreateDocumentAsync("", accountDocumentModel);

            if(result.StatusCode == System.Net.HttpStatusCode.OK)
            {

                // Use AutoMapper to transform document model to domain model
                var account = new Account();

                // Add account domain model to the view model
                var accountViewModel = new AccountViewModel { Account = account };

                // Add the remaining authorization details to the view model
                accountViewModel.DeleteEnabled = false;
                accountViewModel.EditEnabled = false;

                return accountViewModel;
            }
            else
            {
                //Handle Exception
                throw new CreateException(nameof(accountDocumentModel), accountDocumentModel.Id);
            }

        }
    }
}
