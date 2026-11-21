using Clock.Models.ClockModel;
using Clock.Models.Notifications;
using Clock.ViewModels.Drawables;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clock.ViewModels
{
    /// <summary>
    /// View model for MainPage.
    /// </summary>
    public partial class MainPageViewModel : ObservableObject
    {
        // the span of time between each clock tick
        private static readonly TimeSpan TickInterval = new(0, 0, 1);
        // the start time of the clock
        private static readonly TimeOnly StartTime = new(0, 0, 0);

        // the object that holds and manages the time on the clock
        private ClockData? ClockData { get; set; }
        // the observer that recieves tick notifications from the clock
        private readonly NotificationReciever<TimeContainer>? TickReciever;

        private ClockFace? _clockFace;
        /// <summary>
        /// The ClockFace managed by this view model.
        /// Can only be set if CanClockFaceBeSet is true.
        /// </summary>
        internal ClockFace? ClockFace
        { 
            private get => _clockFace;
            set
            {
                if (CanClockFaceBeSet)
                    _clockFace = value;
            }
        }

        /// <summary>
        /// Allows ClockFace to be set if true.
        /// </summary>
        internal bool CanClockFaceBeSet { get; private set; } = true;

        /// <summary>
        /// Event fired when the view model wants a drawing canvas to redraw itself.
        /// </summary>
        internal event Action? InvalidateCanvasRequested;

        /// <summary>
        /// Constructs the MainPageViewModel.
        /// </summary>
        public MainPageViewModel()
        {
            DispatcherProvider dispatcherProvider = new();
            IDispatcher? clockDispatcher = dispatcherProvider.GetForCurrentThread();

            if (clockDispatcher != null)
                ClockData = new(clockDispatcher, TickInterval);
            else
                App.LogNullObjectNotification(nameof(MainPageViewModel), nameof(clockDispatcher));

            if (ClockData != null)
            {
                TickReciever = new(ClockTicked, TicksCompleted, ClockError);
                TickReciever.Subscribe(ClockData.TickTransmitter);
            }
            else
            {
                App.LogNullObjectNotification(nameof(MainPageViewModel), nameof(ClockData));
            }
        }

        /// <summary>
        /// Sets whether or not ClockFace is locked from recieving a new reference.
        /// </summary>
        /// 
        /// <param name="isLocked">
        /// True if the ClockFace is to be locked.
        /// </param>
        internal void SetClockFaceLock(bool isLocked) => CanClockFaceBeSet = !isLocked;

        /// <summary>
        /// Starts the clock.
        /// </summary>
        internal void StartClock()
        {
            if (ClockData != null)
                ClockData.StartClock(new(new(2026, 1, 1), StartTime));
            else
                App.LogNullObjectNotification(nameof(StartClock), nameof(ClockData));
        }
        
        // called when TickReciever recieves a tick notification
        private void ClockTicked(TimeContainer timeData)
        {
            App.Logger?.LogEntry($"Clock Ticked at {TimeOnly.FromDateTime(timeData.Time):T}");
        }

        // called when TickReciever recieves a notice that the clock is done ticking
        private void TicksCompleted()
        {

        }

        // called when TickReciever recieves an error from the clock
        private void ClockError(Exception ex)
        {
            
        }
    }
}
