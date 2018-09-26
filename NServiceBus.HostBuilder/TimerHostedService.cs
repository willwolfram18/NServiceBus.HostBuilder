using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace NServiceBus.HostBuilder
{
    public class TimerHostedService : IHostedService, IDisposable
    {
        private readonly Task<IEndpointInstance> _endpointTask;

        private Timer _timer;

        public TimerHostedService(IProvideEndpointInstance _endpointProvider)
        {
            _endpointTask = _endpointProvider.EndpointInstance;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(
                async (e) => await SendMessageOnInterval(),
                null,
                TimeSpan.Zero,
                TimeSpan.FromSeconds(5)
            );

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(System.Threading.Timeout.Infinite, 0);

            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        private async Task SendMessageOnInterval()
        {
            await (await _endpointTask).Publish(new TimerFired());
        }
    }
}