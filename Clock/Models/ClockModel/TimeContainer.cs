using System;
using System.Collections.Generic;
using System.Text;

namespace Clock.Models.ClockModel
{
    public sealed class TimeContainer(DateTime time)
    {
        public DateTime Time { get; } = time;
    }
}
