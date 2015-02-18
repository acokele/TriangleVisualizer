using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TriangleVisualizer
{
    public class TrianglePoint
    {
        public float X { get; set; }
        public float Y { get; set; }

        public readonly static TrianglePoint Zero = new TrianglePoint();

        public TrianglePoint(float x, float y)
        {
            X = x; Y = y;
        }

        public TrianglePoint() : this(0, 0)
        {}

        public static TrianglePoint operator+(TrianglePoint a, TrianglePoint b)
        {
            return new TrianglePoint(a.X + b.X, a.Y + b.Y);
        }

        public static TrianglePoint operator-(TrianglePoint a, TrianglePoint b)
        {
            return new TrianglePoint(a.X - b.X, a.Y - b.Y);
        }

        public static TrianglePoint operator*(float a, TrianglePoint p)
        {
            return new TrianglePoint(a * p.X, a * p.Y);
        }

        public static TrianglePoint operator*(TrianglePoint p, float a)
        {
            return new TrianglePoint(a * p.X, a * p.Y);
        }

        public static implicit operator PointF(TrianglePoint p)
        {
            return new PointF(p.X, p.Y);
        }

        public static TrianglePoint operator/(TrianglePoint p, float a)
        {
            return new TrianglePoint(p.X / a, p.Y / a);
        }

        public static implicit operator TrianglePoint(PointF p)
        {
            return new TrianglePoint(p.X, p.Y);
        }
    }

    public class Triangle
    {
        public TrianglePoint A { get; set; }
        public TrianglePoint B { get; set; }
        public TrianglePoint C { get; set; }

        public float SideA { get; private set; }
        public float SideB { get; private set; }
        public float SideC { get; private set; }

        public float Alpha { get; private set; }
        public float Beta { get; private set; }
        public float Gamma { get; private set; }

        public float Area { get; private set; }
        public float Perimeter { get; private set; }
        public bool IsTriangle { get; private set; }
        
        public Triangle(TrianglePoint _a, TrianglePoint _b, TrianglePoint _c)
        {
            A = _a;
            B = _b;
            C = _c;

            UpdateAll();
        }

        public TrianglePoint TrilinearToCartesian(float x, float y, float z)
        {
            float ax = SideA * x;
            float by = SideB * y;
            float cz = SideC * z;

            float denominator = ax + by + cz;

            return (ax * A + by * B + cz * C) / denominator;
        }

        public TrianglePoint BarycentricToCartesian(float x, float y, float z)
        {
            return x * A + y * B + z * C;
        }

        public TrianglePoint this[int index]
        {
            get
            {
                if (index == 0)
                    return A;
                else if (index == 1)
                    return B;
                else if (index == 2)
                    return C;
                return null;
            }
            set
            {
                if (index < 0 || index >= 3)
                    throw new IndexOutOfRangeException("Only points 0, 1 and 2 exist");

                if (index == 0) A = value;
                else if (index == 1) B = value;
                else C = value;
            }

        }

        public void UpdateAll()
        {
            SideA = Helpers.Distance(B, C);
            SideB = Helpers.Distance(A, C);
            SideC = Helpers.Distance(A, B);

            // Law of cosines
            Alpha = (float)Math.Acos((-SideA * SideA + SideB * SideB + SideC * SideC) / (2 * SideB * SideC));
            Beta =  (float)Math.Acos(( SideA * SideA - SideB * SideB + SideC * SideC) / (2 * SideA * SideC));
            Gamma = (float)Math.Acos(( SideA * SideA + SideB * SideB - SideC * SideC) / (2 * SideA * SideB));

            float D = A.X * (B.Y - C.Y)
                + B.X * (C.Y - A.Y)
                + C.X * (A.Y - B.Y);

            Area = Math.Abs(D / 2);
            Perimeter = SideA + SideB + SideC;
            IsTriangle = Area > 0.1;
        }

    }
}
