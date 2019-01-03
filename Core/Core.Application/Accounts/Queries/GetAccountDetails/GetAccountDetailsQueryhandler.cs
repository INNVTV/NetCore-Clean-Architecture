using Core.Application.Accounts.Models;
using Core.Domain.Entities;
using Core.Infrastructure.Configuration;
using Core.Infrastructure.Persistence.DocumentDatabase;
using MediatR;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Core.Application.Accounts.Queries
{
    public class GetAccountDetailsQueryHandler : IRequestHandler<GetAccountDetailsQuery, AccountViewModel>
    {
        //MediatR will automatically inject dependencies
        private readonly IMediator _mediator;
        private readonly ICoreConfiguration _coreConfiguration;
        private readonly IDocumentContext _documentContext;

        public GetAccountDetailsQueryHandler(IDocumentContext documentContext, ICoreConfiguration coreConfiguration, IMediator mediator)
        {
            _mediator = mediator;
            _coreConfiguration = coreConfiguration;
            _documentContext = documentContext;

            //Log Activity, Check Authorization, Etc...
        }

        public async Task<AccountViewModel> Handle(GetAccountDetailsQuery request, CancellationToken cancellationToken)
        {
            try
            {

                // Create the query
                string sqlQuery = "SELECT * FROM Documents d WHERE d.NameKey ='" + Common.Transformations.NameKey.Transform(request.NameKey) + "'";
                var sqlSpec = new SqlQuerySpec { QueryText = sqlQuery };

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

                var accountDocumentModel = result.AsEnumerable().FirstOrDefault();


                // Create our ViewModel and transform our document model
                var accountViewModel = new AccountViewModel();

                // TODO: Check user role to include data for the view to use
                accountViewModel.DeleteEnabled = false;
                accountViewModel.EditEnabled = false;

                if(accountDocumentModel != null)
                {
                    //Use AutoMapper to transform DocumentModel into Domain Model (Configure via Core.Startup.AutoMapperConfiguration)
                    var account = AutoMapper.Mapper.Map<Account>(accountDocumentModel);
                    accountViewModel.Account = account;
                }

                return accountViewModel;
            }
            catch
            {
                return null;
            }
        }
    }
}
