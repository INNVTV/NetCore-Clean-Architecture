using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using RazorPages.CoreServices;

namespace RazorPages.Pages.Accounts
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public CreateAccountServiceModel NewAccount {get; set;}

        [BindProperty]
        public CreateAccountCommandResponse CreateAccountResponse { get; set; }

        private string baseUrl { get; set; }

        public CreateModel(IConfiguration configuration)
        {
            baseUrl = configuration.GetSection("CoreServices").GetSection("BaseUrl").Value;
        }

        public void OnGet()
        {
            CreateAccountResponse = null;
            NewAccount = new CreateAccountServiceModel();
        }

        public async Task<IActionResult> OnPost()
        {
            var client = new AccountsClient(baseUrl);
            var CreateAccountResponse = await client.CreateAsync(NewAccount);


            // Using HttpClient without OpenAPI/Swagger:
            //HttpClient client = new HttpClient();
            //HttpResponseMessage response = await client.PostAsJsonAsync(
            //baseUrl + "/api/accounts", NewAccount);


            if (!CreateAccountResponse.IsSuccess.Value)
            {
                return Page();
            }
            else
            {
                return RedirectToPage("Index");
            }       
        }

        public async Task<IActionResult> OnPostClear()
        {
            //ValidationIssues = null;
            //NewAccount = null;

            return RedirectToPage("Create");
            //return Page();
        }
    }
}