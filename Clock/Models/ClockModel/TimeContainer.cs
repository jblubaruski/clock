using System;
using System.Collections.Generic;
using System.Text;

namespace Clock.Models.ClockModel
{
    /// <summary>
    /// A container for a point in time.
    /// </summary>
    /// 
    /// <param name="time">
    /// The point in time to be contained in the container.
    /// </param>
    public sealed class TimeContainer(DateTime time)
    {
        /// <summary>
        /// The point in time contained in the container.
        /// </summary>
        public DateTime Time { get; } = time;
    }
}
