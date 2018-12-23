using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Accounts.Models
{
    public class AccountViewModel
    {
        public Account Account { get; set; }

        public bool EditEnabled { get; set; }
        public bool DeleteEnabled { get; set; }
    }
}
