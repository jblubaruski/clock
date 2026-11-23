using Clock.Services.Drawing;
using System;
using System.Collections.Generic;
using System.Text;

namespace Clock.ViewModels.Drawables
{
    internal class ClockStaticGraphics : IDrawable
    {
        private readonly ClockGraphicsData DrawData = new();

        public void Draw(ICanvas canvas, RectF canvasData)
        {
            DrawWall(canvas, canvasData);
            DrawFace(canvas, canvasData);
            DrawHourTickMarks(canvas, canvasData);
            DrawMinuteTickMarks(canvas, canvasData);
        }

        // draws the wall behind the clock
        private void DrawWall(ICanvas canvas, RectF canvasData)
        {
            float wallStripeThickness = canvasData.Width / DrawData.NumberOfStripesOnWall;
            bool paintingStripeColor1 = true;

            for (int i = 0; i < DrawData.NumberOfStripesOnWall; i++)
            {
                if (paintingStripeColor1)
                    canvas.FillColor = DrawData.WallStripeColor1;
                else
                    canvas.FillColor = DrawData.WallStripeColor2;

                paintingStripeColor1 = !paintingStripeColor1;

                float leftX = canvasData.X + wallStripeThickness * i;
                RectF stripeBox = new(leftX, canvasData.Y, wallStripeThickness, canvasData.Height);
                canvas.FillRectangle(stripeBox);
            }
        }

        // draws the rim
        private void DrawFace(ICanvas canvas, RectF canvasData)
        {
            canvas.FillColor = DrawData.RimFillColor;
            canvas.FillCircle(canvasData.Center, DrawData.RimOuterRadius);

            canvas.FillColor = DrawData.FaceFillColor;
            canvas.FillCircle(canvasData.Center, DrawData.RimInnerRadius);

            canvas.StrokeColor = DrawData.RimStrokeColor;
            canvas.StrokeSize = DrawData.RimStrokeSize;
            canvas.DrawCircle(canvasData.Center, DrawData.RimOuterRadius);
            canvas.DrawCircle(canvasData.Center, DrawData.RimInnerRadius);
        }

        // draws the hour tick marks
        private void DrawHourTickMarks(ICanvas canvas, RectF canvasData)
        {
            canvas.StrokeColor = DrawData.HourTickMarkStrokeColor;
            canvas.StrokeSize = DrawData.HourTickMarkStrokeSize;

            for (int hour = 0; hour < 12; hour++)
            {
                double theta = hour * ClockGraphicsData.HourTickRotation;
                float distance = DrawData.RimInnerRadius - DrawData.RimStrokeSize / 2;

                PointF lineStart = DrawMath.GetScaledRadialLineEndPoint(canvasData.Center, distance, DrawData.TickMarkStartScale, (float)theta, false);
                PointF lineEnd = DrawMath.GetScaledRadialLineEndPoint(canvasData.Center, distance, DrawData.HourTickMarkEndScale, (float)theta, false);

                canvas.DrawLine(lineStart, lineEnd);
            }
        }

        // draws the minute tick marks
        private void DrawMinuteTickMarks(ICanvas canvas, RectF canvasData)
        {
            canvas.StrokeColor = DrawData.MinuteTickMarkStrokeColor;
            canvas.StrokeSize = DrawData.MinuteTickMarkStrokeSize;

            for (int minute = 0; minute < 60; minute++)
            {
                if (minute % 5 != 0)
                {
                    double theta = minute * ClockGraphicsData.MinuteTickRotation;
                    float distance = DrawData.RimInnerRadius - DrawData.RimStrokeSize / 2;

                    PointF lineStart = DrawMath.GetScaledRadialLineEndPoint(canvasData.Center, distance, DrawData.TickMarkStartScale, (float)theta, false);
                    PointF lineEnd = DrawMath.GetScaledRadialLineEndPoint(canvasData.Center, distance, DrawData.MinuteTickMarkEndScale, (float)theta, false);

                    canvas.DrawLine(lineStart, lineEnd);
                }
            }
        }
    }
}
