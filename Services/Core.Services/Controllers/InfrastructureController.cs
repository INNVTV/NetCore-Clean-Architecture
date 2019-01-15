using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Core.Infrastructure.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Services.Controllers
{
    [ApiExplorerSettings(IgnoreApi = true)] //<-- We do not publish our Infrastructure Methods to our OpenAPI/Swagger documentation
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
        public HostingConfiguration Instance()
        {
            // Returns info on the WebApp instance for the current process
            return coreConfiguration.Hosting;
        }

        
    }
}
