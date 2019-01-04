using Core.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Accounts.Models.Views
{
    public class AccountSearchResultsViewModel
    {
        public AccountSearchResultsViewModel()
        {
            Accounts = new List<AccountListViewItem>();
            EditEnabled = false;
            DeleteEnabled = false;

        }

        public List<AccountListViewItem> Accounts { get; set; }

        public int TotalResults;
        public int NextAmount;
        public int Page;
        public int PageSize;

        public bool EditEnabled { get; set; }
        public bool DeleteEnabled { get; set; }
    }
}
