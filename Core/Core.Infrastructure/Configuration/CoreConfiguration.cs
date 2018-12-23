using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Infrastructure.Configuration
{
    /// <summary>
    /// We use the ICoreConfiguration type as a way to inject settings
    /// and resource connections into our classes from our main entry points.
    /// </summary>
    public class CoreConfiguration : ICoreConfiguration
    {
        public CoreConfiguration(IConfiguration configuration)
        {
            // New up our root classes
            Application = new ApplicationConfiguration();
            Hosting = new HostingConfiguration();


            Application.Name = configuration.GetSection("Application").GetSection("Name").Value;


            #region Hosting configuration details (if available)

            try
            {
                // Azure WebApp provides these settings when deployed.
                Hosting.SiteName = configuration["WEBSITE_SITE_NAME"];
                Hosting.InstanceId = configuration["WEBSITE_INSTANCE_ID"];
            }
            catch
            {
            }


            #endregion


            // TO BE REMOVED ------------

            Azure = new AzureConfiguration();
            // New up our CosmosDB classes
            //Azure.CosmosDb = new CosmosDbConfiguration();
            //Azure.CosmosDb.Settings = new CosmosDbSettings();

            // New up our Storage classes
            Azure.Storage = new StorageConfiguration();
            Azure.Storage.Settings = new StorageSettings();

            // New up our Redis classes
            Azure.Redis = new RedisConfiguration();
            Azure.Redis.Settings = new RedisSettings();

        }

        public ApplicationConfiguration Application { get; set; }
        public HostingConfiguration Hosting { get; set; }
        public AzureConfiguration Azure { get; set; }
    }
}
