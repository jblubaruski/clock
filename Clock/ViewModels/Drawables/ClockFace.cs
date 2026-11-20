using Clock.Models.ClockModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clock.ViewModels.Drawables
{
    /// <summary>
    /// Draws the clock face to a graphics canvas.
    /// </summary>
    public class ClockFace : IDrawable
    {
        public const float RIM_OUTER_RADIUS = 170.0f;
        public const float RIM_INNER_RADIUS = 150.0f;
        public const float RIM_STROKE_SIZE = 4;
        public const float TICK_MARK_START_SCALE = 1.0f;
        public const float HOUR_TICK_MARK_STROKE_SIZE = 3;
        public const float HOUR_TICK_MARK_END_SCALE = 0.8f;
        public const float MINUTE_TICK_MARK_STROKE_SIZE = 2;
        public const float MINUTE_TICK_MARK_END_SCALE = 0.9f;
        public static readonly Color RIM_STROKE_COLOR = Colors.Black;
        public static readonly Color HOUR_TICK_MARK_STROKE_COLOR = Colors.DarkGray;
        public static readonly Color MINUTE_TICK_MARK_STROKE_COLOR = Colors.Gray;

        public static readonly double FULL_ROTATION = 2 * Math.PI;
        public static readonly double HOUR_TICK_ROTATION = Math.PI / 6.0;
        public static readonly double MINUTE_TICK_ROTATION = Math.PI / 30.0;

        // the rotations of the second, minute and hour hands
        // rotations are in degrees with 0 degrees at the top of the clock with numbers increasing in the clockwise direction
        // at 360 degrees, rotations are set back to 0 degrees
        public double SecondHandRotation { get; set; }
        public double MinuteHandRotation { get; set; }
        public double HourHandRotation { get; set; }
        
        private ClockData? _clockData;
        public ClockData? ClockData 
        { 
            private get => _clockData; 
            set
            { 
                if (!ClockDataHasBeenSet)
                {
                    _clockData = value;
                    ClockDataHasBeenSet = true;
                }
                else
                {
                    App.LogPropertyCantBeSetTwiceNotice(nameof(ClockFace), nameof(ClockData));
                }
            }
        }
        private bool ClockDataHasBeenSet = false;

        /// <summary>
        /// The draw logic that determines the look of the clock face.
        /// </summary>
        /// 
        /// <param name="canvas">
        /// The drawing canvas the clock face is drawn on.
        /// </param>
        /// <param name="canvasData">
        /// The size and position on the screen of the drawing canvas.
        /// </param>
        public void Draw(ICanvas canvas, RectF canvasData)
        {
            // get the center of the drawing canvas
            float centerX = canvasData.Center.X;
            float centerY = canvasData.Center.Y;

            // draw outer body
            canvas.StrokeColor = RIM_STROKE_COLOR;
            canvas.StrokeSize = RIM_STROKE_SIZE;
            canvas.DrawCircle(centerX, centerY, RIM_OUTER_RADIUS);
            canvas.DrawCircle(centerX, centerY, RIM_INNER_RADIUS);

            // draw the hour tick marks
            canvas.StrokeColor = HOUR_TICK_MARK_STROKE_COLOR;
            canvas.StrokeSize = HOUR_TICK_MARK_STROKE_SIZE;
            for (int hour = 0; hour < 12; hour++)
            {
                double theta = hour * HOUR_TICK_ROTATION;

                float lineStartX = centerX + TICK_MARK_START_SCALE * (RIM_INNER_RADIUS - RIM_STROKE_SIZE / 2) * MathF.Sin((float)theta);
                float lineStartY = centerY + -TICK_MARK_START_SCALE * (RIM_INNER_RADIUS - RIM_STROKE_SIZE / 2) * MathF.Cos((float)theta);
                float lineEndX = centerX + HOUR_TICK_MARK_END_SCALE * RIM_INNER_RADIUS * MathF.Sin((float)theta);
                float lineEndY = centerY + -HOUR_TICK_MARK_END_SCALE * RIM_INNER_RADIUS * MathF.Cos((float)theta);

                canvas.DrawLine(lineStartX, lineStartY, lineEndX, lineEndY);
            }

            // draw the minute tick marks
            canvas.StrokeColor = MINUTE_TICK_MARK_STROKE_COLOR;
            canvas.StrokeSize = MINUTE_TICK_MARK_STROKE_SIZE;
            for (int minute = 0; minute < 60; minute++)
            {
                double theta = minute * MINUTE_TICK_ROTATION;

                if (minute % 5 != 0)
                {
                    float lineStartX = centerX + TICK_MARK_START_SCALE * (RIM_INNER_RADIUS - RIM_STROKE_SIZE / 2) * MathF.Sin((float)theta);
                    float lineStartY = centerY + -TICK_MARK_START_SCALE * (RIM_INNER_RADIUS - RIM_STROKE_SIZE / 2) * MathF.Cos((float)theta);
                    float lineEndX = centerX + MINUTE_TICK_MARK_END_SCALE * RIM_INNER_RADIUS * MathF.Sin((float)theta);
                    float lineEndY = centerY + -MINUTE_TICK_MARK_END_SCALE * RIM_INNER_RADIUS * MathF.Cos((float)theta);

                    canvas.DrawLine(lineStartX, lineStartY, lineEndX, lineEndY);
                }
            }
        }
    }
}
