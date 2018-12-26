using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Accounts.Models
{
    public class AccountDocumentModel
    {
        public AccountDocumentModel()
        {
            Owner = new Owner();
        }

        [JsonProperty(PropertyName = "id")] //<-- Required for all Documents
        public string Id { get; set; }

        [JsonProperty(PropertyName = "_docType")]
        public string DocumentType { get; internal set; } //<-- Our paritioning property

        public string Name { get; set; }
        public string NameKey { get; set; }

        public bool Active { get; set; }

        public DateTime CreatedDate {get; set;}

        public Owner Owner { get; set; }
    }

    public class Owner
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
