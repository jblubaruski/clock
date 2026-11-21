using System;
using System.Collections.Generic;
using System.Text;

namespace Clock.Services.Logging
{
    public static class LoggingUtility
    {
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
            App.Logger?.LogEntry
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
            App.Logger?.LogEntry
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
            App.Logger?.LogEntry
                (
                    $"In {callLocation}:",
                    $"{objectName} is null"
                );
        }
    }
}
