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

        public string Id { get; set; }
        public string Name { get; set; }
        public string NameKey { get; set; }

        public ICollection<User> Users { get; private set; }
    }
}
