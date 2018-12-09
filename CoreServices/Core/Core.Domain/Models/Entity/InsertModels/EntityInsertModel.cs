using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Domain.Models.Entity.InsertModels
{
    public class EntityInsertModel
    {
        public Guid id { get; set; }
        public string Name { get; set; }
        public int Count { get; set; }
        public DateTime Created { get; set; }
        public string Email { get; set; }
        public string Code { get; set; }
    }
}
