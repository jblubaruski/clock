using Clock.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clock.Models.ClockModel
{
    /// <summary>
    /// Contains and manages the time on the clock as well as a provider that notifies subscribers of ticks.
    /// </summary>
    public class ClockData
    {
        public NotificationProvider<TimeContainer> TickTransmitter { get; } = new();

        private readonly IDispatcher Dispatcher;

        public ClockData(IDispatcher dispatcher)
        {
            ArgumentNullException.ThrowIfNull(dispatcher);

            Dispatcher = dispatcher;
        }
    }
}
