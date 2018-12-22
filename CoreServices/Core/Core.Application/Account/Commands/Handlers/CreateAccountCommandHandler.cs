using Core.Application.Account.Commands.Validators;
using FluentValidation.Results;
using Core.Application.Account.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Common.Configuration;
using Core.Common.Exceptions;
using Core.Common.BaseClasses;

namespace Core.Application.Account.Commands.Handlers
{
    public class CreateAccountCommandHandler : CommandHandlerBase, IRequestHandler<CreateAccountCommand, AccountDocumentModel>
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

        public async Task<AccountDocumentModel> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
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
            accountDocumentModel.Name = request.AccountName;
            accountDocumentModel.NameKey = Common.Transformations.NameKey.Transform(request.AccountName);
            accountDocumentModel.CreatedDate = DateTime.UtcNow;

            var result = await _coreConfiguration.Azure.CosmosDb.Client.CreateDocumentAsync("", accountDocumentModel);

            if(result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                return true; // accountDocumentModel;
            }
            else
            {
                //Handle Exception
                throw new CreateException(nameof(accountDocumentModel), accountDocumentModel.Id);
            }

            //TODO: Complete and Test Validation (Make part of MediatR)
            //TODO: Add Exception Response
            //TODO: Add Cross-Cutting (ActivityLog, ExceptionLog, ErrorLog, Authorization)
        }
    }
}
