using Core.Application.Accounts.Models.Documents;
using Core.Common.Response;
using Core.Infrastructure.Configuration;
using Core.Infrastructure.Persistence.DocumentDatabase;
using Core.Infrastructure.Services.Email;
using FluentValidation.Results;
using MediatR;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Accounts.Commands
{
    public class CloseAccountCommandHandler : IRequestHandler<CloseAccountCommand, BaseResponse>
    {
        //MediatR will automatically inject dependencies
        private readonly IMediator _mediator;
        private readonly ICoreConfiguration _coreConfiguration;
        private readonly IDocumentContext _documentContext;
        private readonly IEmailService _emailService;

        public CloseAccountCommandHandler(IMediator mediator, IDocumentContext documentContext, ICoreConfiguration coreConfiguration, IEmailService emailService)
        {
            _mediator = mediator;
            _coreConfiguration = coreConfiguration;
            _documentContext = documentContext;
            _emailService = emailService;
        }

        public async Task<BaseResponse> Handle(CloseAccountCommand request, CancellationToken cancellationToken)
        {
            //=========================================================================
            // VALIDATE OUR COMMAND REQUEST
            //=========================================================================

            CloseAccountValidator validator = new CloseAccountValidator();
            ValidationResult validationResult = validator.Validate(request);
            if (!validationResult.IsValid)
            {
                return new BaseResponse(validationResult.Errors) { Message = "Invalid Input" };
            }

            //=========================================================================
            // GET ACCOUNT TO CLOSE FROM DOCUMENT STORE
            //=========================================================================

            // Create the query
            string sqlQuery = "SELECT * FROM Documents d WHERE d.id ='" + request.Id + "'";
            var sqlSpec = new SqlQuerySpec { QueryText = sqlQuery };

            // Generate collection uri
            Uri collectionUri = UriFactory.CreateDocumentCollectionUri(_documentContext.Settings.Database, _documentContext.Settings.Collection);

            // Generate FeedOptions/ParitionKey
            var feedOptions = new FeedOptions
            {
                PartitionKey = new PartitionKey(Common.Constants.DocumentType.Account())
            };

            // Run query against the document store
            var getResult = _documentContext.Client.CreateDocumentQuery<AccountDocumentModel>(
                collectionUri,
                sqlSpec,
                feedOptions
            );

            var accountDocumentModel = getResult.AsEnumerable().FirstOrDefault();

            if (accountDocumentModel == null)
            {
                return new BaseResponse { Message = "Account does not exist." };   
            }


            //=========================================================================
            // DELETE DOCUMENT FROM DOCUMENT STORE
            //=========================================================================

            // Generate RequestOptions/ParitionKey
            var requestOptions = new RequestOptions
            {
                PartitionKey = new PartitionKey(Common.Constants.DocumentType.Account())
            };

            // Generate Document Uri
            Uri documentUri = UriFactory.CreateDocumentUri(_documentContext.Settings.Database, _documentContext.Settings.Collection, accountDocumentModel.Id);

            // Save the document to document store using the IDocumentContext dependency
            var result = await _documentContext.Client.DeleteDocumentAsync(documentUri, requestOptions);

            if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
            {
                //=========================================================================
                // SEND EMAIL 
                //=========================================================================
                // Send an email to the account owner using the IEmailService dependency
                var emailMessage = new EmailMessage
                {
                    ToEmail = accountDocumentModel.Owner.Email,
                    ToName = String.Concat(accountDocumentModel.Owner.FirstName, " ", accountDocumentModel.Owner.LastName),
                    Subject = "Account closed",
                    TextContent = String.Concat("Your account: '", accountDocumentModel.Name, "' has been closed."),
                    HtmlContent = String.Concat("Your account: <b>", accountDocumentModel.Name, "</b> has been closed.")
                };

                var emailSent = await _emailService.SendEmail(emailMessage);

                /*=========================================================================
                 * CLEANUP ROUTINES
                 * =========================================================================
                 * 
                 *  1. Delete all document storage partitions associated with this account
                 *  
                 *  2. Delete all storage data associated with this account.
                 *     (blobs, tables and queues)
                 *     
                 *  3. Purge all caches that may still hold data associated with the account
                 *  
                 *  4. Update search index or send indexer request
                 *  
                 *  NOTE: This can be done via message queue picked up by a worker proccess
                 *        or by a record checked by a custodal process
                 *  
                 *  NOTE: The ability to delete an entire document partition is likely coming
                 *        in a future CosmosDB release.
                 *  
                 * --------------------------------------------------------------------------
                 */

                return new BaseResponse { isSuccess = true, Message = "Account has been closed." };
            }
            else
            {
                return new BaseResponse { Message = "Could not delete document. Status code: " + result.StatusCode };
            }

        }
    }
}
