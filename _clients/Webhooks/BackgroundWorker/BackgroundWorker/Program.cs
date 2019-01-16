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
        private static string baseUrl { get; set; }

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

            baseUrl = configuration
                    .GetSection("Webhooks")
                    .GetSection("CoreServices")
                    .GetSection("BaseUrl").Value;


            Console.WriteLine("Worker configured:");
            Console.WriteLine($"IntervalMin: {intervalMin.ToString("N0")}");
            Console.WriteLine($"IntervalMin: {intervalMax.ToString("N0")}");
            Console.WriteLine($"BaseUrl: {baseUrl}");
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

                var uri = $"{baseUrl}/webhooks/example";
                var data = new { Requester = "Worker", Id = "123456", Action = "SendAccountWarningMessage" };
                var json = JsonConvert.SerializeObject(data);

                var httpClient = new HttpClient();
                var response = httpClient.PostAsync(uri, new StringContent(json, Encoding.UTF8, "application/json")).Result;


                Console.WriteLine($" > { response.StatusCode }");
                Console.WriteLine("Tasks completed!");

                #endregion

                //Delete message from queue or send issues/exceptions to log for retry or manual processing
            }
            else
            {
                // increase backoff (up to max) until we start getting messages
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
