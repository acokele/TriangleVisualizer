using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TriangleVisualizer
{
    public interface IVisualizer
    {
        void Visualize(Graphics g, Triangle triangle);
    }

    public class VisualizerData
    {
        public IVisualizer Visualizer { get; set; }
        public bool Active { get; set; }
        public string Name { get; set; }
    }

    public class VisualizerGroup : IVisualizer, IEnumerable<VisualizerData>
    {
        
        List<VisualizerData> visualizers = new List<VisualizerData>();
        public string Name { get; set; }

        public VisualizerGroup(string _name)
        {
            Name = _name;
        }
        public void Add(string name, IVisualizer v)
        {
            visualizers.Add(new VisualizerData
            {
                Name = name,
                Active = false,
                Visualizer = v
            });
        }

        public void Visualize(Graphics g, Triangle t)
        {
            foreach (VisualizerData data in visualizers)
                if(data.Active)
                    data.Visualizer.Visualize(g, t);
        }

        public IEnumerator<VisualizerData> GetEnumerator()
        {
            foreach (VisualizerData v in visualizers)
                yield return v;
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }

    public abstract class LineVisualizer : IVisualizer
    {
        public Pen Pen { get; set; }

        public LineVisualizer(Pen p) { Pen = p; }
        public abstract void Visualize(Graphics g, Triangle triangle);
    }

    public abstract class PointVisualizer : IVisualizer
    {
        public Brush Brush { get; set; }

        public PointVisualizer(Brush b) { Brush = b; }
        public abstract void Visualize(Graphics g, Triangle triangle);
        public static readonly float Radius = 5;
    }
   
    public class IncircleVisualizer : LineVisualizer
    {
        public IncircleVisualizer(Pen p) : base(p) { }
        public override void Visualize(Graphics g, Triangle triangle)
        {
            var incenter = triangle.TrilinearToCartesian(1, 1, 1);
            var P = triangle.Area;
            var s = triangle.Perimeter / 2;
            var r = P / s;

            g.DrawEllipse(Pen, Helpers.EllipseToRectangle(incenter, r));

        }
    }

    public class IncenterVisualizer : PointVisualizer
    {
        public IncenterVisualizer(Brush b) : base(b) { }
        public override void Visualize(Graphics g, Triangle triangle)
        {
            var incenter = triangle.TrilinearToCartesian(1, 1, 1);
            g.FillEllipse(Brush, Helpers.EllipseToRectangle(incenter, PointVisualizer.Radius));

        }
    }

    public class MediansVisualizer : LineVisualizer
    {
        public MediansVisualizer(Pen p) : base(p) { }

        public override void Visualize(Graphics g, Triangle triangle)
        {
            g.DrawLine(Pen, triangle.A, (triangle.B + triangle.C) / 2);
            g.DrawLine(Pen, triangle.B, (triangle.A + triangle.C) / 2);
            g.DrawLine(Pen, triangle.C, (triangle.A + triangle.B) / 2);
        }
    }

    public class CentroidVisualizer : PointVisualizer
    {
        public CentroidVisualizer(Brush b) : base(b) { }

        public override void Visualize(Graphics g, Triangle triangle)
        {
            var centroid = GetCentroid(triangle);
            g.FillEllipse(Brush, Helpers.EllipseToRectangle(centroid, Radius));
        }

        public static TrianglePoint GetCentroid(Triangle triangle)
        {
            return triangle.BarycentricToCartesian(1.0f / 3, 1.0f / 3, 1.0f / 3);
        }
    }

    public class CircumcircleVisualizer : LineVisualizer
    {
        public CircumcircleVisualizer(Pen p) : base(p) { }

        public override void Visualize(Graphics g, Triangle triangle)
        {

            var circumcenter = CircumcenterVisualizer.GetCircumcenter(triangle);
            var R = Helpers.Distance(circumcenter, triangle.A);

            g.DrawEllipse(Pen, Helpers.EllipseToRectangle(circumcenter, R));
 
        }
    }

    public class CircumcenterVisualizer : PointVisualizer
    {
        public CircumcenterVisualizer(Brush b) : base(b) { }

        public override void Visualize(Graphics g, Triangle triangle)
        {
            var circumcenter = GetCircumcenter(triangle);
            var R = Helpers.Distance(circumcenter, triangle.A);

            g.FillEllipse(Brush, Helpers.EllipseToRectangle(circumcenter, Radius));
        }

        public static TrianglePoint GetCircumcenter(Triangle triangle)
        {
            float a = triangle.SideA, b = triangle.SideB, c = triangle.SideC;
            float x = a * (-a * a + b * b + c * c);
            float y = b * (a * a - b * b + c * c);
            float z = c * (a * a + b * b - c * c);

            return triangle.TrilinearToCartesian(x, y, z);
        }
    }

    public class AltitudeVisualizer : LineVisualizer
    {
        public Pen ExtensionPen { get; set; }
        public AltitudeVisualizer(Pen p) : base(p) 
        {
            ExtensionPen = Pen;
        }

        public AltitudeVisualizer(Pen main, Pen extension)
            : base(main)
        {
            ExtensionPen = extension;
        }
        public override void Visualize(Graphics g, Triangle triangle)
        {
            TrianglePoint hA = GetAltitudeFoot(triangle.A, triangle.B, triangle.C),
                hB = GetAltitudeFoot(triangle.B, triangle.A, triangle.C),
                hC = GetAltitudeFoot(triangle.C, triangle.A, triangle.B);

            // Altitudes
            g.DrawLine(Pen, triangle.A, hA);
            g.DrawLine(Pen, triangle.B, hB);
            g.DrawLine(Pen, triangle.C, hC);

            // Extensions
            g.DrawLine(ExtensionPen, triangle.B, hA);
            g.DrawLine(ExtensionPen, triangle.A, hC);
            g.DrawLine(ExtensionPen, triangle.C, hB);

            var orthocenter = OrthocenterVisualizer.GetOrthocenter(triangle);
            g.DrawLine(ExtensionPen, hA, orthocenter);
            g.DrawLine(ExtensionPen, hB, orthocenter);
            g.DrawLine(ExtensionPen, hC, orthocenter);
        }

        private TrianglePoint GetAltitudeFoot(TrianglePoint altitudeVertex, TrianglePoint referentVertex, TrianglePoint thirdVertex)
        {
            float lengthSquared = Helpers.DistanceSquared(referentVertex, thirdVertex);

            TrianglePoint referentToAltitude = altitudeVertex - referentVertex;
            TrianglePoint referentToThird = thirdVertex - referentVertex;

            float k = Helpers.DotProduct(referentToThird, referentToAltitude) / lengthSquared;

            return referentVertex + referentToThird * k;
        }
    }

    public class OrthocenterVisualizer : PointVisualizer
    {
        public OrthocenterVisualizer(Brush b) : base(b) { }

        public override void Visualize(Graphics g, Triangle triangle)
        {
            var orthocenter = GetOrthocenter(triangle);

            g.FillEllipse(Brush, Helpers.EllipseToRectangle(orthocenter, Radius));
        }

        public static TrianglePoint GetOrthocenter(Triangle triangle)
        {
            TrianglePoint A = triangle.A, B = triangle.B, C = triangle.C;
            float sinA = Helpers.Sine(B, A, C),
                sinB = Helpers.Sine(A, B, C),
                sinC = Helpers.Sine(A, C, B),
                cosA = Helpers.Cosine(B, A, C),
                cosB = Helpers.Cosine(A, B, C),
                cosC = Helpers.Cosine(A, C, B);

            float x = cosA - sinB * sinC;
            float y = cosB - sinC * sinA;
            float z = cosC - sinA * sinB;

            return triangle.TrilinearToCartesian(x, y, z);
        }
    }

    public class EulerLineVisualizer : LineVisualizer
    {
        public EulerLineVisualizer(Pen p) : base(p){ }


        public override void Visualize(Graphics g, Triangle triangle)
        {
            var centroid = CentroidVisualizer.GetCentroid(triangle);
            var circumcenter = CircumcenterVisualizer.GetCircumcenter(triangle);

            TrianglePoint vector = Helpers.Normalize(circumcenter - centroid);

            var point1 = centroid + vector * 2000;
            var point2 = centroid - vector * 2000;

            g.DrawLine(Pen, point1, point2);
        }
    }

    public class AngleBisectorVisualizer : LineVisualizer
    {
        public AngleBisectorVisualizer(Pen p) : base(p) { }


        public override void Visualize(Graphics g, Triangle triangle)
        {
            var incenter = triangle.TrilinearToCartesian(1, 1, 1);

            g.DrawLine(Pen, triangle.A, incenter);
            g.DrawLine(Pen, triangle.B, incenter);
            g.DrawLine(Pen, triangle.C, incenter);
        }
    }

    public class ExternalAngleBisectorVisualizer : LineVisualizer
    {
        public ExternalAngleBisectorVisualizer(Pen p) : base(p) { }

        public override void Visualize(Graphics g, Triangle triangle)
        {
            TrianglePoint eA = triangle.TrilinearToCartesian(-1, 1, 1),
                eB = triangle.TrilinearToCartesian(1, -1, 1),
                eC = triangle.TrilinearToCartesian(1, 1, -1);

            g.DrawLine(Pen, eA, eB);
            g.DrawLine(Pen, eB, eC);
            g.DrawLine(Pen, eC, eA);
        }
    }

    public class SideBisectorVisualizer : LineVisualizer
    {
        int Scale { get; set; }

        public SideBisectorVisualizer(Pen p) : base(p) { Scale = 15; }
        
        public override void Visualize(Graphics g, Triangle triangle)
        {
            var circumcenter = CircumcenterVisualizer.GetCircumcenter(triangle);

            TrianglePoint midAB = (triangle.A + triangle.B) / 2,
                midAC = (triangle.A + triangle.C) / 2,
                midBC = (triangle.B + triangle.C) / 2;

            TrianglePoint midABtoCircumcenter = Helpers.Normalize(circumcenter - midAB),
                midACtoCircumcenter = Helpers.Normalize(circumcenter - midAC),
                midBCtoCircumcenter = Helpers.Normalize(circumcenter - midBC);

            g.DrawLine(Pen, midAB + midABtoCircumcenter * 10, midAB - midABtoCircumcenter * 10);
            g.DrawLine(Pen, midAC + midACtoCircumcenter * 10, midAC - midACtoCircumcenter * 10);
            g.DrawLine(Pen, midBC + midBCtoCircumcenter * 10, midBC - midBCtoCircumcenter * 10);
        }
    }

    public class ExcirclesVisualiser : LineVisualizer
    {
        public ExcirclesVisualiser(Pen p) : base(p) { }
        
        public override void Visualize(Graphics g, Triangle triangle)
        {
            TrianglePoint eA = triangle.TrilinearToCartesian(-1, 1, 1),
                eB = triangle.TrilinearToCartesian(1, -1, 1),
                eC = triangle.TrilinearToCartesian(1, 1, -1);

            float s = triangle.Perimeter / 2,
                a = triangle.SideA,
                b = triangle.SideB,
                c = triangle.SideC;

            float rA = (float)Math.Sqrt(s * (s - b) * (s - c) / (s - a)),
                rB = (float)Math.Sqrt(s * (s - a) * (s - c) / (s - b)),
                rC = (float)Math.Sqrt(s * (s - a) * (s - b) / (s - c));

            g.DrawEllipse(Pen, Helpers.EllipseToRectangle(eA, rA));
            g.DrawEllipse(Pen, Helpers.EllipseToRectangle(eB, rB));
            g.DrawEllipse(Pen, Helpers.EllipseToRectangle(eC, rC));


        }

        
    }

    public class ExcircleCentersVisualizer : PointVisualizer
    {
        public ExcircleCentersVisualizer(Brush b) : base(b) { }

        public override void Visualize(Graphics g, Triangle triangle)
        {
            TrianglePoint eA = triangle.TrilinearToCartesian(-1, 1, 1),
                eB = triangle.TrilinearToCartesian(1, -1, 1),
                eC = triangle.TrilinearToCartesian(1, 1, -1);

            g.FillEllipse(Brush, Helpers.EllipseToRectangle(eA, Radius));
            g.FillEllipse(Brush, Helpers.EllipseToRectangle(eB, Radius));
            g.FillEllipse(Brush, Helpers.EllipseToRectangle(eC, Radius));
        }
    }

    public class ExcircleTangentVisualizer : LineVisualizer
    {
         public ExcircleTangentVisualizer(Pen p) : base(p) { }

         public override void Visualize(Graphics g, Triangle triangle)
         {
             TrianglePoint cA = triangle.TrilinearToCartesian(-1, 1, 1),
                 cB = triangle.TrilinearToCartesian(1, -1, 1),
                 cC = triangle.TrilinearToCartesian(1, 1, -1);

             g.DrawLine(Pen, TangentCircleIntersection(triangle.A, triangle.B, cA), triangle.B);
             g.DrawLine(Pen, TangentCircleIntersection(triangle.A, triangle.C, cA), triangle.C);

             g.DrawLine(Pen, TangentCircleIntersection(triangle.B, triangle.A, cB), triangle.A);
             g.DrawLine(Pen, TangentCircleIntersection(triangle.B, triangle.C, cB), triangle.C);

             g.DrawLine(Pen, TangentCircleIntersection(triangle.C, triangle.B, cC), triangle.B);
             g.DrawLine(Pen, TangentCircleIntersection(triangle.C, triangle.A, cC), triangle.A);

         }

         TrianglePoint TangentCircleIntersection(TrianglePoint from, TrianglePoint to, TrianglePoint center)
         {
             TrianglePoint fromTo = to - from,
                 toCenter = center - to;
             float lengthSquared = Helpers.DistanceSquared(from, to);
             float k = Helpers.DotProduct(fromTo, toCenter) / lengthSquared;

             return 2 * k * fromTo + to;
         }
    }

    public class NinePointCircleVisualizer : LineVisualizer
    {
        public NinePointCircleVisualizer(Pen p) : base(p) { }

        public override void Visualize(Graphics g, Triangle triangle)
        {
            TrianglePoint orthocenter = OrthocenterVisualizer.GetOrthocenter(triangle),
                circumcenter = CircumcenterVisualizer.GetCircumcenter(triangle);

            TrianglePoint ninePointCenter = (orthocenter + circumcenter) / 2;
            float ninePointRadius = Helpers.Distance(circumcenter, triangle.A) / 2;

            g.DrawEllipse(Pen, Helpers.EllipseToRectangle(ninePointCenter, ninePointRadius));

        }
    }

    public class NinePointCenterVisualizer : PointVisualizer
    {
        public NinePointCenterVisualizer(Brush b) : base(b) { }

        public override void Visualize(Graphics g, Triangle triangle)
        {
            TrianglePoint orthocenter = OrthocenterVisualizer.GetOrthocenter(triangle),
                circumcenter = CircumcenterVisualizer.GetCircumcenter(triangle);

            TrianglePoint ninePointCenter = (orthocenter + circumcenter) / 2;

            g.FillEllipse(Brush, Helpers.EllipseToRectangle(ninePointCenter, Radius));
        }
    }
}
