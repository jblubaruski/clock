using Clock.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clock.Models.ClockModel
{
    // contains the time data for the clock and ticks on a regular interval
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
