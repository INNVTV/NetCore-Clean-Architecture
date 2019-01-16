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
        [BindProperty]
        public AccountListResultsViewModel AccountListView { get; set; }

        private string baseUrl { get; set; }

        public IndexModel(IConfiguration configuration)
        {
            baseUrl = configuration.GetSection("CoreServices").GetSection("BaseUrl").Value;
            AccountListView = new AccountListResultsViewModel();
        }

        public async Task<IActionResult> OnGet()
        {
            var client = new AccountsClient(baseUrl);
            var response = await client.ListAsync(10, OrderBy.CreatedDate, OrderDirection.DESC, "");

            if(response != null)
            {
                AccountListView = response;
            }
            else
            {
                AccountListView.Count = 0;
            }

            return Page();

        }

        public async Task<IActionResult> OnGetNext()
        {
            var client = new AccountsClient(baseUrl);
            var response = await client.ListAsync(10, OrderBy.CreatedDate, OrderDirection.DESC, AccountListView.ContinuationToken);


            AccountListView = response;

            return Page();

        }
    }
}