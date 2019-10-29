using System;
using System.Threading.Tasks;
using Grains.Interfaces;
using Microsoft.Extensions.Logging;
using Orleans;
using Orleans.Configuration;
using Orleans.Clustering.Kubernetes;
using Orleans.Hosting;
using Ticketing.Models;
using Utils;
using System.IO;
using Microsoft.Extensions.Configuration;

namespace Admin.View
{
    static class Program
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

        private static IConfiguration _configuration;

        private static async Task<IClusterClient> InitialiseClient()
        {
            _configuration = BuildConfiguration();
            
            int initialiseCounter = 0;

            var initSucceeded = false;

            while (!initSucceeded)
            {
                try
                {
                    var clientBuilder = new ClientBuilder()
                        .Configure<ClusterOptions>(options =>
                        {
                            options.ClusterId = TicketingConstants.ClusterId;
                            options.ServiceId = TicketingConstants.ServiceId;
                        })
                        .ConfigureApplicationParts(parts =>
                            parts.AddApplicationPart(typeof(IMessageBatch).Assembly).WithReferences())
                        .UseKubeGatewayListProvider()
                        .ConfigureLogging(logging => logging.SetMinimumLevel(LogLevel.Warning).AddConsole())
                        .AddSimpleMessageStreamProvider(TicketingConstants.LogStreamProvider);
                    
                    if (_configuration.GetValue<bool>("RunningInKubernetes"))
                    {
                        clientBuilder.UseKubeGatewayListProvider();
                    }
                    else
                    {
                        clientBuilder.UseAzureStorageClustering(options => options.ConnectionString = _configuration.GetConnectionString("OrleansStorage"));
                    }
                    
                    var client = clientBuilder.Build();
                        

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
        
        private static IConfiguration BuildConfiguration() => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json",
                optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
    }
}
