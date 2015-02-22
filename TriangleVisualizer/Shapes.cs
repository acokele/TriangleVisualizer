using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TriangleVisualizer
{
    public interface IDrawable
    {
        Pen NormalPen { get; set; }
        void Draw(Graphics g);
    }

    public interface IHoverable
    {
        Pen HoveredPen { get; set; }
        bool IsHovered { get; }
        bool UpdateHovered(PointF point);
    }

    public class Line : IDrawable, IHoverable
    {
        public PointF StartPoint { get; set; }
        public PointF EndPoint { get; set; }
        public Pen NormalPen { get; set; }
        public Pen HoveredPen { get; set; }

        public bool IsHovered { get; private set; }

        public Line(PointF start, PointF end, Pen normal, Pen hovered)
        {
            NormalPen = normal; HoveredPen = hovered;
            StartPoint = start; EndPoint = end; IsHovered = false; }

        public void Draw(Graphics g)
        {
            if (!IsHovered)
                g.DrawLine(NormalPen, StartPoint, EndPoint);
            else
                g.DrawLine(HoveredPen, StartPoint, EndPoint);
        }

        public bool UpdateHovered(PointF point)
        {
            float D = StartPoint.X * (EndPoint.Y - point.Y)
               + EndPoint.X * (point.Y - StartPoint.Y)
               + point.X * (StartPoint.Y - EndPoint.Y);

            float area = Math.Abs(D / 2);
            float side = Helpers.Distance(StartPoint, EndPoint);
            float pointToLineDistance = 2 * area / side;

            bool hovered = pointToLineDistance < 5;
            if (hovered && (Helpers.Distance(point, StartPoint) > side || Helpers.Distance(point, EndPoint) > side))
                hovered = false;

            if (IsHovered == hovered)
                return false;

            IsHovered = hovered;
            return true;
        }

    }
}
