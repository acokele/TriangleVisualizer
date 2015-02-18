using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TriangleVisualizer
{
    public static class Helpers
    {
        public static float DistanceSquared(PointF point1, PointF point2)
        {
            float deltaX = point1.X - point2.X;
            float deltaY = point1.Y - point2.Y;
            return deltaX * deltaX + deltaY * deltaY;
        }

        public static float Distance(PointF point1, PointF point2)
        {
            return (float)Math.Sqrt(DistanceSquared(point1, point2));
        }

        public static float DotProduct(PointF point1, PointF point2)
        {
            return point1.X * point2.X + point1.Y * point2.Y;
        }

        public static float Intensity(PointF point)
        {
            return Distance(point, new PointF(0, 0));
        }

        public static PointF Normalize(PointF point)
        {
            float length = Intensity(point);
            return new PointF(point.X / length, point.Y / length);
        }

        public static RectangleF EllipseToRectangle(PointF center, float radius)
        {
            return new RectangleF(center.X - radius, center.Y - radius, 2 * radius, 2 * radius);
        }

        public static float Cosine(PointF point1, PointF vertex, PointF point2)
        {
            PointF vector1 = new PointF(point1.X - vertex.X, point1.Y - vertex.Y);
            PointF vector2 = new PointF(point2.X - vertex.X, point2.Y - vertex.Y);

            float v1 = Intensity(vector1), v2 = Intensity(vector2);
            return DotProduct(vector1, vector2) / (v1 * v2);
        }

        public static float Sine(PointF point1, PointF vertex, PointF point2)
        {
            float cosine = Cosine(point1, vertex, point2);
            return (float)Math.Sqrt(1 - cosine * cosine);
        }
    }
}
