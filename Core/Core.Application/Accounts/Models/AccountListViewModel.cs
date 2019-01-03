using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Accounts.Models
{
    public class AccountListViewModel
    {
        public AccountListViewModel()
        {
            Accounts = new List<Account>();
        }

        public List<Account> Accounts { get; set; }

        public int Total;
        public int Next;
        public int Page;

        public bool EditEnabled { get; set; }
        public bool DeleteEnabled { get; set; }
    }
}
