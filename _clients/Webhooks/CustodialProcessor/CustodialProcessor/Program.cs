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
        private static string baseUrl { get; set; }

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

            baseUrl = configuration
                    .GetSection("Webhooks")
                    .GetSection("CoreServices")
                    .GetSection("BaseUrl").Value;


            Console.WriteLine("Custodian configured:");
            Console.WriteLine($"Interval: {intervalMilliseconds.ToString("N0")}");
            Console.WriteLine($"BaseUrl: {baseUrl}");
            Console.WriteLine("--------------------------");
            #endregion


            while (true)
            {

                #region Process tasks

                Console.WriteLine("Custodian processing tasks...");

                var uri = $"{baseUrl}/webhooks/example";
                var data = new { Requester = "Custodian", Id = "123456", Action = "CloseExpiredAccounts" };
                var json = JsonConvert.SerializeObject(data);

                var httpClient = new HttpClient();
                var response = httpClient.PostAsync(uri, new StringContent(json, Encoding.UTF8, "application/json")).Result;


                Console.WriteLine($" > { response.StatusCode }");
                Console.WriteLine("Tasks completed!");

                #endregion



                Thread.Sleep(intervalMilliseconds);
                Console.WriteLine($"Custodian sleeping for { intervalMilliseconds.ToString("N0") } milliseconds...");
            }
        }
    }
}
