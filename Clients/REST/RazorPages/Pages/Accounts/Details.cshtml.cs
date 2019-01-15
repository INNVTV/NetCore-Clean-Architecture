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
    public class DetailsModel : PageModel
    {
        [BindProperty]
        public AccountDetailsViewModel AccountDetails { get; set; }

        [BindProperty]
        public string Message { get; set; }

        private string NameKey { get; set; }

        private string baseUrl { get; set; }

        public DetailsModel(IConfiguration configuration)
        {
            baseUrl = configuration.GetSection("CoreServices").GetSection("BaseUrl").Value;
        }

        public async Task<IActionResult> OnGet(string nameKey)
        {
            NameKey = nameKey;

            var client = new AccountsClient(baseUrl);
            AccountDetails = await client.DetailsAsync(NameKey);

            return Page();
        }

        public async Task<IActionResult> OnPostDelete()
        {
            try
            {
                var client = new AccountsClient(baseUrl);
                var results = await client.DeleteAsync(AccountDetails.Account.Id);

                if (results.IsSuccess.Value == true)
                {
                    return RedirectToPage("Index");
                }
                else
                {
                    Message = results.Message;
                    return Page();
                }
            }
            catch(Exception ex)
            {
                Message = $"An error occured: { ex.Message }";
                return Page();
            }
        }
    }
}