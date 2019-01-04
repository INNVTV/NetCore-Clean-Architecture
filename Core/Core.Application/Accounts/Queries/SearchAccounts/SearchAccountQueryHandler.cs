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
            // TODO: Set up search either by manual updates through commands or through an indexer.
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

                // Create the query
                

                return accountsSearchViewModel;
            }
            catch(Exception e)
            {
                // Log our exception.
                // Use structured logging to capture the full exception object.

                return null;
            }

        }
    }
}
