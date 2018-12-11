using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Common
{
    public interface ICoreSettings
    {
        ApplicationSettings Application { get; set; }
        AzureSettings Azure { get; set; }
    }

    #region Internal Classes

    public class ApplicationSettings
    {
        public string Name { get; set; }
    }

    public class AzureSettings
    {
        public AzureSettings()
        {
            CosmosDb = new CosmosDbSettings();
            Storage = new StorageSettings();
        }

        public CosmosDbSettings CosmosDb;
        public StorageSettings Storage;
    }

    public class CosmosDbSettings
    {
        public string Name { get; set; }
        public string Key { get; set; }
    }

    public class StorageSettings
    {
        public string Name { get; set; }
        public string Key { get; set; }
    }

    #endregion
}
