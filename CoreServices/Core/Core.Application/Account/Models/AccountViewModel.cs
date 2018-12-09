using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Account.Models
{
    class AccountViewModel
    {
        public AccountModel Account { get; set; }

        public bool EditEnables { get; set; }
        public bool DeleteEnabled { get; set; }
    }
}
