using System;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using Grains;
using Orleans.Configuration;
using Orleans;
using Orleans.Clustering.Kubernetes;
using Orleans.Hosting;
using Microsoft.Extensions.Logging;
using Utils;

namespace Silo
{
    static class Program
    {
        private static ISiloHost _silo;
        private static readonly ManualResetEvent SiloStopped = new ManualResetEvent(false);
        static void Main(string[] args)
        {
            
            _silo = new SiloHostBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = TicketingConstants.ClusterId;
                    options.ServiceId = TicketingConstants.ServiceId;
                })
                .UseKubeMembership(opt =>
                {
                    opt.CanCreateResources = false;
                })
                .AddMemoryGrainStorageAsDefault()
                .AddMemoryGrainStorage("store1")
                .AddMemoryGrainStorage("PubSubStore")
                .AddSimpleMessageStreamProvider(TicketingConstants.LogStreamProvider)
                .ConfigureEndpoints(new Random(1).Next(30001, 30100), new Random(1).Next(20001, 20100), listenOnAnyHostAddress: true)
                .ConfigureApplicationParts(parts =>
                    parts.AddApplicationPart(typeof(TicketsReserved).Assembly).WithReferences())
                .ConfigureServices(DependencyInjectionHelper.IocContainerRegistration)
                .UseDashboard(options => { options.Port = 8020; })
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
