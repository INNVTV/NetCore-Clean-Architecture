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
                var sqlQuery = new StringBuilder("SELECT * FROM Documents d");

                //Check for options or fall back to defaults
                if(!String.IsNullOrEmpty(request.OrderBy.ToString()))
                {
                    sqlQuery.Append(" ORDER BY d.");
                    sqlQuery.Append(request.OrderBy);

                    if(!String.IsNullOrEmpty(request.OrderDirection.ToString()))
                    {
                        sqlQuery.Append(" ");
                        sqlQuery.Append(request.OrderDirection);
                    }
                    else
                    {
                        sqlQuery.Append(" ASC");
                    }
                }
                else
                {
                    sqlQuery.Append(" ORDER BY d.Name ASC");
                }
                
                var sqlSpec = new SqlQuerySpec { QueryText = sqlQuery.ToString() };

                // Generate collection uri
                Uri collectionUri = UriFactory.CreateDocumentCollectionUri(_documentContext.Settings.Database, _documentContext.Settings.Collection);

                // Generate FeedOptions/ParitionKey
                var feedOptions = new FeedOptions
                {
                    PartitionKey = new PartitionKey(Common.Constants.DocumentType.Account())
                };

                // Run query against the document store
                var result = _documentContext.Client.CreateDocumentQuery<AccountDocumentModel>(
                    collectionUri,
                    sqlSpec,
                    feedOptions
                );

                var accountDocuments = result.ToList();

                if(accountDocuments != null && accountDocuments.Count > 0)
                {
                    foreach (var accountDocument in accountDocuments)
                    {
                        //Use AutoMapper to transform DocumentModel into Domain Model (Configure via Core.Startup.AutoMapperConfiguration)
                        var account = AutoMapper.Mapper.Map<Account>(accountDocument);
                        accountsListViewModel.Accounts.Add(account);
                    }
                }

                return accountsListViewModel;
            }
            catch
            {
                return null;
            }

        }
    }
}
