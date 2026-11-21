using FileUtility.Logging;
using Microsoft.Extensions.DependencyInjection;

namespace Clock
{
    public partial class App : Application
    {
        public const string COMPANY_LOGS_FOLDER = "PlaypenSoftware";
        public const string APPLICATION_NAME = "Clock";

        public int WindowWidth { get; private set; }
        public int WindowHeight { get; private set; }

        public static Logger? Logger { get; private set; }

        public App()
        {
            InitializeComponent();

            Logger = new("log", Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), COMPANY_LOGS_FOLDER, APPLICATION_NAME));

            LoadApplicationResources();
        }

        private void LoadApplicationResources()
        {
            string key = "WindowWidth";
            if (Resources.TryGetValue(key, out var value) == true && value is int width)
                WindowWidth = width;
            else
                LogResourceError(nameof(LoadApplicationResources), key, nameof(App), "Int32");

            key = "WindowHeight";
            if (Resources.TryGetValue(key, out value) == true && value is int height)
                WindowHeight = height;
            else
                LogResourceError(nameof(LoadApplicationResources), key, nameof(App), "Int32");
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

        /// <summary>
        /// Logs an error caused by a resource retrieval failure in the application's log file.
        /// </summary>
        /// 
        /// <param name="callLocation">
        /// The name of the method where the error occurred.
        /// </param>
        /// <param name="key">
        /// The key of the resource that is being retrieved.
        /// </param>
        /// <param name="resourceLocation">
        /// The name of the view where the resource is stored.
        /// </param>
        /// <param name="type">
        /// The expected type of the resource that is being retrieved.
        /// </param>
        public static void LogResourceError(string callLocation, string key, string resourceLocation, string type)
        {
            Logger?.LogEntry
                (
                    $"In {callLocation}:",
                    $"The resource {key} does not exist in the {resourceLocation} resource dictionary as {type}"
                );
        }

        /// <summary>
        /// Logs a notice caused by an attempt to set a one-time-set-only property more than once in the application's log file.
        /// </summary>
        /// 
        /// <param name="propertyClass"></param>
        /// 
        /// <param name="propertyName"></param>
        public static void LogPropertyCantBeSetTwiceNotice(string propertyClass, string propertyName)
        {
            Logger?.LogEntry
                (
                    $"In {propertyClass}:",
                    $"{propertyName} cannot be set more than once and the last set call has been ignored"
                );
        }

        /// <summary>
        /// Logs an error caused by a DispatcherProvider returning a null reference after getting a dispatcher for the current thread.
        /// </summary>
        /// 
        /// <param name="callLocation">
        /// The location of the null object.
        /// </param>
        /// <param name="objectName">
        /// The name of the null object.
        /// </param>
        public static void LogNullObjectNotification(string callLocation, string objectName)
        {
            Logger?.LogEntry
                (
                    $"In {callLocation}:",
                    $"{objectName} is null"
                );
        }
    }
}