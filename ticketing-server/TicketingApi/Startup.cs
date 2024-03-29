﻿using System;
using System.Threading.Tasks;
using Grains.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Orleans;
using Orleans.Clustering.Kubernetes;
using Orleans.Configuration;
using Orleans.Hosting;
using Utils;

namespace TicketingApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        private readonly IConfiguration _configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var createClusterClient = CreateClusterClient(services.BuildServiceProvider());
            services.AddSingleton(createClusterClient);

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fake Ticketing", Version = "V1" });
            });

            services.AddCors(options => { options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:8080", "http://127.0.0.1:8080", "http://ticketweb.localtest.me");
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });
            });
            
            services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseCors();
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Fake Ticketing V1");
                c.RoutePrefix = String.Empty;
            });

            app.UseRouting();
            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }

        private IClusterClient CreateClusterClient(IServiceProvider serviceProvider)
        {
            var log = serviceProvider.GetService<ILogger<Startup>>();

            var clientBuilder = new ClientBuilder()
                .ConfigureApplicationParts(parts => parts.AddApplicationPart((typeof(ITicketsReserved).Assembly)))
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = TicketingConstants.ClusterId;
                    options.ServiceId = TicketingConstants.ServiceId;
                })
                .ConfigureLogging(logger => logger.SetMinimumLevel(LogLevel.Error).AddConsole())
                .UseKubeGatewayListProvider();
            
            if (_configuration.GetValue<bool>("RunningInKubernetes"))
            {
                clientBuilder.UseKubeGatewayListProvider();
            }
            else
            {
                clientBuilder.UseAzureStorageClustering(options => options.ConnectionString = _configuration.GetConnectionString("OrleansStorage"));
            }

            var client = clientBuilder.Build();

            client.Connect(RetryFilter).GetAwaiter().GetResult();
            return client;

            async Task<bool> RetryFilter(Exception exception)
            {
                log?.LogWarning("Exception while attempting to connect to Orleans cluster: {Exception}", exception);
                Console.WriteLine($"Exception while attempting to connect to Orleans: {exception.Message}");
                ;
                await Task.Delay(TimeSpan.FromSeconds(3));
                return true;
            }
        }
    }
}
