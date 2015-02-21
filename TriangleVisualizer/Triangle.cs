using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TriangleVisualizer
{

    public class Triangle
    {
        private TrianglePoint _a;
        private TrianglePoint _b;
        private TrianglePoint _c;

        public event ValueChangedEventHandler<TrianglePoint> ACoordinatesChanged;
        public event ValueChangedEventHandler<TrianglePoint> BCoordinatesChanged;
        public event ValueChangedEventHandler<TrianglePoint> CCoordinatesChanged;

        protected virtual void OnACoordinatesChanged(ValueChangedEventArgs<TrianglePoint> e)
        {
            if (ACoordinatesChanged != null)
                ACoordinatesChanged(this, e);
        }

        protected virtual void OnBCoordinatesChanged(ValueChangedEventArgs<TrianglePoint> e)
        {
            if (BCoordinatesChanged != null)
                BCoordinatesChanged(this, e);
        }

        protected virtual void OnCCoordinatesChanged(ValueChangedEventArgs<TrianglePoint> e)
        {
            if (CCoordinatesChanged != null)
                CCoordinatesChanged(this, e);
        }

        /// <summary>
        /// Coordinates of point A.
        /// </summary>
        public TrianglePoint A
        {
            get
            {
                return _a;
            }
            set
            {
                TrianglePoint oldValue;
                if (A == null)
                    oldValue = new TrianglePoint(0, 0);
                else
                    oldValue = new TrianglePoint(A.X, A.Y);

                _a = value;
                OnACoordinatesChanged(new ValueChangedEventArgs<TrianglePoint>(oldValue, A));
            }
        }

        /// <summary>
        /// Coordinates of point B.
        /// </summary>
        public TrianglePoint B
        {
            get
            {
                return _b;
            }
            set
            {
                TrianglePoint oldValue;
                if (B == null)
                    oldValue = new TrianglePoint(0, 0);
                else
                    oldValue = new TrianglePoint(B.X, B.Y);
                _b = value;
                OnACoordinatesChanged(new ValueChangedEventArgs<TrianglePoint>(oldValue, B));
            }
        }

        /// <summary>
        /// Coordinates of point C.
        /// </summary>
        public TrianglePoint C
        {
            get
            {
                return _c;
            }
            set
            {
                TrianglePoint oldValue;
                if (C == null)
                    oldValue = new TrianglePoint(0, 0);
                else
                    oldValue = new TrianglePoint(C.X, C.Y);
                _c = value;
                OnACoordinatesChanged(new ValueChangedEventArgs<TrianglePoint>(oldValue, C));
            }
        }

        /// <summary>
        /// Length of side BC.
        /// </summary>
        public float SideA { get; private set; }

        /// <summary>
        /// Length of side AC.
        /// </summary>
        public float SideB { get; private set; }

        /// <summary>
        /// Length of side AB.
        /// </summary>
        public float SideC { get; private set; }

        /// <summary>
        /// Angle BAC (in radians).
        /// </summary>
        public float Alpha { get; private set; }

        /// <summary>
        /// Angle ABC (in radians).
        /// </summary>
        public float Beta { get; private set; }

        /// <summary>
        /// Angle ACB (in radians).
        /// </summary>
        public float Gamma { get; private set; }

        /// <summary>
        /// Area of the triangle.
        /// </summary>
        public float Area { get; private set; }

        /// <summary>
        /// Perimeter of the triangle.
        /// </summary>
        public float Perimeter { get; private set; }

        /// <summary>
        /// False if points A, B and C are colinear, true otherwise/
        /// </summary>
        public bool IsTriangle { get; private set; }

        public Triangle(TrianglePoint _a, TrianglePoint _b, TrianglePoint _c)
        {
            A = _a;
            B = _b;
            C = _c;

            A.XCoordinateChanged += (sender, e) =>
            {
                TrianglePoint oldA = new TrianglePoint(e.OldValue, A.Y);
                OnACoordinatesChanged(new ValueChangedEventArgs<TrianglePoint>(oldA, A));
            };

            A.YCoordinateChanged += (sender, e) =>
            {
                TrianglePoint oldA = new TrianglePoint(A.X, e.OldValue);
                OnACoordinatesChanged(new ValueChangedEventArgs<TrianglePoint>(oldA, A));
            };

            B.XCoordinateChanged += (sender, e) =>
            {
                TrianglePoint oldB = new TrianglePoint(e.OldValue, B.Y);
                OnACoordinatesChanged(new ValueChangedEventArgs<TrianglePoint>(oldB, B));
            };

            B.YCoordinateChanged += (sender, e) =>
            {
                TrianglePoint oldB = new TrianglePoint(B.X, e.OldValue);
                OnACoordinatesChanged(new ValueChangedEventArgs<TrianglePoint>(oldB, B));
            };

            C.XCoordinateChanged += (sender, e) =>
            {
                TrianglePoint oldC = new TrianglePoint(e.OldValue, C.Y);
                OnACoordinatesChanged(new ValueChangedEventArgs<TrianglePoint>(oldC, C));
            };

            C.YCoordinateChanged += (sender, e) =>
            {
                TrianglePoint oldC = new TrianglePoint(C.X, e.OldValue);
                OnACoordinatesChanged(new ValueChangedEventArgs<TrianglePoint>(oldC, C));
            };

            ACoordinatesChanged += (sender, e) => UpdateAll();
            BCoordinatesChanged += (sender, e) => UpdateAll();
            CCoordinatesChanged += (sender, e) => UpdateAll();

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

        private void UpdateAll()
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
