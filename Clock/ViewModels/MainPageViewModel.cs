using Clock.Models.ClockModel;
using Clock.ViewModels.Drawables;
using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clock.ViewModels
{
    public partial class MainPageViewModel : ObservableObject
    {
        private ClockData? Clock { get; set; }

        public event Func<ClockFace>? ProvideClockFaceReferenceRequested;

        public MainPageViewModel()
        {
            DispatcherProvider dispatcherProvider = new DispatcherProvider();
            IDispatcher? clockDispatcher = dispatcherProvider.GetForCurrentThread();
            if (clockDispatcher != null)
                Clock = new(clockDispatcher);
            else
                App.LogDispatcherNullError(nameof(MainPageViewModel));
        }

        public void GiveClockDataToClockFace(ClockFace clockFace)
        {
            clockFace.ClockData = Clock;
        }
    }
}
