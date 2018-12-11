using Microsoft.Azure.Documents.Client;
using Microsoft.WindowsAzure.Storage;
using StackExchange.Redis;

namespace Core.Common.Configuration
{
    public interface ICoreConfiguration
    {
        ApplicationConfiguration Application { get; set; }
        AzureConfiguration Azure { get; set; }
    }

    #region Classes

    public class ApplicationConfiguration
    {
        public string Name { get; set; }
    }

    #region Azure

    public class AzureConfiguration
    {
        public AzureConfiguration()
        {
            CosmosDb = new CosmosDbConfiguration();
            Storage = new StorageConfiguration();
            Redis = new RedisConfiguration();
        }

        public CosmosDbConfiguration CosmosDb;
        public StorageConfiguration Storage;
        public RedisConfiguration Redis;
    }

    #region Resource Types

    #region Cosmos

    public class CosmosDbConfiguration
    {
        public DocumentClient Client;
        public CosmosDbSettings Settings;
    }

    public class CosmosDbSettings
    {
        public string Url { get; set; }
        public string Key { get; set; }
        public string ReadOnlyKey { get; set; }
        public string Database { get; set; }
        public string Collection { get; set; }
    }

    #endregion

    #region Storage

    public class StorageConfiguration
    {
        public CloudStorageAccount StorageAccount;
        public StorageSettings Settings;
    }

    public class StorageSettings
    {
        public string Name { get; set; }
        public string Key { get; set; }
    }

    #endregion

    #region Redis

    public class RedisConfiguration
    {
        public ConnectionMultiplexer ConnectionMultiplexer;
        public RedisSettings Settings;
    }


    public class RedisSettings
    {
        public string Url { get; set; }
        public string Key { get; set; }
    }

    #endregion

    #endregion

    #endregion

    #endregion
}
