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
        public string Error { get; set; }

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

        public async Task<IActionResult> OnPostDelete(string id)
        {
            try
            {
                var client = new AccountsClient(baseUrl);
                var results = await client.DeleteAsync(id);

                if (results.IsSuccess.Value == true)
                {
                    Message = "Account closed";
                    return Page();
                    //return RedirectToPage("Index");
                }
                else
                {
                    Error = results.Message;
                    return Page();
                }
            }
            catch(Exception ex)
            {
                Error = $"An error occured: { ex.Message }";
                return Page();
            }
        }
    }
}