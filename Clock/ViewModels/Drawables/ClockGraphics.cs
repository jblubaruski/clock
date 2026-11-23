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
    public class ClockGraphics : IDrawable
    {
        public ClockGraphicsData drawData = new();

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
            DrawWall(canvas, canvasData);
            DrawRim(canvas, canvasData);
            DrawHourTickMarks(canvas, canvasData);
            DrawMinuteTickMarks(canvas, canvasData);
            DrawHand(canvas, canvasData, drawData.HourHandStrokeColor, drawData.HourHandStrokeSize, drawData.HourHandStartScale, drawData.HourHandEndScale, (float)HourHandRotation);
            DrawHand(canvas, canvasData, drawData.MinuteHandStrokeColor, drawData.MinuteHandStrokeSize, drawData.MinuteHandStartScale, drawData.MinuteHandEndScale, (float)MinuteHandRotation);
            DrawHand(canvas, canvasData, drawData.SecondHandStrokeColor, drawData.SecondHandStrokeSize, drawData.SecondHandStartScale, drawData.SecondHandEndScale, (float)SecondHandRotation);
        }

        // draws the wall behind the clock
        private void DrawWall(ICanvas canvas, RectF canvasData)
        {
            float wallStripeThickness = canvasData.Width / drawData.NumberOfStripesOnWall;
            bool paintingStripeColor1 = true;

            for (int i = 0; i < drawData.NumberOfStripesOnWall; i++)
            {
                if (paintingStripeColor1)
                    canvas.FillColor = drawData.WallStripeColor1;
                else
                    canvas.FillColor = drawData.WallStripeColor2;

                paintingStripeColor1 = !paintingStripeColor1;

                float leftX = canvasData.X + wallStripeThickness * i;
                RectF stripeBox = new(leftX, canvasData.Y, wallStripeThickness, canvasData.Height);
                canvas.FillRectangle(stripeBox);
            }
        }

        // draws the rim
        private void DrawRim(ICanvas canvas, RectF canvasData)
        {
            canvas.FillColor = drawData.RimFillColor;
            canvas.FillCircle(canvasData.Center, drawData.RimOuterRadius);

            canvas.FillColor = drawData.FaceFillColor;
            canvas.FillCircle(canvasData.Center, drawData.RimInnerRadius);

            canvas.StrokeColor = drawData.RimStrokeColor;
            canvas.StrokeSize = drawData.RimStrokeSize;
            canvas.DrawCircle(canvasData.Center, drawData.RimOuterRadius);
            canvas.DrawCircle(canvasData.Center, drawData.RimInnerRadius);
        }

        // draws the hour tick marks
        private void DrawHourTickMarks(ICanvas canvas, RectF canvasData)
        {
            canvas.StrokeColor = drawData.HourTickMarkStrokeColor;
            canvas.StrokeSize = drawData.HourTickMarkStrokeSize;
            for (int hour = 0; hour < 12; hour++)
            {
                double theta = hour * HOUR_TICK_ROTATION;
                float distance = drawData.RimInnerRadius - drawData.RimStrokeSize / 2;
                
                PointF lineStart = DrawMath.GetScaledRadialLineEndPoint(canvasData.Center, distance, drawData.TickMarkStartScale, (float)theta, false);
                PointF lineEnd = DrawMath.GetScaledRadialLineEndPoint(canvasData.Center, distance, drawData.HourTickMarkEndScale, (float)theta, false);

                canvas.DrawLine(lineStart, lineEnd);
            }
        }

        // draws the minute tick marks
        private void DrawMinuteTickMarks(ICanvas canvas, RectF canvasData)
        {
            canvas.StrokeColor = drawData.MinuteTickMarkStrokeColor;
            canvas.StrokeSize = drawData.MinuteTickMarkStrokeSize;
            for (int minute = 0; minute < 60; minute++)
            {
                if (minute % 5 != 0)
                {
                    double theta = minute * MINUTE_TICK_ROTATION;
                    float distance = drawData.RimInnerRadius - drawData.RimStrokeSize / 2;

                    PointF lineStart = DrawMath.GetScaledRadialLineEndPoint(canvasData.Center, distance, drawData.TickMarkStartScale, (float)theta, false);
                    PointF lineEnd = DrawMath.GetScaledRadialLineEndPoint(canvasData.Center, distance, drawData.MinuteTickMarkEndScale, (float)theta, false);

                    canvas.DrawLine(lineStart, lineEnd);
                }
            }
        }

        // draws a clock hand
        private void DrawHand(ICanvas canvas, RectF canvasData, Color strokeColor, float strokeSize, float startScale, float endScale, float theta)
        {
            canvas.StrokeColor = strokeColor;
            canvas.StrokeSize = strokeSize;

            float distance = drawData.RimInnerRadius - drawData.RimStrokeSize / 2;

            PointF lineStart = DrawMath.GetScaledRadialLineEndPoint(canvasData.Center, distance, startScale, theta, true);
            PointF lineEnd = DrawMath.GetScaledRadialLineEndPoint(canvasData.Center, distance, endScale, theta, false);

            canvas.DrawLine(lineStart, lineEnd);
        }
    }
}
