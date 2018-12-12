using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common.Configuration
{
    /// <summary>
    /// We use the ICoreConfiguration type as a way to inject settings
    /// and resource connections into our classes from our main entry points.
    /// </summary>
    public class CoreConfiguration : ICoreConfiguration
    {
        public CoreConfiguration()
        {
            // New up our root classes
            Application = new ApplicationConfiguration();
            Azure = new AzureConfiguration();

            // New up our CosmosDB classes
            Azure.CosmosDb = new CosmosDbConfiguration();
            Azure.CosmosDb.Settings = new CosmosDbSettings();

            // New up our Storage classes
            Azure.Storage = new StorageConfiguration();
            Azure.Storage.Settings = new StorageSettings();

            // New up our Redis classes
            Azure.Redis = new RedisConfiguration();
            Azure.Redis.Settings = new RedisSettings();

        }

        public ApplicationConfiguration Application { get; set; }
        public AzureConfiguration Azure { get; set; }
    }
}
