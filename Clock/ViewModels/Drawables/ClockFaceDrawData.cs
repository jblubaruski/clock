using System;
using System.Collections.Generic;
using System.Text;

namespace Clock.ViewModels.Drawables
{
    /// <summary>
    /// Contains data used by ClockFace to draw the clock
    /// </summary>
    public class ClockFaceDrawData
    {
        public readonly float RimOuterRadius = 170.0f;
        public readonly float RimInnerRadius = 150.0f;
        public readonly float TickMarkStartScale = 1.0f;
        public readonly float HourTickMarkEndScale = 0.8f;
        public readonly float MinuteTickMarkEndScale = 0.9f;
        public readonly float SecondHandStartScale = 0.1f;
        public readonly float SecondHandEndScale = 0.85f;
        public readonly float MinuteHandStartScale = 0.075f;
        public readonly float MinuteHandEndScale = 0.7f;
        public readonly float HourHandStartScale = 0.05f;
        public readonly float HourHandEndScale = 0.5f;

        public readonly float RimStrokeSize = 4;
        public readonly float HourTickMarkStrokeSize = 3;
        public readonly float MinuteTickMarkStrokeSize = 2;
        public readonly float SecondHandStrokeSize = 1;
        public readonly float MinuteHandStrokeSize = 3;
        public readonly float HourHandStrokeSize = 4;

        public readonly Color RimStrokeColor = Colors.Black;
        public readonly Color HourTickMarkStrokeColor = Colors.DarkGray;
        public readonly Color MinuteTickMarkStrokeColor = Colors.Gray;
        public readonly Color SecondHandStrokeColor = Colors.Red;
        public readonly Color MinuteHandStrokeColor = Colors.Green;
        public readonly Color HourHandStrokeColor = Colors.Blue;
    }
}
