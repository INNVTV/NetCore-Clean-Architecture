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
using Core.Infrastructure.Services.Email;

namespace Core.Application.Accounts.Commands
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, CommandResponse>
    {
        //MediatR will automatically inject dependencies
        private readonly IMediator _mediator;
        private readonly ICoreConfiguration _coreConfiguration;
        private readonly IDocumentContext _documentContext;
        private readonly IEmailService _emailService;

        public CreateAccountCommandHandler(IMediator mediator, IDocumentContext documentContext, ICoreConfiguration coreConfiguration, IEmailService emailService)
        {
            _mediator = mediator;
            _coreConfiguration = coreConfiguration;
            _documentContext = documentContext;
            _emailService = emailService;
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
                // VALIDATE OUR COMMAND REQUEST USING FLUENT VALIDATION
                //=========================================================================

                CreateAccountValidator validator = new CreateAccountValidator(_mediator);
                ValidationResult validationResult = validator.Validate(request);
                if(!validationResult.IsValid)
                {
                    return new CommandResponse { Message = "One or more validation errors occurred.", ValidationErrors = validationResult.Errors };
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

                accountDocumentModel.Owner.Email = request.Email;
                accountDocumentModel.Owner.FirstName = request.FirstName;
                accountDocumentModel.Owner.LastName = request.LastName;

                // Add the ParitionKey to the document
                accountDocumentModel.DocumentType = Common.Constants.DocumentType.Account();

                // Generate collection uri
                Uri collectionUri = UriFactory.CreateDocumentCollectionUri(_documentContext.Settings.Database, _documentContext.Settings.Collection);

                // Save the document to document store using the IDocumentContext dependency
                var result = await _documentContext.Client.CreateDocumentAsync(
                    collectionUri,
                    accountDocumentModel
                );

                if(result.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    //==========================================================================
                    // SEND EMAIL 
                    //=========================================================================
                    // Send an email to the new account owner using the IEmailService dependency
                    var emailMessage = new EmailMessage
                    {
                        ToEmail = request.Email,
                        ToName = String.Concat(request.FirstName, " ", request.LastName),
                        Subject = "Account created",
                        TextContent = String.Concat("Thank you ", String.Concat(request.FirstName, " ", request.LastName), "! your account named ", request.Name, " has been created!"),
                        HtmlContent = String.Concat("Thank you ", String.Concat(request.FirstName, " ", request.LastName), ",<br>Your account named <b>", request.Name, "</b> has been created!")
                    };

                    var emailSent = await _emailService.SendEmail(emailMessage);


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
                    return new CommandResponse { Message = "Could not save model to document store. Status code: " + result.StatusCode };
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
