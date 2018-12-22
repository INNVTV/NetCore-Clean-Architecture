using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Account.Models
{
    public class AccountDocumentModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string NameKey { get; set; }

        public DateTime CreatedDate {get; set;}
        
    }
}
