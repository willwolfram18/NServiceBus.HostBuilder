using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace NServiceBus.HostBuilder
{
    public static class HostBuilderWindowsServiceExtensions
    {
        private static IHostBuilder UseWindowsServiceLifetime(this IHostBuilder builder)
        {
            return builder.ConfigureServices(services => services.AddSingleton<IHostLifetime, WindowsServiceLifetime>());
        }

        public static Task RunAsWindowsServiceAsync(this IHostBuilder builder)
        {
            return builder.UseWindowsServiceLifetime().Build().RunAsync();
        }
    }
}