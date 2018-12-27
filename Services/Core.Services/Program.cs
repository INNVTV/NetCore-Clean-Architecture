using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Core.Common;
using Core.Infrastructure.Configuration;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Core.Services
{
    public class Program
    {
        readonly IConfiguration _configuration;
        public static ICoreConfiguration _coreConfiguration;

        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();

            //We add a constructor for Dependancy Injection of confirguration into the controller
            //_configuration = 
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
