using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Application.Account.Queries;
using Core.Common.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;

namespace Core.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountsController : ControllerBase
    {
        //readonly ICoreConfiguration _coreConfiguration;
        readonly IServiceProvider _serviceProvider;

        // Constructor automatically pulls in configuration via build in dependancy injection
        public AccountsController(IServiceProvider serviceProvider)//ICoreConfiguration coreConfiguration)
        {
            //We add a constructor for Dependancy Injection of confirguration into the controller
            //_coreConfiguration = coreConfiguration;
            _serviceProvider = serviceProvider;
        }

        // GET: api/Accounts
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Accounts/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            //return Settings.
            //return _configuration["Application:Name"];
            var accountsListQuery = new GetAccountsListQuery(_serviceProvider);
            return accountsListQuery.GetAccountsListQueryName();
        }

        // POST: api/Accounts
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Accounts/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
