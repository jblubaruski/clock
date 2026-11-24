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
        /// <summary>
        /// The current mode this clock is set to.
        /// </summary>
        public ClockMode CurrentMode { get; private set; } = ClockMode.CustomTime;

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
        /// The clock's start time. Will be ignored is ClockMode.SystemTime is passed in for currentMode.
        /// </param>
        /// <param name="currentMode">
        /// The ClockMode this clock is running in.
        /// </param>
        public Task StartClock(DateTime startTime, ClockMode currentMode)
        {
            CurrentMode = currentMode;

            if (CurrentMode == ClockMode.SystemTime)
                return StartSystemTimeClock();
            else if (CurrentMode == ClockMode.CustomTime)
                return StartCustomTimeClock(startTime);
            else
                return Task.Run(() => { });
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

        // starts a clock running in the SystemTime mode
        private Task StartSystemTimeClock()
        {
            StartTime = DateTime.Now;

            return Task.Run(async () =>
            {
                try
                {
                    while (!CancellationToken.IsCancellationRequested)
                    {
                        int timeUntilNextSecond_ms = (int)TimeSpan.MillisecondsPerSecond - DateTime.UtcNow.Millisecond;
                        await Task.Delay(timeUntilNextSecond_ms, CancellationToken).ConfigureAwait(false);

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

        // starts a clock running in the CustomTime mode
        private Task StartCustomTimeClock(DateTime startTime)
        {
            StartTime = startTime;
            TicksSinceStart = 0;

            return Task.Run(async () =>
            {
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

        // ticks the clock once every TickInterval until the clock is stopped
        private void TickClock()
        {
            if (CurrentMode == ClockMode.SystemTime)
            {
                DateTime currentTime = DateTime.Now;

                // round the nearest second
                DateTime roundedTime = currentTime.AddMilliseconds(500);
                roundedTime = new(roundedTime.Year, roundedTime.Month, roundedTime.Day, roundedTime.Hour, roundedTime.Minute, roundedTime.Second, DateTimeKind.Local);

                TickTransmitter.NotifyAll(new(roundedTime));
            }
            else if (CurrentMode == ClockMode.CustomTime)
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

                TimeContainer clockTime = new(StartTime.Add(TickInterval.Multiply(TicksSinceStart)));
                TickTransmitter.NotifyAll(clockTime);
            }
            
        }
    }
}
