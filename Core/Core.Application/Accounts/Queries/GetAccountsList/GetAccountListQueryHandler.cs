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

namespace Core.Application.Accounts.Queries
{
    public class GetAccountListQueryHandler : IRequestHandler<GetAccountListQuery, List<AccountViewModel>>
    {
        //MediatR will automatically inject  dependencies
        private readonly IMediator _mediator;
        private readonly ICoreConfiguration _coreConfiguration;
        private readonly IDocumentContext _documentClient;


        public GetAccountListQueryHandler(IDocumentContext documentClient, ICoreConfiguration coreConfiguration, IMediator mediator)
        {
            _mediator = mediator;
            _coreConfiguration = coreConfiguration;
            _documentClient = documentClient;

        }

    public async Task<List<AccountViewModel>> Handle(GetAccountListQuery request, CancellationToken cancellationToken)
        {
            // Prepare our domain model to be returned
            var accountsList = new List<AccountViewModel>();

            return accountsList;

        }
    }
}
