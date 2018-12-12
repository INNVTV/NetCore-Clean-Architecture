using Core.Application.Account.Queries;
using Core.Common.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using System;
using System.IO;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Build our configuration

            var configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) //<-- Copy from Core.Services project and set to 'CopyAlways' in file/solution properties
               //.AddEnvironmentVariables() // (Optional) <-- Allows for Docker Env Variables
              .Build();

            #endregion

            #region Initialize our ICoreConfiguration object

            ICoreConfiguration _coreConfiguration;
            _coreConfiguration = Core.Common.Configuration.Initialize.InitializeCoreConfiguration(configuration);

            #endregion

            var accountsListQuery = new GetAccountsListQuery(_coreConfiguration);
            var appName =  accountsListQuery.GetAccountsListQueryName();

            Console.WriteLine(appName);
            Console.ReadLine();
        }
    }
}
