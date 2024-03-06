using System;
using System.Threading.Tasks;
using TraceService.Application.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;

namespace TraceService.WebAPI
{
    public class Program
    {
        private static IConfigurationRoot _configuration;

        public static async Task Main(string[] args)
        {
            _configuration = new ConfigurationBuilder()
                .AddJsonFile($"appsettings.json", false, true)
                .AddJsonFile($"appsettings.{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}.json", true, true)
                .Build();

            IHost host = CreateHostBuilder(args).Build();
            
            using (IServiceScope scope = host.Services.CreateScope())
            {
                IStartupTask startupTask = scope.ServiceProvider.GetRequiredService<IStartupTask>();
                startupTask.Execute();
            }
            await host.RunAsync();
        }

        //private static void Initialize(IHost host)
        //{
        //    using IServiceScope scope = host.Services.CreateScope();
        //    ILogger<Program> logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
        //}
        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .UseWindowsService()
                .UseSystemd()
                .ConfigureServices((context, services) => services.Configure<KestrelServerOptions>(
                        context.Configuration.GetSection("Kestrel")))
                .UseSerilog((ctx, config) => config.ReadFrom.Configuration(ctx.Configuration))
                .ConfigureWebHostDefaults(webBuilder => webBuilder.UseStartup<Startup>());
        }
    }
}
