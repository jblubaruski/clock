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
        /// <summary>
        /// The provider that notifies subscribing observers of each tick of the clock.
        /// </summary>
        public NotificationProvider<TimeContainer> TickTransmitter { get; } = new();

        // the dispatcher that ticks the clock every TickInterval
        private readonly IDispatcher Dispatcher;
        // the span of time between ticks
        private readonly TimeSpan TickInterval;

        // the time the clock was started at
        private DateTime StartTime;
        // the number of ticks since the clock was started
        private long TicksSinceStart = 0;
        // true if the clock is currently running
        public bool IsClockRunning { get; private set; } = false;

        /// <summary>
        /// Constructs a new ClockData.
        /// </summary>
        /// 
        /// <param name="dispatcher">
        /// The dispatcher that controls the clock's ticking.
        /// </param>
        /// <param name="tickInterval">
        /// The span of time between ticks
        /// </param>
        public ClockData(IDispatcher dispatcher, TimeSpan tickInterval)
        {
            ArgumentNullException.ThrowIfNull(dispatcher);

            Dispatcher = dispatcher;
            TickInterval = tickInterval;
        }

        /// <summary>
        /// Starts the clock at the passed in time.
        /// </summary>
        /// 
        /// <param name="startTime">
        /// The clock's start time.
        /// </param>
        public void StartClock(DateTime startTime)
        {
            StartTime = startTime;
            TicksSinceStart = 0;
            IsClockRunning = true;
            Dispatcher.StartTimer(TickInterval, TickClock);
        }

        /// <summary>
        /// Stops the clock from ticking.
        /// </summary>
        /// 
        /// <param name="completeClockNotifications">
        /// Tells the clock to notify all subscribers to its provider that it's done with its notifications if true.
        /// This unsubscribes all subscribers from the clock's provider.
        /// </param>
        public void StopClock(bool completeClockNotifications)
        {
            IsClockRunning = false;

            if (completeClockNotifications)
                TickTransmitter.CompleteAll();
        }

        /*
         * TODO
         * Edit TickClock() so it runs asynchronously and uses a while loop instead of a dispatcher.
         */

        // ticks the clock once and returns whether or not the clock is currently running
        private bool TickClock()
        {
            if (IsClockRunning)
            {
                try
                {
                    TicksSinceStart += 1;
                }
                catch (OverflowException ex)
                {
                    TickTransmitter.ErrorAll(ex);
                    StopClock(false);
                    return false;
                }

                TimeContainer currentTime = new(StartTime.Add(TickInterval.Multiply(TicksSinceStart)));
                TickTransmitter.NotifyAll(currentTime);
            }

            return IsClockRunning;
        }
    }
}
