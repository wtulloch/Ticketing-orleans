using System;
using System.Configuration;
using System.Net;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using Grains;
using Orleans.Configuration;
using Orleans.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;
using Utils;

namespace Silo
{
    class Program
    {
        private static ISiloHost _silo;
        private static readonly ManualResetEvent SiloStopped = new ManualResetEvent(false);
        static void Main(string[] args)
        {
           
            var connectionString = ConfigurationManager.ConnectionStrings["persistence"].ConnectionString;

            _silo = new SiloHostBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = TicketingConstants.ClusterId;
                    options.ServiceId = TicketingConstants.ServiceId;
                })
                .UseAzureStorageClustering(options => options.ConnectionString = connectionString)
                .AddAzureTableGrainStorage("store1", options => options.ConnectionString = connectionString)
                .AddAzureTableGrainStorage("PubSubStore", options => options.ConnectionString = connectionString)
                .AddSimpleMessageStreamProvider(TicketingConstants.LogStreamProvider)
                .ConfigureEndpoints(siloPort: 11111, gatewayPort: 30000, listenOnAnyHostAddress: true,advertisedIP: IPAddress.Loopback)
                .ConfigureApplicationParts(parts =>
                    parts.AddApplicationPart(typeof(TicketsReserved).Assembly).WithReferences())
                .ConfigureServices(DependencyInjectionHelper.IocContainerRegistration)
                .UseDashboard(options => { options.Port = 80020; })
                .ConfigureLogging(builder => builder.SetMinimumLevel(LogLevel.Warning).AddConsole())
                .Build();

            Task.Run(StartSilo);

            AssemblyLoadContext.Default.Unloading += context =>
            {
                Task.Run(StopSilo);
                SiloStopped.WaitOne();
            };

            SiloStopped.WaitOne();
        }

        private static async Task StartSilo()
        {
            await _silo.StartAsync();
            Console.WriteLine("Silo started");
        }

        private static async Task StopSilo()
        {
            await _silo.StopAsync();
            Console.WriteLine("Silo Stopped");
            SiloStopped.Set();
        }

       
    }
}
