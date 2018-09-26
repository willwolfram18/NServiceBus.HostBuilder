using System;
using System.Threading.Tasks;

namespace NServiceBus.HostBuilder
{
    public class Handler : IHandleMessages<TimerFired>
    {
        public Task Handle(TimerFired message, IMessageHandlerContext context)
        {
            Console.WriteLine($"Timer fired at {message.Timestamp}");

            return Task.CompletedTask;
        }
    }
}