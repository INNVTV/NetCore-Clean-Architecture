using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using RazorPages.CoreServices;

namespace RazorPages.Pages.Accounts
{
    public class IndexModel : PageModel
    {
        public AccountListResultsViewModel AccountListView { get; set; }

        private string baseUrl { get; set; }

        public IndexModel(IConfiguration configuration)
        {
            baseUrl = configuration.GetSection("CoreServices").GetSection("BaseUrl").Value;
        }

        public async void OnGet()
        {
            var client = new AccountsClient(baseUrl);
            var response = await client.ListAsync(2, OrderBy.CreatedDate, OrderDirection.DESC, "");

            
            AccountListView = response;

        }

        public async void OnGetNext()
        {
            var client = new AccountsClient(baseUrl);
            var response = await client.ListAsync(2, OrderBy.CreatedDate, OrderDirection.DESC, AccountListView.ContinuationToken);


            AccountListView = response;

        }
    }
}