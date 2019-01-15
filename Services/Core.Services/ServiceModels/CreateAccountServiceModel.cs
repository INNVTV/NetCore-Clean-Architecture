using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Services.ServiceModels
{
    [JsonObject(Title = "CreateAccount")] //<-- Update name for OpenAPI/Swagger
    public class CreateAccountServiceModel
    {
        public string Name { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
    }
}
