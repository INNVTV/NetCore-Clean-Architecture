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

namespace Core.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        readonly IMediator _mediator;

        // Constructor automatically pulls in configuration via build in dependancy injection
        public AccountsController(IServiceProvider serviceProvider)//ICoreConfiguration coreConfiguration)
        {
            //_serviceProvider = serviceProvider;
            _mediator = serviceProvider.GetService<IMediator>();
        }

        // GET: api/accounts
        [HttpGet]
        public async Task<IEnumerable<AccountViewModel>> GetAsync()
        {
            var createAccountCommand = new CreateAccountCommand()
            {
                Name = "Account Name 7",
                Email = "kaz@innvtv.com",
                FirstName = "John",
                LastName = "Smith"
            };

            // Send our command to MediatR for processing...
            var createAccountResponse = await _mediator.Send(createAccountCommand);

            var accountListQuery = new GetAccountListQuery();
            var result = await _mediator.Send(accountListQuery);
            return result;
        }

        // GET: api/accounts/{guid}
        [HttpGet("{nameKey}", Name = "Get")]
        public async Task<AccountViewModel> GetAsync(string nameKey)
        {
            var accountDetailsQuery = new GetAccountDetailsQuery() { NameKey = nameKey };
            return await _mediator.Send(accountDetailsQuery);
        }

        // POST: api/accounts
        [HttpPost]
        public async Task<AccountViewModel> PostAsync([FromBody] CreateAccountCommand createAccountCommand)
        {
            var result = await _mediator.Send(createAccountCommand);
            return (AccountViewModel)result.Object;
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
