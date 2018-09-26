using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using NServiceBus.Logging;

namespace NServiceBus.HostBuilder
{
    public class NServiceBusHostedService : IHostedService
    {
        private IEndpointInstance _endpoint;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var endpointConfig = new EndpointConfiguration("ExampleEndpoint");

            endpointConfig.UsePersistence<LearningPersistence>();
            endpointConfig.UseTransport<LearningTransport>();

            _endpoint = await Endpoint.Start(endpointConfig);
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            if (_endpoint == null)
            {
                return;
            }

            await _endpoint.Stop();
        }
    }
}