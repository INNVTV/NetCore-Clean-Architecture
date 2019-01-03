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

namespace Core.Application.Accounts.Queries
{
    public class GetAccountListQueryHandler : IRequestHandler<GetAccountListQuery, AccountListViewModel>
    {
        //MediatR will automatically inject  dependencies
        private readonly IMediator _mediator;
        private readonly ICoreConfiguration _coreConfiguration;
        private readonly IDocumentContext _documentContext;


        public GetAccountListQueryHandler(IDocumentContext documentContext, ICoreConfiguration coreConfiguration, IMediator mediator)
        {
            _mediator = mediator;
            _coreConfiguration = coreConfiguration;
            _documentContext = documentContext;

        }

        public async Task<AccountListViewModel> Handle(GetAccountListQuery request, CancellationToken cancellationToken)
        {
            //-----------------------------------------------------
            // TODO: DocumentDB will soon have skip/take
            // For now we use continuation token
            // For even more robust query capabilities you should also use Azure Search
            //-----------------------------------------------------

            // Prepare our domain model to be returned
            var accountsListViewModel = new AccountListViewModel();

            // TODO: Check user role to include data for the view to use
            accountsListViewModel.DeleteEnabled = false;
            accountsListViewModel.EditEnabled = false;

            // TODO: Add page/row/totals
            accountsListViewModel.Total = 0;
            accountsListViewModel.Page = 0;

            try
            {

                // Create the query
                var sqlQuery = new StringBuilder(String.Concat(
                    "SELECT * FROM Documents d ORDER BY d.",
                    request.OrderBy,
                    " ",
                    request.OrderDirection
                    ));

                var sqlSpec = new SqlQuerySpec { QueryText = sqlQuery.ToString() };

                // Generate collection uri
                Uri collectionUri = UriFactory.CreateDocumentCollectionUri(_documentContext.Settings.Database, _documentContext.Settings.Collection);

                // Generate FeedOptions/ParitionKey
                var feedOptions = new FeedOptions
                {
                    PartitionKey = new PartitionKey(Common.Constants.DocumentType.Account()),
                    MaxItemCount = request.PageSize,
                    RequestContinuation = request.ContinuationToken
                };

                // Run query against the document store
                var query = _documentContext.Client.CreateDocumentQuery<AccountDocumentModel>(
                    collectionUri,
                    sqlSpec,
                    feedOptions
                ).AsDocumentQuery();

                var result = await query.ExecuteNextAsync();

                if(query.HasMoreResults)
                {
                    //If there are more results pass back a continuation token
                    accountsListViewModel.ContinuationToken = result.ResponseContinuation;
                }

                /*
                if(result != null && result.Count > 0)
                {
                    foreach (var accountDocument in result.ToList())
                    {
                        //Use AutoMapper to transform DocumentModel into Domain Model (Configure via Core.Startup.AutoMapperConfiguration)
                        var account = AutoMapper.Mapper.Map<Account>(accountDocument);
                        accountsListViewModel.Accounts.Add(account);
                    }
                }*/

                return accountsListViewModel;
            }
            catch
            {
                return null;
            }

        }
    }
}
