using Clock.Models.ClockModel;
using Clock.Models.Notifications;
using Clock.Services.Logging;
using Clock.Services.Conversions;
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
        // string format for a double that converts it to a decimal number string with max precision
        private readonly string DoubleMaxPrecisionStringFormat = "0." + new string('#', 339);
        // whether or not the clock drift test logging is active
        private readonly bool ClockDriftTestLoggingActive = true;
        // tracks the number of ticks since the clock started ticking
        private int ClockTicks = 0;

        // the span of time between each clock tick
        private readonly TimeSpan TickInterval = new(0, 0, 1);
        // the start time of the clock
        private TimeOnly StartTime = new(0, 0, 0);

        // the object that holds and manages the time on the clock
        private ClockData ClockData { get; set; }
        // the observer that recieves tick notifications from the clock
        private readonly NotificationReciever<TimeContainer> TickReciever;

        private ClockGraphics? _clockGraphics;
        /// <summary>
        /// The ClockFace managed by this view model.
        /// Can only be set if CanClockFaceBeSet is true.
        /// </summary>
        internal ClockGraphics? ClockGraphics
        { 
            private get => _clockGraphics;
            set
            {
                if (CanClockFaceBeSet)
                    _clockGraphics = value;
            }
        }

        /// <summary>
        /// The dispatcher of whatever view uses this view model.
        /// </summary>
        internal IDispatcher? Dispatcher { private get; set; }

        /// <summary>
        /// Allows ClockFace to be set if true.
        /// </summary>
        internal bool CanClockFaceBeSet { get; private set; } = true;

        /// <summary>
        /// Event fired when the view model wants a drawing canvas to redraw itself.
        /// </summary>
        internal event Action? InvalidateCanvasRequested;
        internal event Action<bool>? SetEntryTextStyleRequested;

        /// <summary>
        /// Constructs the MainPageViewModel.
        /// </summary>
        public MainPageViewModel()
        {
            ClockData = new(TickInterval);
            TickReciever = new(ClockTicked, TicksCompleted, ClockError);
            TickReciever.Subscribe(ClockData.TickTransmitter);
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
        internal async Task StartClock(bool clockUsesSystemTime)
        {
            ClockTicks = 0;

            if (ClockData != null)
                if (clockUsesSystemTime)
                    await ClockData.StartClock(DateTime.Now, ClockMode.SystemTime);
                else
                    await ClockData.StartClock(new(new(2026, 1, 1), StartTime), ClockMode.CustomTime);
            else
                LoggingUtility.LogNullObjectNotification(nameof(StartClock), nameof(ClockData));
        }

        internal void StopClock()
        {
            ClockData.StopClock(false);
        }

        // called when TickReciever recieves a tick notification
        private void ClockTicked(TimeContainer timeData)
        {
            if (Dispatcher == null)
                throw new InvalidOperationException("The UI thread's dispatcher cannot be null");

            if (ClockGraphics == null)
            {
                LoggingUtility.LogNullObjectNotification(nameof(ClockTicked), nameof(ClockGraphics));
                StopClock();
            }

            
            DateTime currentTime = DateTime.Now;
            
            if (ClockDriftTestLoggingActive)
            {
                if (ClockData.CurrentMode == ClockMode.SystemTime)
                {
                    ClockTicks += 1;
                    TimeSpan timeDiff = currentTime.Subtract(timeData.Time);
                    LoggingUtility.LogTabSeparatedValueLine(false, ClockTicks.ToString(), timeDiff.TotalSeconds.ToString(DoubleMaxPrecisionStringFormat));
                }
            }

            Dispatcher.Dispatch(() =>
            {
                SetClockHands(TimeOnly.FromDateTime(timeData.Time));
            });
        }

        // called when TickReciever recieves a notice that the clock is done ticking
        private void TicksCompleted()
        {

        }

        // called when TickReciever recieves an error from the clock
        private void ClockError(Exception ex)
        {
            
        }

        // sets the clock hands to the passed in time
        private void SetClockHands(TimeOnly time)
        {
            ClockGraphics?.HourHandRotation = time.Hour * ClockGraphicsData.HourTickRotation;
            ClockGraphics?.MinuteHandRotation = time.Minute * ClockGraphicsData.MinuteTickRotation;
            ClockGraphics?.SecondHandRotation = time.Second * ClockGraphicsData.MinuteTickRotation;

            InvalidateCanvasRequested?.Invoke();
        }

        /*
         * BINDABLE PROPERTIES
         */

        private bool _isUseSystemTimeButtonEnabled = true;
        private bool _isEnterCustomTimeEntryEnabled = true;
        private bool _isStartClockButtonEnabled = false;
        private bool _isStopClockButtonEnabled = false;
        private string _customTimeEntryText = "";

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
        /// <summary>
        /// The text entered into the enter custom time entry box.
        /// </summary>
        public string CustomTimeEntryText
        {
            get => _customTimeEntryText;
            set => SetProperty(ref _customTimeEntryText, value);
        }

        /*
         * COMMANDS
         */

        // runs when the user clicks the use system time button
        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(UseSystemTimeCanExecute))]
        async Task UseSystemTime()
        {
            if (ClockGraphics != null)
            {
                ToUseSystemTimeButtonPressedState();
                SetClockHands(TimeOnly.FromDateTime(DateTime.Now));

                StartTime = TimeOnly.FromDateTime(DateTime.Now);
                await StartClock(true);
            }
            else
            {
                LoggingUtility.LogNullObjectNotification(nameof(UseSystemTime), nameof(ClockGraphics));
            }
        }
        bool UseSystemTimeCanExecute()
        {
            return IsUseSystemTimeButtonEnabled;
        }

        // runs when the user presses enter while entering text in the enter custom time entry box
        [RelayCommand(CanExecute = nameof(EnterCustomTimeCanExecute))]
        void EnterCustomTime()
        {
            if (TimeConverters.ConvertStringToTimeOnly(CustomTimeEntryText, ":", out StartTime))
            {
                ToCustomTimeEnteredState();
                SetClockHands(StartTime);
                SetEntryTextStyleRequested?.Invoke(true);
            }
            else
            {
                SetEntryTextStyleRequested?.Invoke(false);
            }
        }
        bool EnterCustomTimeCanExecute()
        {
            return IsEnterCustomTimeEntryEnabled;
        }

        // runs when the user clicks the start clock button
        [RelayCommand(AllowConcurrentExecutions = false, CanExecute = nameof(StartClockButtonClickedCanExecute))]
        async Task StartClockButtonClicked()
        {
            ToStartClockButtonPressedState();
            await StartClock(false);
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
        }

        // transitions to the use-system-time-button-pressed state
        private void ToUseSystemTimeButtonPressedState()
        {
            ToStartClockButtonPressedState();
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
