using System;
using System.Configuration;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using Grains;
using Orleans.Configuration;
using Orleans.Hosting;
using Microsoft.Extensions.Logging;
using Orleans;

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
                    options.ClusterId = "Ticketing-docker";
                    options.ServiceId = "TicketingSampleApp";
                })
                .UseAzureStorageClustering(options => options.ConnectionString = connectionString)
                .AddAzureTableGrainStorage("store1", options => options.ConnectionString = connectionString)
                .ConfigureEndpoints(siloPort: 11111, gatewayPort: 30000, listenOnAnyHostAddress: true)
                .ConfigureApplicationParts(parts =>
                    parts.AddApplicationPart(typeof(TicketsReserved).Assembly).WithReferences())
                .ConfigureServices(DependencyInjectionHelper.IocContainerRegistration)
                .UseDashboard()
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
