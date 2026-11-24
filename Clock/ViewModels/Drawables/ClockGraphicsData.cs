using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Clock.ViewModels.Drawables
{
    /// <summary>
    /// Contains data used to draw the non-static components of the clock display.
    /// </summary>
    internal class ClockGraphicsData
    {
        public float SecondHandStartScale { get; set; } = 0.1f;
        public float SecondHandEndScale { get; set; } = 0.85f;
        public float MinuteHandStartScale { get; set; } = 0.075f;
        public float MinuteHandEndScale { get; set; } = 0.7f;
        public float HourHandStartScale { get; set; } = 0.05f;
        public float HourHandEndScale { get; set; } = 0.5f;

        public float SecondHandStrokeSize { get; set; } = 1;
        public float MinuteHandStrokeSize { get; set; } = 3;
        public float HourHandStrokeSize { get; set; } = 4;
        
        public Color SecondHandStrokeColor { get; set; } = new(0, 0, 0);
        public Color MinuteHandStrokeColor { get; set; } = new(20, 33, 61);
        public Color HourHandStrokeColor { get; set; } = new(20, 33, 61);
    }
}
