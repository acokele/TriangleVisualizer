﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TriangleVisualizer
{
    public class CoordinateChangedEventArgs : EventArgs
    {
        private float _oldValue;
        private float _newValue;

        public float OldValue { get { return _oldValue; } private set { _oldValue = value; } }
        public float NewValue { get { return _newValue; } private set { _newValue = value; } }

        public CoordinateChangedEventArgs(float oldValue, float newValue)
        {
            OldValue = oldValue; NewValue = newValue;
        }

    }

    public delegate void CoordinateChangedEventHandler(object sender, CoordinateChangedEventArgs e);


    

    public class Triangle
    {
        /// <summary>
        /// Coordinates of point A.
        /// </summary>
        public TrianglePoint A { get; set; }

        /// <summary>
        /// Coordinates of point B.
        /// </summary>
        public TrianglePoint B { get; private set; }

        /// <summary>
        /// Coordinates of point C.
        /// </summary>
        public TrianglePoint C { get; private set; }

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
