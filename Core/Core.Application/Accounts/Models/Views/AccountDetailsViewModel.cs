using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Accounts.Models.Views
{
    public class AccountDetailsViewModel
    {
        public AccountDetailsViewModel()
        {
            EditEnabled = false;
            DeleteEnabled = false;
        }

        public Account Account { get; set; }

        public bool EditEnabled { get; set; }
        public bool DeleteEnabled { get; set; }
    }
}
