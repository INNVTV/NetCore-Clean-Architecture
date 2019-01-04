using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Accounts.Models
{
    public class AccountSearchViewModel
    {
        public AccountSearchViewModel()
        {
            Accounts = new List<AccountListItem>();
        }

        public List<AccountListItem> Accounts { get; set; }

        public int Total;
        public int Next;
        public int Page;

        public bool EditEnabled { get; set; }
        public bool DeleteEnabled { get; set; }
    }
}
