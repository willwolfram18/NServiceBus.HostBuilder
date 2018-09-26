using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace NServiceBus.HostBuilder
{
    class Program
    {
        private static string Directory => AppDomain.CurrentDomain.BaseDirectory;

        static async Task Main(string[] args)
        {
            var hostBuilder = new Microsoft.Extensions.Hosting.HostBuilder()
                                .ConfigureHostConfiguration(config =>
                                {
                                    config.SetBasePath(Directory)
                                        .AddEnvironmentVariables(prefix: "ASPNETCORE_");
                                })
                                .ConfigureAppConfiguration(config =>
                                {
                                    config.SetBasePath(Directory)
                                        .AddEnvironmentVariables(prefix: "ASPNETCORE_")
                                        .AddJsonFile("appsettings.json");
                                })
                                .ConfigureServices(services =>
                                {
                                    services.AddHostedService<NServiceBusHostedService>()
                                        .AddHostedService<TimerHostedService>()
                                        .AddSingleton<IProvideEndpointInstance, EndpointInstanceProvider>();
                                });

            if (Debugger.IsAttached || args.Contains("--console"))
            {
                await hostBuilder.RunConsoleAsync();
            }
            else
            {
                await hostBuilder.RunAsWindowsServiceAsync();
            }
        }
    }
}
