using System;
using System.Collections.Generic;
using System.Text;

namespace Clock.Services.Drawing
{
    /// <summary>
    /// Utility class containing calculations related to drawing graphics.
    /// </summary>
    public static class DrawMath
    {
        /// <summary>
        /// Returns a point a certain scaled distance away from the center of a circle.
        /// </summary>
        /// 
        /// <param name="center">
        /// The center of the point's circle.
        /// </param>
        /// <param name="distance">
        /// The point's unscaled distance from the center of the circle.
        /// </param>
        /// <param name="distanceScale">
        /// The scale value for the distance.
        /// </param>
        /// <param name="theta">
        /// The clockwise rotating angle from the up direction in radians.
        /// </param>
        /// <param name="reverse">
        /// whether or not the point is on the opposite side of the center from where the calculation would otherwise place it.
        /// </param>
        /// 
        /// <returns>
        /// The calculated end point as a PointF.
        /// </returns>
        public static PointF GetScaledRadialLineEndPoint(PointF center, float distance, float distanceScale, float theta, bool reverse)
        {
            int reversal = reverse ? -1 : 1;

            float pointX = center.X + reversal * distanceScale * distance * MathF.Sin(theta);
            float pointY = center.Y - reversal * distanceScale * distance * MathF.Cos(theta);

            return new(pointX, pointY);
        }
    }
}
