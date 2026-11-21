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
        private ClockData? ClockData { get; set; }
        private ClockFace? ClockFace { get; set; }

        public event Action? InvalidateCanvasRequested;

        public MainPageViewModel()
        {
            DispatcherProvider dispatcherProvider = new DispatcherProvider();
            IDispatcher? clockDispatcher = dispatcherProvider.GetForCurrentThread();
            if (clockDispatcher != null)
                ClockData = new(clockDispatcher);
            else
                App.LogDispatcherNullError(nameof(MainPageViewModel));
        }

        public void RecieveClockFace(ClockFace clockFace)
        {
            ClockFace = clockFace;
        }
    }
}
