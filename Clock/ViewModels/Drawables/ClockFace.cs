using Clock.Services.Drawing;
using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Clock.ViewModels.Drawables
{
    /// <summary>
    /// Draws the clock face to a graphics canvas.
    /// </summary>
    public class ClockFace : IDrawable
    {
        public const float RIM_OUTER_RADIUS = 170.0f;
        public const float RIM_INNER_RADIUS = 150.0f;
        public const float TICK_MARK_START_SCALE = 1.0f;
        public const float HOUR_TICK_MARK_END_SCALE = 0.8f;
        public const float MINUTE_TICK_MARK_END_SCALE = 0.9f;
        public const float SECOND_HAND_START_SCALE = 0.1f;
        public const float SECOND_HAND_END_SCALE = 0.85f;
        public const float MINUTE_HAND_START_SCALE = 0.075f;
        public const float MINUTE_HAND_END_SCALE = 0.7f;
        public const float HOUR_HAND_START_SCALE = 0.05f;
        public const float HOUR_HAND_END_SCALE = 0.5f;

        public const float RIM_STROKE_SIZE = 4;
        public const float HOUR_TICK_MARK_STROKE_SIZE = 3;
        public const float MINUTE_TICK_MARK_STROKE_SIZE = 2;
        public const float SECOND_HAND_STROKE_SIZE = 1;
        public const float MINUTE_HAND_STROKE_SIZE = 3;
        public const float HOUR_HAND_STROKE_SIZE = 4;

        public static readonly Color RIM_STROKE_COLOR = Colors.Black;
        public static readonly Color HOUR_TICK_MARK_STROKE_COLOR = Colors.DarkGray;
        public static readonly Color MINUTE_TICK_MARK_STROKE_COLOR = Colors.Gray;
        public static readonly Color SECOND_HAND_STROKE_COLOR = Colors.Red;
        public static readonly Color MINUTE_HAND_STROKE_COLOR = Colors.Green;
        public static readonly Color HOUR_HAND_STROKE_COLOR = Colors.Blue;

        // the rotations of the second, minute and hour hands
        // rotations are in radians with 0 radians at the top of the clock with numbers increasing in the clockwise direction
        // at 2 * PI radians, rotations are set back to 0
        public double SecondHandRotation { get; set; }
        public double MinuteHandRotation { get; set; }
        public double HourHandRotation { get; set; }

        public static readonly double FULL_ROTATION = 2 * Math.PI;
        public static readonly double HOUR_TICK_ROTATION = Math.PI / 6.0;
        public static readonly double MINUTE_TICK_ROTATION = Math.PI / 30.0;

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
            DrawRim(canvas, canvasData);
            DrawHourTickMarks(canvas, canvasData);
            DrawMinuteTickMarks(canvas, canvasData);
            DrawHand(canvas, canvasData, SECOND_HAND_STROKE_COLOR, SECOND_HAND_STROKE_SIZE, SECOND_HAND_START_SCALE, SECOND_HAND_END_SCALE, (float)SecondHandRotation);
            DrawHand(canvas, canvasData, MINUTE_HAND_STROKE_COLOR, MINUTE_HAND_STROKE_SIZE, MINUTE_HAND_START_SCALE, MINUTE_HAND_END_SCALE, (float)MinuteHandRotation);
            DrawHand(canvas, canvasData, HOUR_HAND_STROKE_COLOR, HOUR_HAND_STROKE_SIZE, HOUR_HAND_START_SCALE, HOUR_HAND_END_SCALE, (float)HourHandRotation);
        }

        // draws the rim
        private static void DrawRim(ICanvas canvas, RectF canvasData)
        {
            canvas.StrokeColor = RIM_STROKE_COLOR;
            canvas.StrokeSize = RIM_STROKE_SIZE;

            canvas.DrawCircle(canvasData.Center, RIM_OUTER_RADIUS);
            canvas.DrawCircle(canvasData.Center, RIM_INNER_RADIUS);
        }

        // draws the hour tick marks
        private static void DrawHourTickMarks(ICanvas canvas, RectF canvasData)
        {
            canvas.StrokeColor = HOUR_TICK_MARK_STROKE_COLOR;
            canvas.StrokeSize = HOUR_TICK_MARK_STROKE_SIZE;
            for (int hour = 0; hour < 12; hour++)
            {
                double theta = hour * HOUR_TICK_ROTATION;

                PointF lineStart = DrawMath.GetScaledRadialLineEndPoint(canvasData.Center, RIM_INNER_RADIUS - RIM_STROKE_SIZE / 2, TICK_MARK_START_SCALE, (float)theta, false);
                PointF lineEnd = DrawMath.GetScaledRadialLineEndPoint(canvasData.Center, RIM_INNER_RADIUS - RIM_STROKE_SIZE / 2, HOUR_TICK_MARK_END_SCALE, (float)theta, false);

                canvas.DrawLine(lineStart, lineEnd);
            }
        }

        // draws the minute tick marks
        private static void DrawMinuteTickMarks(ICanvas canvas, RectF canvasData)
        {
            canvas.StrokeColor = MINUTE_TICK_MARK_STROKE_COLOR;
            canvas.StrokeSize = MINUTE_TICK_MARK_STROKE_SIZE;
            for (int minute = 0; minute < 60; minute++)
            {
                if (minute % 5 != 0)
                {
                    double theta = minute * MINUTE_TICK_ROTATION;

                    PointF lineStart = DrawMath.GetScaledRadialLineEndPoint(canvasData.Center, RIM_INNER_RADIUS - RIM_STROKE_SIZE / 2, TICK_MARK_START_SCALE, (float)theta, false);
                    PointF lineEnd = DrawMath.GetScaledRadialLineEndPoint(canvasData.Center, RIM_INNER_RADIUS - RIM_STROKE_SIZE / 2, MINUTE_TICK_MARK_END_SCALE, (float)theta, false);

                    canvas.DrawLine(lineStart, lineEnd);
                }
            }
        }

        // draws a clock hand
        private static void DrawHand(ICanvas canvas, RectF canvasData, Color strokeColor, float strokeSize, float startScale, float endScale, float theta)
        {
            canvas.StrokeColor = strokeColor;
            canvas.StrokeSize = strokeSize;

            PointF lineStart = DrawMath.GetScaledRadialLineEndPoint(canvasData.Center, RIM_INNER_RADIUS - RIM_STROKE_SIZE / 2, startScale, theta, true);
            PointF lineEnd = DrawMath.GetScaledRadialLineEndPoint(canvasData.Center, RIM_INNER_RADIUS - RIM_STROKE_SIZE / 2, endScale, theta, false);

            canvas.DrawLine(lineStart, lineEnd);
        }
    }
}
