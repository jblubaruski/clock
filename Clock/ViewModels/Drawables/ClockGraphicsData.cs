using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Clock.ViewModels.Drawables
{
    /// <summary>
    /// Contains data used by ClockFace to draw the clock.
    /// </summary>
    public class ClockGraphicsData
    {
        /// <summary>
        /// Radians in a full rotation.
        /// </summary>
        public static readonly double FullRotation = 2 * Math.PI;
        /// <summary>
        /// Radians in one rotation of the hour hand.
        /// </summary>
        public static readonly double HourTickRotation = Math.PI / 6.0;
        /// <summary>
        /// Radians in one rotation of the minute hand.
        /// </summary>
        public static readonly double MinuteTickRotation = Math.PI / 30.0;

        public readonly float RimOuterRadius = 160.0f;
        public readonly float RimInnerRadius = 140.0f;
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

        public readonly Color RimStrokeColor = new(67, 40, 24);
        public readonly Color RimFillColor = new(153, 88, 42);
        public readonly Color FaceFillColor = new(250, 237, 205);
        public readonly Color HourTickMarkStrokeColor = new(111, 29, 27);
        public readonly Color MinuteTickMarkStrokeColor = new(111, 29, 27);
        public readonly Color SecondHandStrokeColor = new (0, 0, 0);
        public readonly Color MinuteHandStrokeColor = new(20, 33, 61);
        public readonly Color HourHandStrokeColor = new(20, 33, 61);
        public readonly Color WallStripeColor1 = new(255, 200, 221);
        public readonly Color WallStripeColor2 = new(189, 224, 254);
        
        public readonly int NumberOfStripesOnWall = 7;
    }
}
