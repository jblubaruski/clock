using Clock.Models.Notifications;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clock.Models.ClockModel
{
    /// <summary>
    /// Contains and manages the time on the clock as well as a provider that notifies subscribers of ticks.
    /// </summary>
    /// 
    /// <param name="tickInterval">
    /// The span of time between ticks
    /// </param>
    public class ClockData
    {
        /// <summary>
        /// The provider that notifies subscribing observers of each tick of the clock.
        /// </summary>
        public NotificationProvider<TimeContainer> TickTransmitter { get; } = new();

        // the span of time between ticks
        private readonly TimeSpan TickInterval;

        // the time the clock was started at
        private DateTime StartTime;
        // the number of ticks since the clock was started
        private long TicksSinceStart = 0;
        private CancellationTokenSource CancellationSource;
        private CancellationToken CancellationToken;

        /// <summary>
        /// Constructs a ClockData instance.
        /// </summary>
        /// 
        /// <param name="tickInterval">
        /// The amount of time between ticks.
        /// </param>
        public ClockData(TimeSpan tickInterval)
        {
            TickInterval = tickInterval;

            CancellationSource = new();
            CancellationToken = CancellationSource.Token;
        }

        /// <summary>
        /// Starts the clock at the passed in time.
        /// </summary>
        /// 
        /// <param name="startTime">
        /// The clock's start time.
        /// </param>
        public Task StartClock(DateTime startTime)
        {
            return Task.Run(async () =>
            {
                StartTime = startTime;
                TicksSinceStart = 0;

                using PeriodicTimer timer = new(TickInterval);

                try
                {
                    while (await timer.WaitForNextTickAsync(CancellationToken).ConfigureAwait(false))
                    {
                        TickClock();
                    }
                }
                catch (OperationCanceledException) 
                {
                    CancellationSource = new();
                    CancellationToken = CancellationSource.Token;
                }
            }, CancellationToken);
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
            CancellationSource.Cancel();

            if (completeClockNotifications)
                TickTransmitter.CompleteAll();
        }

        // ticks the clock once every TickInterval until the clock is stopped
        private void TickClock()
        {
            try
            {
                TicksSinceStart += 1;
            }
            catch (OverflowException ex)
            {
                TickTransmitter.ErrorAll(ex);
                StopClock(false);
            }

            TimeContainer currentTime = new(StartTime.Add(TickInterval.Multiply(TicksSinceStart)));
            TickTransmitter.NotifyAll(currentTime);
        }
    }
}
