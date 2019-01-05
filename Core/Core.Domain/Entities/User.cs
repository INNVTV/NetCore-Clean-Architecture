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

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
    }
}
