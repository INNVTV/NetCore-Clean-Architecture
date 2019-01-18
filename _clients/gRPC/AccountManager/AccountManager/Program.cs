using Grpc.Core;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace AccountManager
{
    class Program
    {
        private static string grpcEndpoint { get; set; }

        static void Main(string[] args)
        {
            Console.WriteLine("AccountManager started.");

            #region Load Configuration

            var configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) //<-- Set to 'CopyAlways' in file/solution properties
              .AddEnvironmentVariables() // (Optional) <-- Allows for Docker Env Variables
              .Build();

            grpcEndpoint = configuration
                    .GetSection("gRPC")
                    .GetSection("Endpoints")
                    .GetSection("CoreServices").Value;


            Console.WriteLine("AccountManager configured:");
            Console.WriteLine($"gRPC Endpoint: {grpcEndpoint}");
            Console.WriteLine("--------------------------");

            #endregion

            Console.WriteLine("Press any key to create accounts...");
            Console.ReadKey();

            int totalAccounts = 10;

            Channel channel = new Channel(grpcEndpoint, ChannelCredentials.Insecure);
            var accountClient = new Shared.GrpcClientLibrary.AccountServices.AccountServicesClient(channel);

            #region Create Accounts

            for (int i = 1; i <= totalAccounts; i++)
            {
                Console.WriteLine($"Creating: 'Account { i }'...");

                var createAccountRequest = new Shared.GrpcClientLibrary.CreateAccountRequest
                {
                    Name = $"Account {i}",
                    FirstName = "John",
                    LastName = "Smith",
                    Email = "test@email.com"
                };

                var createAccountResponse = accountClient.CreateAccount(createAccountRequest);

                if (createAccountResponse.IsSuccess)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"Message: { createAccountResponse.Message }");
                    Console.ForegroundColor = ConsoleColor.White;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"Message: { createAccountResponse.Message }");
                    Console.ForegroundColor = ConsoleColor.White;
                }

            }

            #endregion

            Console.WriteLine();
            Console.WriteLine("Account(s) creation complete!");
            Console.WriteLine("Press any key to get account list...");
            Console.ReadKey();

            #region Get Account List


            var getAccountListRequest = new Shared.GrpcClientLibrary.GetAccountListRequest
            {
                PageSize = totalAccounts,
                OrderBy = Shared.GrpcClientLibrary.GetAccountListRequest.Types.OrderBy.Namekey,
                OrderDirection = Shared.GrpcClientLibrary.GetAccountListRequest.Types.OrderDirection.Desc//,
                //ContinuationToken = "123"
            };

            var getAccountListResponse = accountClient.GetAccountList(getAccountListRequest);

            Console.WriteLine($"Found { getAccountListResponse.Count } accounts.");

            if (getAccountListResponse.Count > 0)
            {

                foreach (var account in getAccountListResponse.Accounts)
                {
                    Console.WriteLine($"Account: { account.Name } ({account.Id}).");
                }
            }

            #endregion

            Console.WriteLine();
            Console.WriteLine("Account list retrieved!");
            Console.WriteLine("Press any key to close accounts...");
            Console.ReadKey();

            #region Close Accounts

            if (getAccountListResponse.Count > 0)
            {
                Console.WriteLine("Closing all accounts retrieved...");

                foreach (var account in getAccountListResponse.Accounts)
                {
                    Console.WriteLine($"Deleting: { account.Name } ({account.Id}).");

                    var closeAccountResult = accountClient.CloseAccount(new Shared.GrpcClientLibrary.CloseAccountRequest { Id = account.Id });

                    if (closeAccountResult.IsSuccess)
                    {
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine($"Message: { closeAccountResult.Message }");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine($"Message: { closeAccountResult.Message }");
                        Console.ForegroundColor = ConsoleColor.White;
                    }
                }
            }

            #endregion

            //Shut down the channel
            channel.ShutdownAsync().Wait();
            Console.WriteLine();
            Console.WriteLine("Account tasks completed! gRPC channel shutdown.");

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
