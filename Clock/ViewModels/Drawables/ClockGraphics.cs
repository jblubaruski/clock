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
    internal class ClockGraphics : IDrawable
    {

        private readonly ClockGraphicsData DrawData = new();

        /*
         * Rotations are in radians with 0 radians at the top of the clock with numbers increasing in the clockwise direction.
         * At 2 * PI radians, rotations are set back to 0.
         */

        /// <summary>
        /// Current rotation of the second hand.
        /// </summary>
        public double SecondHandRotation { get; set; }
        /// <summary>
        /// Current rotation of the minute hand.
        /// </summary>
        public double MinuteHandRotation { get; set; }
        /// <summary>
        /// Current rotation of the hour hand.
        /// </summary>
        public double HourHandRotation { get; set; }

        public void Draw(ICanvas canvas, RectF canvasData)
        {
            DrawHand(canvas, canvasData, DrawData.HourHandStrokeColor, DrawData.HourHandStrokeSize, DrawData.HourHandStartScale, DrawData.HourHandEndScale, (float)HourHandRotation);
            DrawHand(canvas, canvasData, DrawData.MinuteHandStrokeColor, DrawData.MinuteHandStrokeSize, DrawData.MinuteHandStartScale, DrawData.MinuteHandEndScale, (float)MinuteHandRotation);
            DrawHand(canvas, canvasData, DrawData.SecondHandStrokeColor, DrawData.SecondHandStrokeSize, DrawData.SecondHandStartScale, DrawData.SecondHandEndScale, (float)SecondHandRotation);
        }

        // draws a clock hand
        private void DrawHand(ICanvas canvas, RectF canvasData, Color strokeColor, float strokeSize, float startScale, float endScale, float theta)
        {
            canvas.StrokeColor = strokeColor;
            canvas.StrokeSize = strokeSize;

            float distance = DrawData.RimInnerRadius - DrawData.RimStrokeSize / 2;

            PointF lineStart = DrawMath.GetScaledRadialLineEndPoint(canvasData.Center, distance, startScale, theta, true);
            PointF lineEnd = DrawMath.GetScaledRadialLineEndPoint(canvasData.Center, distance, endScale, theta, false);

            canvas.DrawLine(lineStart, lineEnd);
        }
    }
}
