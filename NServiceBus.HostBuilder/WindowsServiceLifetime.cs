using Microsoft.Extensions.Hosting;
using System;
using System.ServiceProcess;
using System.Threading;
using System.Threading.Tasks;

namespace NServiceBus.HostBuilder
{
    internal class WindowsServiceLifetime : ServiceBase, IHostLifetime
    {
        private readonly TaskCompletionSource<object> _taskForLifetime = new TaskCompletionSource<object>();

        private readonly IApplicationLifetime _appLifetime;

        public WindowsServiceLifetime(IApplicationLifetime appLifetime)
        {
            _appLifetime = appLifetime ?? throw new ArgumentNullException(nameof(appLifetime));
        }

        public Task WaitForStartAsync(CancellationToken cancellationToken)
        {
            cancellationToken.Register(() => _taskForLifetime.TrySetCanceled());

            _appLifetime.ApplicationStopping.Register(Stop);

            new Thread(StartWindowsService).Start();

            return _taskForLifetime.Task;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            Stop();
            return Task.CompletedTask;
        }

        protected override void OnStart(string[] args)
        {
            _taskForLifetime.TrySetResult(null);

            base.OnStart(args);
        }

        protected override void OnStop()
        {
            _appLifetime.StopApplication();

            base.OnStop();
        }

        private void StartWindowsService()
        {
            try
            {
                Run(this);

                _taskForLifetime.TrySetException(new InvalidOperationException("Windows Services stopped without starting."));
            }
            catch (Exception)
            {
                _taskForLifetime.TrySetException(e);
            }
        }
    }
}