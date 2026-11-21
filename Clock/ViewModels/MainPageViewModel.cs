using Clock.Models.ClockModel;
using Clock.Models.Notifications;
using Clock.Services.Logging;
using Clock.ViewModels.Drawables;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        private TimeSpan TickInterval = new(0, 0, 1);
        // the start time of the clock
        private TimeOnly StartTime = new(0, 0, 0);
        // whether or not the clock uses the system time
        private bool ClockUsesSystemTime = false;

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
                LoggingUtility.LogNullObjectNotification(nameof(MainPageViewModel), nameof(clockDispatcher));

            if (ClockData != null)
            {
                TickReciever = new(ClockTicked, TicksCompleted, ClockError);
                TickReciever.Subscribe(ClockData.TickTransmitter);
            }
            else
            {
                LoggingUtility.LogNullObjectNotification(nameof(MainPageViewModel), nameof(ClockData));
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
                if (ClockUsesSystemTime)
                    ClockData.StartClock(DateTime.Now);
                else
                    ClockData.StartClock(new(new(2026, 1, 1), StartTime));
            else
                LoggingUtility.LogNullObjectNotification(nameof(StartClock), nameof(ClockData));
        }

        internal void StopClock()
        {
            if (ClockData != null)
                ClockData.StopClock(false);
            else
                LoggingUtility.LogNullObjectNotification(nameof(StartClock), nameof(ClockData));
        }
        
        // called when TickReciever recieves a tick notification
        private void ClockTicked(TimeContainer timeData)
        {
            if (ClockFace != null)
            {
                SetClockHands(timeData.Time);
            }
            else
            {
                LoggingUtility.LogNullObjectNotification(nameof(ClockTicked), nameof(ClockFace));
                StopClock();
            }
        }

        // called when TickReciever recieves a notice that the clock is done ticking
        private void TicksCompleted()
        {

        }

        // called when TickReciever recieves an error from the clock
        private void ClockError(Exception ex)
        {
            
        }

        private void SetClockHands(DateTime time)
        {
            ClockFace?.HourHandRotation = time.Hour * ClockFace.HOUR_TICK_ROTATION;
            ClockFace?.MinuteHandRotation = time.Minute * ClockFace.MINUTE_TICK_ROTATION;
            ClockFace?.SecondHandRotation = time.Second * ClockFace.MINUTE_TICK_ROTATION;

            InvalidateCanvasRequested?.Invoke();
        }

        /*
         * BINDABLE PROPERTIES
         */

        private bool _isUseSystemTimeButtonEnabled = true;
        private bool _isEnterCustomTimeEntryEnabled = true;
        private bool _isStartClockButtonEnabled = false;
        private bool _isStopClockButtonEnabled = false;

        /// <summary>
        /// Determines whether or not the use system time button is enabled.
        /// </summary>
        public bool IsUseSystemTimeButtonEnabled
        {
            get => _isUseSystemTimeButtonEnabled;
            set => SetProperty(ref _isUseSystemTimeButtonEnabled, value);
        }
        /// <summary>
        /// Determines whether or not the enter custom time entry is enabled.
        /// </summary>
        public bool IsEnterCustomTimeEntryEnabled
        {
            get => _isEnterCustomTimeEntryEnabled;
            set => SetProperty(ref _isEnterCustomTimeEntryEnabled, value);
        }
        /// <summary>
        /// Determines whether or not the start clock button is enabled.
        /// </summary>
        public bool IsStartClockButtonEnabled
        {
            get => _isStartClockButtonEnabled;
            set => SetProperty(ref _isStartClockButtonEnabled, value);
        }
        /// <summary>
        /// Determines whether or not the stop clock button is enabled.
        /// </summary>
        public bool IsStopClockButtonEnabled
        {
            get => _isStopClockButtonEnabled;
            set => SetProperty(ref _isStopClockButtonEnabled, value);
        }

        /*
         * COMMANDS
         */

        // runs when the user clicks the use system time button
        [RelayCommand(CanExecute = nameof(UseSystemTimeCanExecute))]
        void UseSystemTime()
        {
            if (ClockFace != null)
            {
                ToUseSystemTimeButtonPressedState();

                DateTime currentTime = DateTime.Now;
                StartTime = TimeOnly.FromDateTime(currentTime);

                SetClockHands(currentTime);
            }
            else
            {
                LoggingUtility.LogNullObjectNotification(nameof(UseSystemTime), nameof(ClockFace));
            }
        }
        bool UseSystemTimeCanExecute()
        {
            return IsUseSystemTimeButtonEnabled;
        }

        // runs when the user clicks the start clock button
        [RelayCommand(CanExecute = nameof(StartClockButtonClickedCanExecute))]
        void StartClockButtonClicked()
        {
            ToStartClockButtonPressedState();
            StartClock();
        }
        bool StartClockButtonClickedCanExecute()
        {
            return IsStartClockButtonEnabled;
        }

        // runs when the user clicks the stop clock button
        [RelayCommand(CanExecute = nameof(StopClockButtonClickedCanExecute))]
        void StopClockButtonClicked()
        {
            ToStopClockButtonPressedState();
            StopClock();
        }
        bool StopClockButtonClickedCanExecute()
        {
            return IsStopClockButtonEnabled;
        }

        /*
         * STATE TRANSITIONS
         */

        // transitions the view model back to its base state
        private void ToBaseState()
        {
            IsUseSystemTimeButtonEnabled = true;
            IsEnterCustomTimeEntryEnabled = true;
            IsStartClockButtonEnabled = false;
            IsStopClockButtonEnabled = false;

            ClockUsesSystemTime = false;
        }

        // transitions to the use-system-time-button-pressed state
        private void ToUseSystemTimeButtonPressedState()
        {
            IsStartClockButtonEnabled = true;

            ClockUsesSystemTime = true;
        }

        // transitions to the custom-time-entered state
        private void ToCustomTimeEnteredState()
        {
            IsStartClockButtonEnabled = true;
        }

        // transitions to the start-clock-button-pressed state
        private void ToStartClockButtonPressedState()
        {
            IsUseSystemTimeButtonEnabled = false;
            IsEnterCustomTimeEntryEnabled = false;
            IsStartClockButtonEnabled = false;
            IsStopClockButtonEnabled = true;
        }

        // transitions to the stop-clock-button-pressed state
        private void ToStopClockButtonPressedState()
        {
            IsUseSystemTimeButtonEnabled = true;
            IsEnterCustomTimeEntryEnabled = true;
            IsStopClockButtonEnabled = false;
        }
    }
}
