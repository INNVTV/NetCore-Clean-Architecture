using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Core.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InfrastructureController : ControllerBase
    {
        // GET: api/Infrastructure/instance
        [Route("Instance")]
        [HttpGet]
        public IEnumerable<string> Instance()
        {
            var ipAdresses = new List<string>();

            IPAddress[] localIPs = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress addr in localIPs)
            {
                if (addr.AddressFamily == AddressFamily.InterNetwork)
                {
                    ipAdresses.Add(addr.ToString());
                }
            }

            return ipAdresses;
        }

        
    }
}
