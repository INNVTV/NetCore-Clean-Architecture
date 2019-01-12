using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RazorPages.CoreServices;

namespace RazorPages.Pages.Accounts
{
    public class CreateModel : PageModel
    {
        [BindProperty]
        public CreateAccountServiceModel createAccountServiceModel {get; set;}

        [BindProperty]
        public List<RazorPages.CoreServices.ValidationIssue> validationIssues { get; set; }

        public void OnGet()
        {
            validationIssues = null;
            createAccountServiceModel = new CreateAccountServiceModel();
        }

        public void OnPost()
        {

            var coreServicesClient = new CoreServices.Client("", new System.Net.Http.HttpClient());

            
            var name = createAccountServiceModel.Name;
            var email = createAccountServiceModel.Email;
        }
    }
}