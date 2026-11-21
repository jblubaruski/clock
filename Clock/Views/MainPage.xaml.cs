using Clock.Services.Logging;
using Clock.ViewModels;
using Clock.ViewModels.Drawables;

namespace Clock.Views;

/// <summary>
/// Code-behind for MainPage.
/// </summary>
public partial class MainPage : ContentPage
{
    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        SetupClockFace();
        SetupBindingContext(viewModel);
    }

    private void SetupClockFace()
    {
        ClockFace clockFace = (ClockFace)graphicsView.Drawable;

        string key = "DefaultSecondHandRotation";
        if (Resources.TryGetValue(key, out var value) == true && value is double shr)
            clockFace.SecondHandRotation = shr;
        else
            LoggingUtility.LogResourceError(nameof(SetupClockFace), key, nameof(MainPage), "Double");

        key = "DefaultMinuteHandRotation";
        if (Resources.TryGetValue(key, out value) == true && value is double mhr)
            clockFace.MinuteHandRotation = mhr;
        else
            LoggingUtility.LogResourceError(nameof(SetupClockFace), key, nameof(MainPage), "Double");

        key = "DefaultHourHandRotation";
        if (Resources.TryGetValue(key, out value) == true && value is double hhr)
            clockFace.HourHandRotation = hhr;
        else
            LoggingUtility.LogResourceError(nameof(SetupClockFace), key, nameof(MainPage), "Double");

        graphicsView.Invalidate();
    }

    private void SetupBindingContext(MainPageViewModel viewModel)
    {
        BindingContext = viewModel;
        ((MainPageViewModel)BindingContext).InvalidateCanvasRequested += InvalidateCanvas;
        ((MainPageViewModel)BindingContext).ClockFace = (ClockFace)graphicsView.Drawable;
        ((MainPageViewModel)BindingContext).SetClockFaceLock(true);
    }

    private void InvalidateCanvas()
    {
        graphicsView.Invalidate();
    }
}
