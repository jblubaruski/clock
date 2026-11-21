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

        ((MainPageViewModel)BindingContext).RecieveClockFace((ClockFace)graphicsView.Drawable);
    }

    private void SetupClockFace()
    {
        ClockFace clockFace = (ClockFace)graphicsView.Drawable;

        string key = "DefaultSecondHandRotation";
        if (Resources.TryGetValue(key, out var value) == true && value is double shr)
            clockFace.SecondHandRotation = shr;
        else
            App.LogResourceError(nameof(SetupClockFace), key, nameof(MainPage), "Double");

        key = "DefaultMinuteHandRotation";
        if (Resources.TryGetValue(key, out value) == true && value is double mhr)
            clockFace.MinuteHandRotation = mhr;
        else
            App.LogResourceError(nameof(SetupClockFace), key, nameof(MainPage), "Double");

        key = "DefaultHourHandRotation";
        if (Resources.TryGetValue(key, out value) == true && value is double hhr)
            clockFace.HourHandRotation = hhr;
        else
            App.LogResourceError(nameof(SetupClockFace), key, nameof(MainPage), "Double");

        graphicsView.Invalidate();
    }

    private void SetupBindingContext()
    {
        BindingContext = new MainPageViewModel();
        ((MainPageViewModel)BindingContext).InvalidateCanvasRequested += InvalidateCanvas;
    }

    private void InvalidateCanvas()
    {
        graphicsView.Invalidate();
    }
}
