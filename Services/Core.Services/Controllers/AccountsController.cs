using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Application.Accounts.Models;
using Core.Application.Accounts.Queries;
using Core.Application.Accounts.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Core.Common.Response;
using Core.Services.ServiceModels;
using AutoMapper;
using Core.Application.Accounts.Enums;

namespace Core.Services.Controllers
{
    [Route("api/accounts")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        readonly IMediator _mediator;
        readonly IMapper _mapper; //<-- Instance version of IMapper. Used only in the Service layer for ServiceModels

        // Constructor automatically pulls in configuration via build in dependancy injection
        public AccountsController(IServiceProvider serviceProvider, IMapper mapper)
        {
            _mediator = serviceProvider.GetService<IMediator>();
            _mapper = mapper;
        }

        // GET: api/accounts/list
        [Route("list")]
        [HttpGet]
        public async Task<AccountListViewModel> ListAsync(int pageSize = 20, OrderBy orderBy = OrderBy.Name, OrderDirection orderDirection = OrderDirection.ASC, string continuationToken = null)
        {
            // We don't use the GetAccountListQuery in the controller method otherwise Swagger tries to use a POST on our GET call
            var accountListQuery = new GetAccountListQuery {PageSize = pageSize, OrderBy = orderBy, OrderDirection = orderDirection, ContinuationToken = continuationToken };
            var result = await _mediator.Send(accountListQuery);
            return result;

            //-----------------------------------------------------
            // TODO: DocumentDB will soon have skip/take
            // For now we use continuation token to get next batch from list
            // For even more robust query capabilities you should use the 'search' route
            //-----------------------------------------------------
        }

        // GET: api/accounts/search
        [Route("search")]
        [HttpGet]
        public async Task<AccountListViewModel> SearchAsync(string query, int page, int pageSize = 20, OrderBy orderBy = OrderBy.Name, OrderDirection orderDirection = OrderDirection.ASC)
        {
            // We don't use the GetAccountListQuery in the controller method otherwise Swagger tries to use a POST on our GET call
            var accountListQuery = new GetAccountListQuery { PageSize = pageSize, OrderBy = orderBy, OrderDirection = orderDirection };
            var result = await _mediator.Send(accountListQuery);
            return result;

            //-----------------------------------------------------
            // Uses Azure Search
            //-----------------------------------------------------
        }

        // GET: api/accounts/{nameKey}
        [HttpGet("{nameKey}", Name = "Get")]
        public async Task<AccountViewModel> GetAsync(string nameKey)
        {
            var accountDetailsQuery = new GetAccountDetailsQuery() { NameKey = nameKey };
            return await _mediator.Send(accountDetailsQuery);
        }

        // POST: api/accounts
        [HttpPost]
        public async Task<CommandResponse> PostAsync(CreateAccountServiceModel createAccountServiceModel)
        {
            //Use AutoMapper instance to transform ServiceModel into MediatR Request (Configured in Startup)
            var createAccountCommand = _mapper.Map<CreateAccountCommand>(createAccountServiceModel);

            var result = await _mediator.Send(createAccountCommand);
            return result;
        }

        // PUT: api/accounts/{guid}
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<CommandResponse> Delete(string id)
        {
            var closeAccountCommand = new CloseAccountCommand() { Id = id };
            return await _mediator.Send(closeAccountCommand);
        }
    }
}
