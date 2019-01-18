using Grpc.Core;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace CustodialProcessor
{
    class Program
    {
        private static int intervalMilliseconds = 10000;
        private static string grpcEndpoint { get; set; }

        static void Main(string[] args)
        {
            Console.WriteLine("Custodian started.");

            #region Load Configuration

            var configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) //<-- Set to 'CopyAlways' in file/solution properties
              .AddEnvironmentVariables() // (Optional) <-- Allows for Docker Env Variables
              .Build();

            int.TryParse(configuration
                    .GetSection("Scheduling")
                    .GetSection("Interval").Value,
                out intervalMilliseconds);

            grpcEndpoint = configuration
                    .GetSection("gRPC")
                    .GetSection("Endpoints")
                    .GetSection("CoreServices").Value;


            Console.WriteLine("Custodian configured:");
            Console.WriteLine($"Interval: {intervalMilliseconds.ToString("N0")}");
            Console.WriteLine($"gRPC Endpoint: {grpcEndpoint}");
            Console.WriteLine("--------------------------");

            #endregion

            int accountNumber = 0;

            var runContinuously = true;
            while (runContinuously)
            {

                #region Process tasks

                accountNumber++;
                Console.WriteLine($"Custodian creating: 'Account { accountNumber }'...");


                Channel channel = new Channel(grpcEndpoint, ChannelCredentials.Insecure);

                var createAccountRequest = new Shared.GrpcClientLibrary.CreateAccountRequest
                {
                    Name = $"Account {accountNumber}",
                    FirstName = "John",
                    LastName = "Smith",
                    Email = "kaz@innvtv.com"
                };

                var accountClient = new Shared.GrpcClientLibrary.AccountServices.AccountServicesClient(channel);

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

                //Shut down the channel
                channel.ShutdownAsync().Wait();


                Console.WriteLine("Task completed!");

                #endregion

                Thread.Sleep(intervalMilliseconds);
                Console.WriteLine($"Custodian sleeping for { intervalMilliseconds.ToString("N0") } milliseconds...");
            }
        }
    }
}
