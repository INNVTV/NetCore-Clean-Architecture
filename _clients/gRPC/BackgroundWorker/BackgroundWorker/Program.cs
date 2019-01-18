using Grpc.Core;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading;

namespace BackgroundWorker
{
    class Program
    {
        private static int intervalMin = 100;
        private static int intervalMax = 1000;
        private static int intervalStep = 10;
        private static string grpcEndpoint { get; set; }

        private static int currentInterval { get; set; }

        static void Main(string[] args)
        {
            Console.WriteLine("Worker started.");

            #region Load Configuration

            var configuration = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true) //<-- Set to 'CopyAlways' in file/solution properties
              .AddEnvironmentVariables() // (Optional) <-- Allows for Docker Env Variables
              .Build();

            int.TryParse(configuration
                    .GetSection("SchedulingBackoff")
                    .GetSection("IntervalMin").Value,
                out intervalMin);

            int.TryParse(configuration
                    .GetSection("SchedulingBackoff")
                    .GetSection("IntervalMax").Value,
                out intervalMax);

            int.TryParse(configuration
                    .GetSection("SchedulingBackoff")
                    .GetSection("IntervalStep").Value,
                out intervalStep);

            grpcEndpoint = configuration
                    .GetSection("gRPC")
                    .GetSection("Endpoints")
                    .GetSection("CoreServices").Value;


            Console.WriteLine("Worker configured:");
            Console.WriteLine($"IntervalMin: {intervalMin.ToString("N0")}");
            Console.WriteLine($"IntervalMin: {intervalMax.ToString("N0")}");
            Console.WriteLine($"gRPC Endpoint: {grpcEndpoint}");
            Console.WriteLine("--------------------------");

            #endregion

            currentInterval = intervalMin;

            var runContinuously = true;
            while (runContinuously)
            {

                ReadAndProcessMessages();

                Thread.Sleep(currentInterval);
                Console.WriteLine($"Worker sleeping for { currentInterval.ToString("N0") } milliseconds...");
            }
        }

        private static void ReadAndProcessMessages()
        {
            // Read next message from queue
            //ReadMessage();
            object nextMessage = new { id= "123" };

            if(nextMessage != null)
            {
                //reset our backoff strategy
                currentInterval = intervalMin;

                #region Process tasks

                Console.WriteLine("Worker processing tasks...");

                Channel channel = new Channel(grpcEndpoint, ChannelCredentials.Insecure);

                var workerTaskRequest = new Shared.GrpcClientLibrary.WorkerTaskRequest
                {
                    Id = 12345678
                };

                var client = new Shared.GrpcClientLibrary.BackgroundServices.BackgroundServicesClient(channel);

                var response = client.WorkerTask(workerTaskRequest);

                Console.WriteLine($"Respnse: { response }.");

                //Shut down the channel
                channel.ShutdownAsync().Wait();
                Console.WriteLine("Tasks completed! gRPC channel shutdown.");

                #endregion

                // Delete message from queue or send issues/exceptions to log for retry or manual processing
            }
            else
            {
                // Increase backoff (up to max) until we start getting messages
                if(currentInterval < intervalMax)
                {
                    currentInterval = currentInterval + intervalStep;
                }
                else
                {
                    currentInterval = intervalMax;
                }
                
            }


        }
    }
}
