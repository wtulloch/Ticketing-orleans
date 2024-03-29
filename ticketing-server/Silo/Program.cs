﻿using System;
using System.IO;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using Grains;
using Microsoft.Extensions.Configuration;
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
        private static IConfiguration _configuration;

        static void Main(string[] args)
        {
            _configuration = BuildConfiguration();

            var siloHostBuilder = new SiloHostBuilder()
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = TicketingConstants.ClusterId;
                    options.ServiceId = TicketingConstants.ServiceId;
                })
                .AddAzureBlobGrainStorage(TicketingConstants.PubSubStorageName, options =>
                {
                    options.ConnectionString = _configuration.GetConnectionString("OrleansStorage");
                    options.UseJson = true;
                })
                .AddAzureBlobGrainStorage(TicketingConstants.StorageProviderName, options =>
                {
                    options.ConnectionString = _configuration.GetConnectionString("OrleansStorage");
                    options.UseJson = true;
                })
                .AddSimpleMessageStreamProvider(TicketingConstants.LogStreamProvider)
                .ConfigureEndpoints(new Random(1).Next(30001, 30100), new Random(1).Next(20001, 20100),
                    listenOnAnyHostAddress: true)
                .ConfigureApplicationParts(parts =>
                    parts.AddApplicationPart(typeof(TicketsReserved).Assembly).WithReferences())
                .ConfigureServices(DependencyInjectionHelper.IocContainerRegistration)
                .UseDashboard(options => { options.Port = 8020; })
                .ConfigureLogging(builder => builder.SetMinimumLevel(LogLevel.Warning).AddConsole());

            if (_configuration.GetValue<bool>("RunningInKubernetes"))
            {
                var env = Environment.GetEnvironmentVariables();
                siloHostBuilder.UseKubeMembership(opt => opt.CanCreateResources = false);
            }
            else
            {
                siloHostBuilder.UseAzureStorageClustering(options =>
                    options.ConnectionString = _configuration.GetConnectionString("OrleansStorage"));
            }

            _silo = siloHostBuilder.Build();

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

        private static IConfiguration BuildConfiguration() => new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") ?? "Development"}.json",
                optional: true, reloadOnChange: true)
            .AddEnvironmentVariables()
            .Build();
    }
}