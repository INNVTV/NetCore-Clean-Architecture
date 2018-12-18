using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Account.Models
{
    public class AccountViewModel
    {
        public AccountDocumentModel Account { get; set; }

        public bool EditEnabled { get; set; }
        public bool DeleteEnabled { get; set; }
    }
}
