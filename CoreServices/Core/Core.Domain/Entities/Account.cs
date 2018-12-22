using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain.Entities
{
    public class Account
    {
        public Account()
        {
            Users = new List<User>();
        }

        public string AccountId { get; set; }
        public string AccountName { get; set; }
        public string AccountNameKey { get; set; }

        public ICollection<User> Users { get; private set; }
    }
}
