using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Accounts.Models.Documents
{
    public class AccountDocumentModel
    {
        public AccountDocumentModel(string name)
        {
            // Set our document partitioning property
            DocumentType = Common.Constants.DocumentType.Account();
           
            //Create our Id
            Id = Guid.NewGuid().ToString();
            
            Name = name;
            NameKey = Common.Transformations.NameKey.Transform(name);
            CreatedDate = DateTime.UtcNow;

            Owner = new Owner();
            Active = true;
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
