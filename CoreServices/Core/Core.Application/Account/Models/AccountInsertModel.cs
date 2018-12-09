using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Application.Account.Models
{
    public class AccountInsertModel
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public DateTime Created { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
