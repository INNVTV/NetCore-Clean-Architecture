using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain.Entities
{
    public class User
    {
        public User()
        {
        }

        public string UserId { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
    }
}
