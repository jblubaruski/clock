using Clock.ViewModels;
using Clock.Views;
using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;

namespace Clock
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<MainPageViewModel>();

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            // create a borderless entry box
            Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping("BorderlessEntry", (handler, view) =>
            {
#if WINDOWS
                // remove resting border and square off corners
                handler.PlatformView.BorderThickness = new Microsoft.UI.Xaml.Thickness(0);
                handler.PlatformView.CornerRadius = new Microsoft.UI.Xaml.CornerRadius(0);
                handler.PlatformView.Padding = new Microsoft.UI.Xaml.Thickness(8, 0, 8, 0);
                handler.PlatformView.MinHeight = 0;

                // keep resting and hover states borderless
                var transparentBrush = new Microsoft.UI.Xaml.Media.SolidColorBrush(Microsoft.UI.Colors.Transparent);
                handler.PlatformView.Resources["TextControlBorderBrush"] = transparentBrush;
                handler.PlatformView.Resources["TextControlBorderBrushPointerOver"] = transparentBrush;
                handler.PlatformView.Resources["TextControlElevationBorderBrush"] = transparentBrush;
                handler.PlatformView.Resources["TextControlBorderThemeThickness"] = new Microsoft.UI.Xaml.Thickness(0);

                // set focus underline color
                var focusUnderlineBrush = new Microsoft.UI.Xaml.Media.SolidColorBrush(Windows.UI.Color.FromArgb(255, 0, 0, 0));
                handler.PlatformView.Resources["TextControlBorderBrushFocused"] = focusUnderlineBrush;
                handler.PlatformView.Resources["TextControlElevationBorderFocusedBrush"] = focusUnderlineBrush;

                // restrict the focused border to the bottom edge
                handler.PlatformView.Resources["TextControlBorderThemeThicknessFocused"] = new Microsoft.UI.Xaml.Thickness(0, 0, 0, 2);
#elif MACCATALYST
                handler.PlatformView.BorderStyle = UIKit.UITextBorderStyle.None;
#endif
            });

            return builder.Build();
        }
    }
}
