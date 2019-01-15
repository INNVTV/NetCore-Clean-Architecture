using Core.Domain.Entities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Accounts.Models.Views
{
    [JsonObject(Title="AccountList")] //<-- Update name for OpenAPI/Swagger 
    public class AccountListResultsViewModel
    {
        public AccountListResultsViewModel()
        {
            Accounts = new List<AccountListViewItem>();
            HasMoreResults = false;
            ContinuationToken = null;
            EditEnabled = false;
            DeleteEnabled = false;
            Count = 0;
        }

        public List<AccountListViewItem> Accounts { get; set; }

        public bool EditEnabled { get; set; }
        public bool DeleteEnabled { get; set; }

        public int Count;
        public bool HasMoreResults;
        public string ContinuationToken { get; set; } //<-- Use for next call. Null on final
    }
}
