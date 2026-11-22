using Clock.Services.Logging;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clock.Services.Conversions
{
    /// <summary>
    /// Contains a collection of converters related to TimeOnly objects.
    /// Requires LoggingUtility.cs
    /// </summary>
    public static class TimeConverters
    {
        // returns a time only object from the passed in string
        // returns false if the conversion failed and logs an entry in the log file explaining the conversion failure
        public static bool ConvertStringToTimeOnly(string timeString, string timeDeliminator, out TimeOnly convertedTime)
        {
            int sectionsInValidTimeStr = 3;
            int[] hourBounds = [0, 23];
            int[] minuteBounds = [0, 59];
            int[] secondBounds = [0, 59];

            string[] splitTimeStr = timeString.Split(timeDeliminator);

            // ensure the split string has exactly three elements
            if (splitTimeStr.Length != sectionsInValidTimeStr)
            {
                LoggingUtility.LogStringSplitIntoWrongNumberOfElementsError(nameof(ConvertStringToTimeOnly), sectionsInValidTimeStr.ToString(), timeDeliminator, timeString);
                convertedTime = new();
                return false;
            }

            string hourString = splitTimeStr[0];
            string minuteString = splitTimeStr[1];
            string secondString = splitTimeStr[2];

            // ensure each element of the split time string is a valid integer
            if (!int.TryParse(hourString, out int hour))
            {
                LoggingUtility.LogStringNotValidNumberError(nameof(ConvertStringToTimeOnly), hourString, "Int32");
                convertedTime = new();
                return false;
            }
            if (!int.TryParse(minuteString, out int minute))
            {
                LoggingUtility.LogStringNotValidNumberError(nameof(ConvertStringToTimeOnly), minuteString, "Int32");
                convertedTime = new();
                return false;
            }
            if (!int.TryParse(secondString, out int second))
            {
                LoggingUtility.LogStringNotValidNumberError(nameof(ConvertStringToTimeOnly), secondString, "Int32");
                convertedTime = new();
                return false;
            }

            // ensure that each number from the split string is in the proper range
            if (hour < hourBounds[0] || hour > hourBounds[1])
            {
                LoggingUtility.LogNumberOutsideOfBoundsError(nameof(ConvertStringToTimeOnly), nameof(hour), hour.ToString(), hourBounds[0].ToString(), hourBounds[1].ToString());
                convertedTime = new();
                return false;
            }
            if (minute < minuteBounds[0] || minute > minuteBounds[1])
            {
                LoggingUtility.LogNumberOutsideOfBoundsError(nameof(ConvertStringToTimeOnly), nameof(minute), minute.ToString(), minuteBounds[0].ToString(), minuteBounds[1].ToString());
                convertedTime = new();
                return false;
            }
            if (second < secondBounds[0] || second > secondBounds[1])
            {
                LoggingUtility.LogNumberOutsideOfBoundsError(nameof(ConvertStringToTimeOnly), nameof(second), second.ToString(), secondBounds[0].ToString(), secondBounds[1].ToString());
                convertedTime = new();
                return false;
            }

            convertedTime = new(hour, minute, second);
            return true;
        }
    }
}
