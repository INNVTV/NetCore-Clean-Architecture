using Core.Application.Accounts.Queries;
using Core.Common.Logging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.EnvironmentVariables;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;
using MediatR;
using Core.Application.Accounts.Commands;
using Core.Infrastructure.Configuration;
using Core.Infrastructure.Persistence.DocumentDatabase;
using Core.Infrastructure.Persistence.StorageAccount;
using Core.Infrastructure.Persistence.RedisCache;
using Core.Domain.Entities;
using Core.Infrastructure.Services.Email;
using Core.Infrastructure.Pipeline;

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

            ICoreConfiguration coreConfiguration = new CoreConfiguration(configuration);

            #endregion

            #region Initialize our ICoreLogger

            ICoreLogger coreLogger = new CoreLogger();

            #endregion

            #region Initialize our Persistence Layer objects

            IDocumentContext documentContext = new DocumentContext(configuration);
            IStorageContext storageContext = new StorageContext(configuration);
            IRedisContext redisContext = new RedisContext(configuration);

            #endregion

            #region Initialize 3rd Party Service Dependencies

            IEmailService sendgridService = new SendGridEmailService(configuration);

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

            // Configuration
            serviceCollection.AddSingleton<IConfiguration>(configuration);
            serviceCollection.AddSingleton<ICoreConfiguration>(coreConfiguration);

            // Persistence
            serviceCollection.AddSingleton<IDocumentContext>(documentContext);
            serviceCollection.AddSingleton<IStorageContext>(storageContext);
            serviceCollection.AddSingleton<IRedisContext>(redisContext);

            // 3rd Part Services
            serviceCollection.AddSingleton<IEmailService>(sendgridService);

            // Logging
            //serviceCollection.AddSingleton<ICoreLogger>(coreLogger);

            // MediatR Pipeline Behaviors
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(TracingBehavior<,>));
            serviceCollection.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));


            /* -----------------------------------------------------
             * REGISTER MEDIATR for CQRS Pattern 
             * ------------------------------------------------------
             * MediatR will automatically search your assemblies for IRequest and IRequestHandler implementations
             * and will build up your library of commands and queries for use throught your project.
             * 
             * Note: MediatR should be added LAST. */

            serviceCollection.AddMediatR();

            // Build the provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            #endregion

            // Initialize Core.Startup
            Core.Startup.Routines.Initialize();

            // Console Debugging:

            // Get our mediator
            var mediator = serviceProvider.GetRequiredService<IMediator>();

            // =======================================
            // TEST BED FOR COMMANDS/QUERIES
            // =======================================

            #region CREATE ACCOUNT

            // Build our CreateAccount Command:
            var createAccountCommand = new CreateAccountCommand()
            {
                Name = "Account Name 5",
                Email = "kaz@innvtv.com",
                FirstName = "John",
                LastName = "Smith"
            };

            // Send our command to MediatR for processing...
            var createAccountResponse = mediator.Send(createAccountCommand);

            // Print results:
            Console.WriteLine("RESULTS:");
            Console.WriteLine(createAccountResponse.Result.isSuccess);
            Console.WriteLine(createAccountResponse.Result.Message);

            if (createAccountResponse.Result.isSuccess)
            {
                var account = (Account)createAccountResponse.Result.Object;

                Console.WriteLine("----------------------------------------");
                Console.WriteLine("NEW ACCOUNT:");
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("Id: " + account.Id);
                Console.WriteLine("NameKey: " + account.NameKey);
                Console.WriteLine("----------------------------------------");
            }

            Console.ReadLine();
          
            #endregion

            #region CREATE BATCH OF ACCOUNTS
            /*
            int amount = 1;

            for(var i = 1; i <= amount; i++)
            {
                // Build our CreateAccount Command:
                var createAccountCommand = new CreateAccountCommand()
                {
                    Name = String.Concat("Account ", i),
                    Email = "kaz@innvtv.com",
                    FirstName = "John",
                    LastName = "Smith"
                };

                // Send our command to MediatR for processing...
                var createAccountResponse = mediator.Send(createAccountCommand);

                if (createAccountResponse.Result.isSuccess)
                {
                    var account = (Account)createAccountResponse.Result.Object;
                    Console.WriteLine(String.Concat(account.Name, " created."));
                }
            }
            
            Console.ReadLine();
            */
            #endregion

            #region GET ACCOUNT LIST

            // Order By
            // Count
            // Pagination

            #endregion

            #region CLOSE ACCOUNT

            /*
            // Build our CreateAccount Command:
            var closeAccountCommand = new CloseAccountCommand()
            {
                Id = "724a70f2-594d-48f8-b623-956f2f94fb61"
            };

            // Send our command to MediatR for processing...
            var closeAccountResponse = mediator.Send(closeAccountCommand);

            // Print results:
            Console.WriteLine("RESULTS:");
            Console.WriteLine(closeAccountResponse.Result.isSuccess);
            Console.WriteLine(closeAccountResponse.Result.Message);

            if (closeAccountResponse.Result.isSuccess)
            {
                Console.WriteLine("----------------------------------------");
                Console.WriteLine("ACCOUNT CLOSED");
                Console.WriteLine("----------------------------------------");
            }

            Console.ReadLine();
            
            */
            #endregion

        }


    }
}
