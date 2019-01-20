using FluentValidation.Results;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Core.Infrastructure.Configuration;
using Core.Infrastructure.Persistence.DocumentDatabase;
using Core.Common.Response;
using Microsoft.Azure.Documents.Client;
using Core.Infrastructure.Services.Email;
using Core.Application.Accounts.Models.Documents;
using Serilog;
using Core.Application.Accounts.Commands.CreateAccount;
using Core.Common.Responses;
using System.Collections.Generic;
using Microsoft.Azure.Documents;

namespace Core.Application.Accounts.Commands
{
    public class CreateAccountCommandHandler : IRequestHandler<CreateAccountCommand, CreateAccountCommandResponse>
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

        public async Task<CreateAccountCommandResponse> Handle(CreateAccountCommand request, CancellationToken cancellationToken)
        {
            //try  NOTE: Try catch is removed as we have Middleware components  (Core.Infrastructure.Middleware) that capture exceptions as they bubble up the application pipeline
            //           For Console apps and tests: exceptions must be captured at the root
            //{

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
            // ValidationExceptions can be captured using Middleware. However it is not as testable or portable outside of the MVC framework
            // I prefer using manual/granular validation within each command. 
            //=========================================================================

            
            CreateAccountValidator validator = new CreateAccountValidator(_mediator);
            ValidationResult validationResult = validator.Validate(request);
            if(!validationResult.IsValid)
            {
                return new CreateAccountCommandResponse(validationResult.Errors) { Message = "One or more validation errors occurred." };
            }
             


            //=========================================================================
            // CREATE AND STORE OUR DOCUMENT MODEL
            //=========================================================================

            // Create new account Id
            var id = Guid.NewGuid();

            // Create the Account Document Model
            var accountDocumentModel = new AccountDocumentModel(request.Name);

            accountDocumentModel.Owner.Email = request.Email;
            accountDocumentModel.Owner.FirstName = request.FirstName;
            accountDocumentModel.Owner.LastName = request.LastName;

            // Add the ParitionKey to the document (Moved to document constructor)
            //accountDocumentModel.DocumentType = Common.Constants.DocumentType.Account();

            // Generate collection uri
            Uri collectionUri = UriFactory.CreateDocumentCollectionUri(_documentContext.Settings.Database, _documentContext.Settings.Collection);

            ResourceResponse<Document> result;

            try
            {
                // Save the document to document store using the IDocumentContext dependency
                result = await _documentContext.Client.CreateDocumentAsync(
                    collectionUri,
                    accountDocumentModel
                );
            }
            catch (Exception ex)
            {
                // throw DocumentStoreException (if a custom exception type is desired)
                // ... Will be caught, logged and handled by the ExceptionHandlerMiddleware

                // Pass exception up the chain:
                throw ex;

                // ...Or pass along as inner exception:
                //throw new Exception("An error occured trying to use the document store", ex);
            }
            finally
            {
                // Close any open connections, etc...
            }
            

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

                try
                {
                    var emailSent = await _emailService.SendEmail(emailMessage);
                }
                catch (Exception ex)
                {
                    // throw EmailServiceException (if a custom exception type is desired)
                    // ... Will be caught, logged and handled by the ExceptionHandlerMiddleware

                    // Pass the exception up the chain:
                    throw ex;

                    // ...Or pass along as inner exception:
                    //throw new Exception("An error occured trying to use the email service", ex);
                }
                finally
                {
                    // Use alternate communication method...
                }
                
                //==========================================================================
                // AUTOMAPPER 
                //=========================================================================
                // Create our domain model using AutoMapper to be returned within our response object.
                // Add additional mappings into the: Core.Startup.AutoMapperConfiguration class.

                var account = AutoMapper.Mapper.Map<Account>(accountDocumentModel);

                //==========================================================================
                // POST COMMAND CHECKLIST 
                //=========================================================================
                // 1. CACHING: Update cache.
                // 2. SEARCH INDEX: Update Search index or send indexer request.
                //-----------------------------------------------------------------------

                return new CreateAccountCommandResponse { isSuccess = true, Account = account, Message = "Account created." };
            }
            else
            {                  
                return new CreateAccountCommandResponse { Message = "Could not save model to document store. Status code: " + result.StatusCode };
            }
        }
    }
}
