using Clock.Services.Logging;
using Clock.ViewModels;
using Clock.ViewModels.Drawables;

namespace Clock.Views;

/// <summary>
/// Code-behind for MainPage.
/// </summary>
public partial class MainPage : ContentPage
{
    private Style? ValidEntryTextStyle;
    private Style? InvalidEntryTextStyle;

    public MainPage(MainPageViewModel viewModel)
    {
        InitializeComponent();
        LoadResources();
        SetupClockFace();
        SetupBindingContext(viewModel);
    }

    private void LoadResources()
    {
        string key = "ValidEntryTextStyle";
        if (Resources.TryGetValue(key, out var value) == true && value is Style ves)
            ValidEntryTextStyle = ves;
        else
            LoggingUtility.LogResourceError(nameof(SetupClockFace), key, nameof(MainPage), "Style");

        key = "InvalidEntryTextStyle";
        if (Resources.TryGetValue(key, out value) == true && value is Style ies)
            InvalidEntryTextStyle = ies;
        else
            LoggingUtility.LogResourceError(nameof(SetupClockFace), key, nameof(MainPage), "Style");
    }

    private void SetupClockFace()
    {
        ClockGraphics dynamicClockGraphics = (ClockGraphics)dynamicGraphicsView.Drawable;

        string key = "DefaultSecondHandRotation";
        if (Resources.TryGetValue(key, out var value) == true && value is double shr)
            dynamicClockGraphics.SecondHandRotation = shr;
        else
            LoggingUtility.LogResourceError(nameof(SetupClockFace), key, nameof(MainPage), "Double");

        key = "DefaultMinuteHandRotation";
        if (Resources.TryGetValue(key, out value) == true && value is double mhr)
            dynamicClockGraphics.MinuteHandRotation = mhr;
        else
            LoggingUtility.LogResourceError(nameof(SetupClockFace), key, nameof(MainPage), "Double");

        key = "DefaultHourHandRotation";
        if (Resources.TryGetValue(key, out value) == true && value is double hhr)
            dynamicClockGraphics.HourHandRotation = hhr;
        else
            LoggingUtility.LogResourceError(nameof(SetupClockFace), key, nameof(MainPage), "Double");

        InvalidateCanvas();
    }

    private void SetupBindingContext(MainPageViewModel viewModel)
    {
        BindingContext = viewModel;
        ((MainPageViewModel)BindingContext).InvalidateCanvasRequested += InvalidateCanvas;
        ((MainPageViewModel)BindingContext).SetEntryTextStyleRequested += SetEntryTextStyle;
        ((MainPageViewModel)BindingContext).ClockGraphics = (ClockGraphics)dynamicGraphicsView.Drawable;
        ((MainPageViewModel)BindingContext).SetClockFaceLock(true);
        ((MainPageViewModel)BindingContext).Dispatcher = Dispatcher;
    }

    // tells the canvas in graphicsView to redraw itself
    private void InvalidateCanvas()
    {
        dynamicGraphicsView.Invalidate();
    }

    // sets the entry text style to either the valid style if valid is true or the invalid style if valid is false
    private void SetEntryTextStyle(bool valid)
    {
        if (valid)
            enterCustomTimeEntryBox.Style = ValidEntryTextStyle;
        else
            enterCustomTimeEntryBox.Style = InvalidEntryTextStyle;
    }
}
