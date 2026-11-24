using System;
using System.Collections.Generic;
using System.Text;

namespace Clock.ViewModels.Drawables
{
    /// <summary>
    /// Contains data used to draw both the static and non-static components of the clock display.
    /// </summary>
    internal class ClockGraphicsSharedData
    {
        public float RimInnerRadius { get; set; } = 140.0f;
        public float RimStrokeSize { get; set; } = 4;
    }
}
