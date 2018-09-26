using System;

namespace NServiceBus.HostBuilder
{
    public class TimerFired : IEvent
    {
        public DateTimeOffset Timestamp { get; set; } = DateTimeOffset.Now;
    }
}