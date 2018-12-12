using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Core.Common.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfrastructureController : ControllerBase
    {
        readonly ICoreConfiguration coreConfiguration;


        public InfrastructureController(IServiceProvider serviceProvider)
        {
            coreConfiguration = serviceProvider.GetService<ICoreConfiguration>();
        }

        // GET: api/Infrastructure/instance
        [Route("Instance")]
        [HttpGet]
        public string Instance()
        {
            return coreConfiguration.Hosting.InstanceId;
        }

        
    }
}
