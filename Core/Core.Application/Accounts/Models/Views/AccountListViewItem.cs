using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Accounts.Models.Views
{
    [JsonObject(Title = "Account")] //<-- Update name for OpenAPI/Swagger
    public class AccountListViewItem
    {
        public AccountListViewItem()
        {
        }

        public string Id { get; set; }
        public string Name { get; set; }
        public string NameKey { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
