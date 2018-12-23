using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;
using StackExchange.Redis;

namespace Core.Infrastructure.Persistence.RedisCache
{
    public class RedisContext : IRedisContext
    {
        public RedisContext(IConfiguration configuration)
        {
            Settings = new RedisSettings();

            #region Map appsettings.json

            Settings.Url = configuration
                .GetSection("Azure")
                .GetSection("Redis")
                .GetSection("Url").Value;

            Settings.Key = configuration
                .GetSection("Azure")
                .GetSection("Redis")
                .GetSection("Key").Value;

            #endregion

            #region Create Redis Multiplexers

            // Because the ConnectionMultiplexer does a lot, it is designed to be shared and reused between callers.
            // You should not create a ConnectionMultiplexer per operation. It is fully thread-safe.
            // Using a Singlton DI Container Instance ensures you have a ConnectionMultiplexer instance always stored away for re-use.

            // Connection String
            StringBuilder redisConnectionString = new StringBuilder();
            redisConnectionString.Append(Settings.Url);
            redisConnectionString.Append(", ssl=false, password=");
            redisConnectionString.Append(Settings.Key);

            // Configuration
            var redisConfiguration = ConfigurationOptions.Parse(redisConnectionString.ToString());
            redisConfiguration.AllowAdmin = true;

            // Once configured and injected into your DI Container this multiplexer should not need to be configured again
            ConnectionMultiplexer = ConnectionMultiplexer.Connect(redisConfiguration);


            #endregion
        }

        public ConnectionMultiplexer ConnectionMultiplexer { get; set; }
        public RedisSettings Settings { get; set; }
    }
}
