using FileUtility.Logging;
using Clock.Services.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Clock
{
    public partial class App : Application
    {
        public int WindowWidth { get; private set; }
        public int WindowHeight { get; private set; }

        public App()
        {

            InitializeComponent();
            LoadApplicationResources();
        }

        private void LoadApplicationResources()
        {
            string key = "WindowWidth";
            if (Resources.TryGetValue(key, out var value) == true && value is int width)
                WindowWidth = width;
            else
                LoggingUtility.LogResourceError(nameof(LoadApplicationResources), key, nameof(App), "Int32");

            key = "WindowHeight";
            if (Resources.TryGetValue(key, out value) == true && value is int height)
                WindowHeight = height;
            else
                LoggingUtility.LogResourceError(nameof(LoadApplicationResources), key, nameof(App), "Int32");
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell())
            {
                MinimumWidth = WindowWidth,
                MaximumWidth = WindowWidth,
                MinimumHeight = WindowHeight,
                MaximumHeight = WindowHeight,
                IsMaximizable = false
            };
        }
    }
}