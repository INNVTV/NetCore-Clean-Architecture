using FluentValidation.Results;
using Core.Application.Accounts.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Common.Exceptions;
using Core.Domain.Entities;
using Core.Application.Accounts.Queries;
using Core.Infrastructure.Configuration;
using Core.Infrastructure.Persistence.DocumentDatabase;
using Core.Common.Response;

namespace Core.Application.Accounts.Commands
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, CommandResponse>
    {
        //MediatR will automatically inject out dependencies
        private readonly IMediator _mediator;
        private readonly ICoreConfiguration _coreConfiguration;
        private readonly IDocumentContext _documentContext;


        public CreateAccountCommandHandler(IDocumentContext documentContext, ICoreConfiguration coreConfiguration, IMediator mediator)
        {
            _mediator = mediator;
            _coreConfiguration = coreConfiguration;
            _documentContext = documentContext;

            //Log Activity

            //Authorization

            //_coreConfiguration = coreConfiguration;

            //_context = context;
            //_notificationService = notificationService;
        }

        public async Task<CommandResponse> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            // Prepare our domain model to be returned
            var newAccount = new Account();

            // Create the new account Id
            var id = Guid.NewGuid();

            //==========================================================================
            // VALIDATE OUR COMMAND REQUEST
            //=========================================================================

            CreateAccountValidator validator = new CreateAccountValidator();
            ValidationResult validationResult = validator.Validate(request);
            if(!validationResult.IsValid)
            {
                throw new FluentValidation.ValidationException(validationResult.Errors);
            }

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

            //var result = await _documentContext.Client.CreateDocumentAsync("", accountDocumentModel);

            if(true)//result.StatusCode == System.Net.HttpStatusCode.OK)
            {
                // Example of using Mediatr to return a list of accounts (Should be removed)
                var accountListQuery = new GetAccountsListQuery();
                var accountList = await _mediator.Send(accountListQuery);



                //==========================================================================
                // AUTOMAPPER 
                //=========================================================================
                // Add additional mappings into the: Core.Startup.AutoMapperConfiguration class.

                var account = AutoMapper.Mapper.Map<Account>(accountDocumentModel);



                return new CommandResponse { isSuccess = true, Object = account };
            }
            else
            {
                //Handle Exception
                throw new CreateException(nameof(accountDocumentModel), accountDocumentModel.Id);
            }

        }
    }
}
