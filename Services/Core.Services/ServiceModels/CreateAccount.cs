using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Services.ServiceModels
{
    //[DataContract]
    public class CreateAccountServiceModel
    {
        //[DataMember(AccountName = "accountName", EmitDefaultValue = false)]
        //[JsonProperty(PropertyName = "accountName")]
        public string AccountName { get; set; }

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
