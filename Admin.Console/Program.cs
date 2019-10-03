using System;
using System.Configuration;
using System.Threading.Tasks;
using Grains.Interfaces;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Ticketing.Models;
using Utils;

namespace Admin.View
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var client = await InitialiseClient();
            if (client == null)
            {
                Console.WriteLine("failed to get client");
                Console.ReadKey();
                return;
            }

            var stream = client.GetStreamProvider(TicketingConstants.LogStreamProvider)
                .GetStream<ShowTicketLogMessage>(Guid.Empty, TicketingConstants.LogStreamNamespace);

           var subHandle = await stream.SubscribeAsync(new StreamObserver());

           Console.ReadKey();
           await subHandle.UnsubscribeAsync();





        }

        private static async Task<IClusterClient> InitialiseClient()
        {
            int initialiseCounter = 0;

            var initSucceeded = false;

            while (!initSucceeded)
            {
                try
                {
                    var connectionString = ConfigurationManager.ConnectionStrings["OrleansStorage"].ConnectionString;
                    var client = new ClientBuilder()
                        .Configure<ClusterOptions>(options =>
                        {
                            options.ClusterId = TicketingConstants.ClusterId;
                            options.ServiceId = TicketingConstants.ServiceId;
                        })
                        .ConfigureApplicationParts(parts => parts.AddApplicationPart(typeof(IMessageBatch).Assembly).WithReferences())
                        .UseAzureStorageClustering(options => options.ConnectionString = connectionString)
                        .ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Warning).AddConsole())
                        .AddSimpleMessageStreamProvider(TicketingConstants.LogStreamProvider)
                        .Build();

                    await client.Connect();
                    initSucceeded = client.IsInitialized;
                    if (initSucceeded)
                    {
                        return client;
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                    initSucceeded = false;
                }

                if (initialiseCounter++ > 10)
                {
                    return null;
                }

                Console.WriteLine("stilling trying to connect");
                await Task.Delay(TimeSpan.FromSeconds(5));
            }

            return null;
        }
    }
}
