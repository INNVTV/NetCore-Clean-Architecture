using FluentValidation.Results;
using Core.Application.Accounts.Models;
using MediatR;
using System;
using System.Threading;
using System.Threading.Tasks;
using Core.Domain.Entities;
using Core.Application.Accounts.Queries;
using System.Collections.Generic;
using Core.Infrastructure.Configuration;
using Core.Infrastructure.Persistence.DocumentDatabase;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System.Linq;
using System.Text;
using Microsoft.Azure.Documents.Linq;
using Core.Application.Accounts.Queries.SearchAccounts;
using Core.Application.Accounts.Models.Views;
using Core.Application.Accounts.Models.Documents;

namespace Core.Application.Accounts.Queries
{
    public class SearchAccountsQueryHandler : IRequestHandler<SearchAccountsQuery, AccountSearchResultsViewModel>
    {
        //MediatR will automatically inject  dependencies
        private readonly IMediator _mediator;
        private readonly ICoreConfiguration _coreConfiguration;
        private readonly IDocumentContext _documentContext;


        public SearchAccountsQueryHandler(IDocumentContext documentContext, ICoreConfiguration coreConfiguration, IMediator mediator)
        {
            _mediator = mediator;
            _coreConfiguration = coreConfiguration;
            _documentContext = documentContext;

        }

        public async Task<AccountSearchResultsViewModel> Handle(SearchAccountsQuery request, CancellationToken cancellationToken)
        {
            //-----------------------------------------------------
            //
            //  TODO:
            //  
            //  1. Set up a search index for "Accounts"
            //  2. Update index by manual updates through commands or through an automatic indexer.
            //
            //  CosmosDB has integrated indexing capabilities with Azure Search.
            //  Note: Once integrated you will need to set up a good interval for scanning your document store that makes sense for your application.
            //  It may make sense to include a call to manually run deltas with an API call whenever you run a command that adds, updates or deletes data in your store so that the update is reflected immediatly in your search results.
            // 
            //-----------------------------------------------------

            // Prepare our view model to be returned
            var accountsSearchViewModel = new AccountSearchResultsViewModel();

            // TODO: Check user role to include data for the view to use
            accountsSearchViewModel.DeleteEnabled = false;
            accountsSearchViewModel.EditEnabled = false;

            // Update return object with incoming properties
            accountsSearchViewModel.Page = request.Page;

            try
            {
                // Create the query. Pass it to Azure Search. Send results to caller:                
                return accountsSearchViewModel;
            }
            catch(Exception ex)
            {
                // throw AzureSearchException (if a custom exception type is desired)
                // ... Will be caught, logged and handled by the ExceptionHandlerMiddleware

                // ...Or pass along as inner exception:
                throw new Exception("An error occured trying to use the search service", ex);
            }
            
        }
    }
}
