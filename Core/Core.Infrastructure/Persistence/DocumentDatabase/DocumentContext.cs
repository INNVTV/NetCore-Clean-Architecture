using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.Configuration;

namespace Core.Infrastructure.Persistence.DocumentDatabase
{
    public class DocumentContext : IDocumentContext
    {
        public DocumentContext(IConfiguration configuration)
        {
            Settings = new Settings();

            #region Map appsettings.json

            Settings.Url = configuration
                .GetSection("Azure")
                .GetSection("CosmosDb")
                .GetSection("Url").Value;

            Settings.Key = configuration
                .GetSection("Azure")
                .GetSection("CosmosDb")
                .GetSection("Key").Value;

            Settings.ReadOnlyKey = configuration
                .GetSection("Azure")
                .GetSection("CosmosDb")
                .GetSection("ReadOnlyKey").Value;

            Settings.Database = configuration
                .GetSection("Azure")
                .GetSection("CosmosDb")
                .GetSection("Database").Value;

            Settings.Collection = configuration
                .GetSection("Azure")
                .GetSection("CosmosDb")
                .GetSection("Collection").Value;
            #endregion

            #region Generate the document client

            ConnectionPolicy _connectionPolicy = new ConnectionPolicy
            {
                // Since we are running within Azure we use Direct/TCP connections for performance.
                // Web clients hosted on Azure using ReadOnly keys should also use this.
                // External clients like mobile phones that have ReadOnly Keys should use Gateway/Https
                ConnectionMode = ConnectionMode.Direct,
                ConnectionProtocol = Protocol.Tcp,
                RetryOptions = new RetryOptions
                {
                    MaxRetryAttemptsOnThrottledRequests = 6,
                    MaxRetryWaitTimeInSeconds = 30,
                }
            };

            // Using a Singleton in your Di Container will ensure you have a DocumentClient instance always stored away for re-use.
            Client = new DocumentClient(new Uri(
                    Settings.Url),
                    Settings.Key,
                    _connectionPolicy
                    );

            #endregion
        }

        public DocumentClient Client { get; set; }
        public Settings Settings { get; set; }
    }
}
