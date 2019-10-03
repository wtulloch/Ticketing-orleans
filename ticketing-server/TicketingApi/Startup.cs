using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grains.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Orleans;
using Orleans.Configuration;
using Orleans.Hosting;
using Utils;
using IHostingEnvironment = Microsoft.AspNetCore.Hosting.IHostingEnvironment;

namespace TicketingApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var createClusterClient = CreateClusterClient(services.BuildServiceProvider());

            services.AddSingleton<IClusterClient>(createClusterClient);
           
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Fake Ticketing", Version = "V1" });
            });

            services.AddCors(options => { options.AddDefaultPolicy(builder =>
                {
                    builder.WithOrigins("http://localhost:8080", "http://127.0.0.1:8080");
                    builder.AllowAnyHeader();
                    builder.AllowAnyMethod();
                });
            });

            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
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
                c.RoutePrefix = string.Empty;
            }); 

            app.UseMvc();
            app.UseDefaultFiles();
            app.UseStaticFiles();
        }

        private IClusterClient CreateClusterClient(IServiceProvider serviceProvider)
        {
            var log = serviceProvider.GetService<ILogger<Startup>>();

            var connectionString = Configuration["LocalStorage"];

            var client = new ClientBuilder()
                .ConfigureApplicationParts(parts => parts.AddApplicationPart((typeof(ITicketsReserved).Assembly)))
                .Configure<ClusterOptions>(options =>
                {
                    options.ClusterId = TicketingConstants.ClusterId;
                    options.ServiceId = TicketingConstants.ServiceId;
                })
                .ConfigureLogging(logger => logger.AddConsole())
                .UseAzureStorageClustering(options => options.ConnectionString = connectionString)
                .Build();

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
