using Microsoft.Azure.Documents.Client;
using Microsoft.WindowsAzure.Storage;
using StackExchange.Redis;

namespace Core.Common.Configuration
{
    public interface ICoreConfiguration
    {
        ApplicationConfiguration Application { get; set; }
        HostingConfiguration Hosting { get; set; }
        AzureConfiguration Azure { get; set; }
    }

    #region Classes

    #region Application

    public class ApplicationConfiguration
    {
        public string Name { get; set; }
        public ApplicationSettings Settings { get; set; }
    }

    public class ApplicationSettings
    {
        public string Url { get; set; }
    }

    #endregion

    #region Hosting

    /// <summary>
    /// Only used in Azure WebApp hosted deployments.
    /// Returns info on the WebApp instance for the current process. 
    /// Can be used to log which WebApp instance a process ran on.
    /// </summary>
    public class HostingConfiguration
    {

        public string SiteName { get; set; }
        public string InstanceId { get; set; }
    }

    #endregion

    #region Azure

    public class AzureConfiguration
    {
        public AzureConfiguration()
        {
            //CosmosDb = new CosmosDbConfiguration();  <-- Moved to Persistence.IDocumentClient
            Storage = new StorageConfiguration();
            Redis = new RedisConfiguration();
        }

        //public CosmosDbConfiguration CosmosDb;  <-- Moved to Persistence.IDocumentClient
        public StorageConfiguration Storage;
        public RedisConfiguration Redis;
    }

    #region Resource Types

    #region Cosmos (Moved to: Persistence.IDocumentClient)
    /*
    public class CosmosDbConfiguration
    {
        //public DocumentClient Client; <-- Moved to Persistence.IDocumentClient
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
    */
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
