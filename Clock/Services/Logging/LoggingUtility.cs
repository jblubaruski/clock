using System;
using System.Collections.Generic;
using System.Security.AccessControl;
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

        /// <summary>
        /// Logs an error caused by a string failing to parse into a number of a specified type.
        /// </summary>
        /// 
        /// <param name="errorLocation">
        /// The location of the error.
        /// </param>
        /// <param name="invalidString">
        /// The string that failed to parse.
        /// </param>
        /// <param name="numberType">
        /// The type the string was being parsed into.
        /// </param>
        public static void LogStringNotValidNumberError(string errorLocation, string invalidString, string numberType)
        {
            App.Logger?.LogEntry
                (
                    $"In {errorLocation}:",
                    $"{invalidString} cannot be parsed into {numberType}"
                );
        }

        /// <summary>
        /// Logs an error caused by a number falling outside of an expected range.
        /// </summary>
        /// 
        /// <param name="errorLocation">
        /// The location of the error.
        /// </param>
        /// <param name="numberVaraiableName">
        /// The name of the number's variable.
        /// </param>
        /// <param name="numberVariableValue">
        /// The value of the number's variable.
        /// </param>
        /// <param name="numberBoundsMin">
        /// The minimum value of the number's bounds (inclusive).
        /// </param>
        /// <param name="numberBoundsMax">
        /// The maximum value of the number's bounds (inclusive).
        /// </param>
        public static void LogNumberOutsideOfBoundsError(string errorLocation, string numberVaraiableName, string numberVariableValue, string numberBoundsMin, string numberBoundsMax)
        {
            App.Logger?.LogEntry
                (
                    $"In {errorLocation}",
                    $"the number in {numberVaraiableName} ({numberVariableValue}) falls outside of the defined bounds: [{numberBoundsMin}, {numberBoundsMax}]"
                );
        }

        /// <summary>
        /// Logs an error caused by a string getting split into an incorrect number of elements with a specified deliminator.
        /// </summary>
        /// 
        /// <param name="errorLocation">
        /// The location of the error.
        /// </param>
        /// <param name="expectedNumberOfElements">
        /// The expected number of elements the string should get split into.
        /// </param>
        /// <param name="stringDeliminator">
        /// The deliminator used to split the string.
        /// </param>
        /// <param name="splitString">
        /// The string getting split.
        /// </param>
        public static void LogStringSplitIntoWrongNumberOfElementsError(string errorLocation, string expectedNumberOfElements, string stringDeliminator, string splitString)
        {
            App.Logger?.LogEntry
                (
                    $"In {errorLocation}",
                    $"There were more or less than {expectedNumberOfElements} sections split by {stringDeliminator} in {splitString}"
                );
        }
    }
}
