using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common.Settings
{
    public class Initialize
    {
        public static ICoreSettings InitializeCoreSettings(IConfiguration configuration)
        {
            var settings = new CoreSettings();

            //Get configuration from Docker/Compose (via .env and appsettings.json)
            //var builder = new ConfigurationBuilder()
                //.SetBasePath(Directory.GetCurrentDirectory())
                //.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                //.AddEnvironmentVariables(); //<-- Allows for Docker Env Variables

            //IConfigurationRoot configuration = builder.Build();

            #region Map appsettings.json to class properties (Showing multiple formats)

            settings.Application.Name = configuration["Application:Name"];

            settings.Azure.CosmosDb.Name = configuration.GetSection("CosmosDb").GetSection("Name").Value;
            settings.Azure.CosmosDb.Key = configuration.GetSection("CosmosDb").GetSection("Key").Value;

            settings.Azure.Storage.Name = configuration["Storage:Name"];
            settings.Azure.Storage.Key = configuration["Storage:Key"];

            #endregion

            return settings;

        }
    }
}
