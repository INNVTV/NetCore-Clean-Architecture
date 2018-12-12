using Core.Application.Account.Queries;
using Core.Common.Configuration;
using Core.Common.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Create our dependancies

            #region Build our IConfiguration

            var configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) //<-- Copy from Core.Services project (or create one specific to this entry point) and set to 'CopyAlways' in file/solution properties
               //.AddEnvironmentVariables() // (Optional) <-- Allows for Docker Env Variables
              .Build();

            #endregion

            #region Initialize our ICoreConfiguration object

            ICoreConfiguration coreConfiguration;
            coreConfiguration = Core.Common.Configuration.Initialize.InitializeCoreConfiguration(configuration);

            #endregion

            #region Initialize our ICoreLogger

            ICoreLogger coreLogger = new CoreLogger();

            #endregion

            #endregion

            #region Inject our dependancies into our provider

            // Create our collection of injectable services
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IConfiguration>(configuration);
            serviceCollection.AddSingleton<ICoreConfiguration>(coreConfiguration);
            serviceCollection.AddSingleton<ICoreLogger>(coreLogger);

            // Build the provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            #endregion

            var accountsListQuery = new GetAccountsListQuery(serviceProvider);
            var appName =  accountsListQuery.GetAccountsListQueryName();

            Console.WriteLine(appName);
            Console.ReadLine();
        }
    }
}
