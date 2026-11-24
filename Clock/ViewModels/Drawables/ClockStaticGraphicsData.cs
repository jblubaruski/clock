using System;
using System.Collections.Generic;
using System.Text;

namespace Clock.ViewModels.Drawables
{
    /// <summary>
    /// Contains data used to draw the static components of the clock display.
    /// </summary>
    internal class ClockStaticGraphicsData
    {
        public float RimOuterRadius { get; set; } = 160.0f;
        public float TickMarkStartScale { get; set; } = 1.0f;
        public float HourTickMarkEndScale { get; set; } = 0.8f;
        public float MinuteTickMarkEndScale { get; set; } = 0.9f;

        public float HourTickMarkStrokeSize { get; set; } = 3;
        public float MinuteTickMarkStrokeSize { get; set; } = 2;

        public Color RimStrokeColor { get; set; } = new(67, 40, 24);
        public Color RimFillColor { get; set; } = new(153, 88, 42);
        public Color FaceFillColor { get; set; } = new(250, 237, 205);
        public Color HourTickMarkStrokeColor { get; set; } = new(111, 29, 27);
        public Color MinuteTickMarkStrokeColor { get; set; } = new(111, 29, 27);
        public Color WallStripeColor1 { get; set; } = new(255, 200, 221);
        public Color WallStripeColor2 { get; set; } = new(189, 224, 254);

        public int NumberOfStripesOnWall { get; set; } = 7;
    }
}
