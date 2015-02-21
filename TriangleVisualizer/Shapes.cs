using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TriangleVisualizer
{
    public interface IDrawable
    {
        void Draw(Graphics g);
    }

    public interface ISelectable
    {
        bool Selected { get; }
        bool UpdateSelected(PointF point);
    }

    public class Line : IDrawable, ISelectable
    {
        PointF StartPoint { get; set; }
        PointF EndPoint { get; set; }
        Pen Pen { get; set; }

        public bool Selected { get; private set; }

        public Line(Pen p, PointF start, PointF end)
        { Pen = p; StartPoint = start; EndPoint = end; Selected = false; }

        public void Draw(Graphics g)
        {
            if (!Selected)
                g.DrawLine(Pen, StartPoint, EndPoint);
            else
                g.DrawLine(new Pen(Color.Green, 4), StartPoint, EndPoint);
        }

        public bool UpdateSelected(PointF point)
        {
            float D = StartPoint.X * (EndPoint.Y - point.Y)
               + EndPoint.X * (point.Y - StartPoint.Y)
               + point.X * (StartPoint.Y - EndPoint.Y);

            float area = Math.Abs(D / 2);
            float side = Helpers.Distance(StartPoint, EndPoint);
            float pointToLineDistance = 2 * area / side;

            bool selected = pointToLineDistance < 5;
            /*if (selected && (Helpers.Distance(point, StartPoint) > side || Helpers.Distance(point, EndPoint) > side))
                selected = false;*/

            if (Selected == selected)
                return false;

            Selected = selected;
            return true;
            System.Diagnostics.Debug.WriteLine(pointToLineDistance);
        }

    }
}
