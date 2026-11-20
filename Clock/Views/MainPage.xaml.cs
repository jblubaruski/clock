using Clock.ViewModels;
using Clock.ViewModels.Drawables;

namespace Clock.Views;

public partial class MainPage : ContentPage
{
    public MainPage()
    {
        InitializeComponent();
        SetupClockFace();
        SetupBindingContext();

        ((MainPageViewModel)BindingContext).GiveClockDataToClockFace((ClockFace)graphicsView.Drawable);
    }

    private void SetupClockFace()
    {
        ClockFace clockFace = (ClockFace)graphicsView.Drawable;

        string key = "DefaultSecondHandRotation";
        if (Resources.TryGetValue(key, out var value) == true && value is double shp)
            clockFace.SecondHandRotation = shp;
        else
            App.LogResourceError(nameof(SetupClockFace), key, nameof(MainPage), "Double");

        key = "DefaultMinuteHandRotation";
        if (Resources.TryGetValue(key, out value) == true && value is double mhp)
            clockFace.MinuteHandRotation = mhp;
        else
            App.LogResourceError(nameof(SetupClockFace), key, nameof(MainPage), "Double");

        key = "DefaultHourHandRotation";
        if (Resources.TryGetValue(key, out value) == true && value is double hhp)
            clockFace.HourHandRotation = hhp;
        else
            App.LogResourceError(nameof(SetupClockFace), key, nameof(MainPage), "Double");

        graphicsView.Invalidate();
    }

    private void SetupBindingContext()
    {
        BindingContext = new MainPageViewModel();
    }
}
