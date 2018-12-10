using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Core.Services
{
    public static class Settings
    {
        #region Public Properties

        public static ApplicationSettings Application { get; set; }
        public static AzureSettings Azure { get; set; }

        #endregion

        #region Startup Method

        public static void InitializeApplicationSettings()
        {
            //Get configuration from Docker/Compose (via .env and appsettings.json)
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables(); //<-- Allows for Docker Env Variables

            IConfigurationRoot configuration = builder.Build();

            #region Map appsettings.json to class properties

            Settings.Application.Name = configuration["Application:Name"];

            Settings.Azure.CosmosDb.Name = configuration["CosmosDb:Name"];
            Settings.Azure.CosmosDb.Key = configuration["CosmosDb:Key"];

            Settings.Azure.Storage.Name = configuration["Storage:Name"];
            Settings.Azure.Storage.Key = configuration["Storage:Key"];

            #endregion

        }

        #endregion


    }


    #region Internal Classes

    internal class ApplicationSettings
    {
        public string Name { get; set; }
    }

    internal class AzureSettings
    {
        public AzureSettings()
        {
            CosmosDb = new CosmosDbSettings();
            Storage = new StorageSettings();
        }

        public CosmosDbSettings CosmosDb;
        public StorageSettings Storage;
    }

    internal class CosmosDbSettings
    {
        public string Name { get; set; }
        public string Key { get; set; }
    }

    internal class StorageSettings
    {
        public string Name { get; set; }
        public string Key { get; set; }
    }

    #endregion


}
