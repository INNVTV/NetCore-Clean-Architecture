using FluentValidation.Results;
using Core.Application.Accounts.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Core.Application.Accounts.Queries;
using Core.Infrastructure.Configuration;
using Core.Infrastructure.Persistence.DocumentDatabase;
using Core.Common.Response;
using Microsoft.Azure.Documents.Client;
using Microsoft.Azure.Documents;

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

            //_notificationService = notificationService;
        }

        public async Task<CommandResponse> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            try
            {
                //==========================================================================
                // EVENT SOURCING - REFACTORING NOTES
                //=========================================================================
                // When migrating to Event Sourcing you will no longer be using Document Client to store the new account
                // Instead you will create a "AccountCreated" event (events are names in the past tense)
                // THis event will later be used as part of your aggregate to create an Accout "projection" or "aggregate"
                // You will also be seperating your Write store from your Read store as this is the           
                //--------------------------------------------------------------------------


                //=========================================================================
                // VALIDATE OUR COMMAND REQUEST
                //=========================================================================

                CreateAccountValidator validator = new CreateAccountValidator();
                ValidationResult validationResult = validator.Validate(request);
                if(!validationResult.IsValid)
                {
                    return new CommandResponse { Message = "Invalid Input", ValidationErrors = validationResult.Errors };
                }

                //=========================================================================
                // VALIDATE ACCOUNT NAME IS UNIQUE (Via MediatR Query)
                //=========================================================================
                // Note: "NameKey" is transformed from "Name" and is used as a both a unique id as well as for pretty routes/urls
                // Note: Consider using both "Name and ""NameKey" as UniqueKeys on your DocumentDB collection.
                // Note: Once contraints are in place you could remove this manual check - however this process does ensure no exceptions are thrown and a cleaner response message.

                var accountDetailsQuery = new GetAccountDetailsQuery { NameKey = Common.Transformations.NameKey.Transform(request.Name) };
                var accountDetails = await _mediator.Send(accountDetailsQuery);

                if(accountDetails != null)
                {
                    if (accountDetails.Account != null)
                    {
                        return new CommandResponse { Message = "Account name already exists." };
                    }
                }


                //=========================================================================
                // CREATE AND STORE OUR DOCUMENT MODEL
                //=========================================================================

                // Create new account Id
                var id = Guid.NewGuid();

                // Create the Account Document Model
                var accountDocumentModel = new AccountDocumentModel();
                accountDocumentModel.Id = id.ToString();
                accountDocumentModel.Name = request.Name;
                accountDocumentModel.NameKey = Common.Transformations.NameKey.Transform(request.Name);
                accountDocumentModel.Active = true;
                accountDocumentModel.CreatedDate = DateTime.UtcNow;

                // Add the ParitionKey to the document
                accountDocumentModel.DocumentType = Common.Constants.DocumentType.Account();

                // Generate collection uri
                Uri collectionUri = UriFactory.CreateDocumentCollectionUri(_documentContext.Settings.Database, _documentContext.Settings.Collection);

                // Save the document to document store
                var result = await _documentContext.Client.CreateDocumentAsync(
                    collectionUri,
                    accountDocumentModel
                );

                if(result.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    //==========================================================================
                    // AUTOMAPPER 
                    //=========================================================================
                    // Create our domain model using AutoMapper to be returned within our response object.
                    // Add additional mappings into the: Core.Startup.AutoMapperConfiguration class.

                    var account = AutoMapper.Mapper.Map<Account>(accountDocumentModel);

                    return new CommandResponse { isSuccess = true, Object = account };
                }
                else
                {                  
                    return new CommandResponse { Message = "Could not save model to document store. Status code:" + result.StatusCode };
                }
            }
            catch(Exception e)
            {
                //Handle Exception
                //throw new CreateException(nameof(accountDocumentModel), accountDocumentModel.Id);
                return new CommandResponse { Message = e.Message };
            }

        }
    }
}
