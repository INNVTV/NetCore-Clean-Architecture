using Core.Application.Accounts.Queries;
using Core.Common.Configuration;
using Core.Common.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using MediatR;
using Core.Application.Accounts.Commands;
using Core.Application.Accounts.Commands;

namespace ConsoleApp
{
    class Program
    {
        static async System.Threading.Tasks.Task Main(string[] args)
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

            #region Register our dependancies into our provider

            /* Default .Net Core IServiceCollection is used in this example.
             * You can switch to Autofaq, Ninject or any DI Container of your choice.
             * 
             * Autofaq allows for automatic registration of Interfaces by using "Assembly Scanning":
             *     - builder.RegisterAssemblyTypes(dataAccess)
             *         .Where(t => t.Name.EndsWith("Repository"))
             *         .AsImplementedInterfaces();
             */

            // Create our collection of injectable services
            var serviceCollection = new ServiceCollection();
            serviceCollection.AddSingleton<IConfiguration>(configuration);
            serviceCollection.AddSingleton<ICoreConfiguration>(coreConfiguration);
            serviceCollection.AddSingleton<ICoreLogger>(coreLogger);

            /* REGISTER MEDIATR for CQRS Pattern ---------------
             * 
             * MediatR will automatically search your assemblies for IRequest and IRequestHandler implementations
             * and will build up your library of commands and queries for use throught your project. */
            serviceCollection.AddMediatR();

            // Build the provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            #endregion

            // Console Debugging:

            // Get our mediator
            var mediator = serviceProvider.GetRequiredService<IMediator>();

            // =======================================
            // COMMANDS
            // =======================================

            // Build our CreateAccount Command:
            var createAccountCommand = new CreateAccountCommand()
            {
                AccountName = "Test Account",
                Email = "test@email.com",
                FirstName = "John",
                LastName = "Smith"
            };

            // Send our command to MediatR for processing...
            var createAccountResponse = mediator.Send(createAccountCommand);
            Console.WriteLine("Command results:" + createAccountResponse.Id);

            // =======================================
            // QUERIES
            // =======================================

            //  Build our GetAccountList Query:
            var accountsListQuery = new GetAccountListQuery();

            // Send our query to MediatR for processing...
            var accountList = await mediator.Send(accountsListQuery);
            Console.WriteLine("Query list results:");
            foreach(var accountViewModel in accountList)
            {
                Console.WriteLine(String.Concat("{0} ({1})", accountViewModel.Account.Name, accountViewModel.Account.Id));
            }

            Console.WriteLine();

            // Get details for first item in our list

            // Build our GetAccountDetail Query:
            var accountDetailQuery = new GetAccountDetailQuery() {
                Id = accountList[0].Account.Id
            };
            var accountDetails = await mediator.Send(accountDetailQuery);
            Console.WriteLine("Query details results:" + accountDetails.Account.Name);

            Console.ReadLine();
        }
    }
}
