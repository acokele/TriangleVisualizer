using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace TriangleVisualizer
{
    public class ValueChangedEventArgs<T> : EventArgs
    {
        private T _oldValue;
        private T _newValue;

        public T OldValue { get { return _oldValue; } private set { _oldValue = value; } }
        public T NewValue { get { return _newValue; } private set { _newValue = value; } }

        public ValueChangedEventArgs(T oldValue, T newValue)
        {
            OldValue = oldValue; NewValue = newValue;
        }

    }

    public delegate void ValueChangedEventHandler<T>(object sender, ValueChangedEventArgs<T> e);

    public class TrianglePoint
    {

        private float _x;
        private float _y;

        public event ValueChangedEventHandler<float> XCoordinateChanged;
        public event ValueChangedEventHandler<float> YCoordinateChanged;

        protected virtual void OnXCoordinateChanged(ValueChangedEventArgs<float> e)
        {
            if (XCoordinateChanged != null)
                XCoordinateChanged(this, e);
        }

        protected virtual void OnYCoordinateChanged(ValueChangedEventArgs<float> e)
        {
            if (YCoordinateChanged != null)
                YCoordinateChanged(this, e);
        }

        /// <summary>
        /// Gets or sets the X coordinate of the point.
        /// </summary>
        public float X
        {
            get
            {
                return _x;
            }
            set
            {
                float oldValue = _x;
                _x = value;

                OnXCoordinateChanged(new ValueChangedEventArgs<float>(oldValue, value));
            }
        }
        /// <summary>
        /// Gets or sets the Y coordinate of the point.
        /// </summary>
        public float Y
        {
            get
            {
                return _y;
            }
            set
            {
                float oldValue = _y;
                _y = value;

                OnYCoordinateChanged(new ValueChangedEventArgs<float>(oldValue, value));
            }
        }

        /// <summary>
        /// Point with coordinates (0, 0).
        /// </summary>
        public readonly static TrianglePoint Zero = new TrianglePoint();

        /// <summary>
        /// Creates a new point with specified coordinates.
        /// </summary>
        /// <param name="x">The point's X coordinate.</param>
        /// <param name="y">The point's Y coordinate.</param>
        public TrianglePoint(float x, float y)
        {
            X = x; Y = y;
        }

        /// <summary>
        /// Creates a new point with coordinates (0, 0)
        /// </summary>
        public TrianglePoint()
            : this(0, 0)
        { }

        #region Operators
        public static TrianglePoint operator +(TrianglePoint a, TrianglePoint b)
        {
            return new TrianglePoint(a.X + b.X, a.Y + b.Y);
        }

        public static TrianglePoint operator -(TrianglePoint a, TrianglePoint b)
        {
            return new TrianglePoint(a.X - b.X, a.Y - b.Y);
        }

        public static TrianglePoint operator *(float a, TrianglePoint p)
        {
            return new TrianglePoint(a * p.X, a * p.Y);
        }

        public static TrianglePoint operator *(TrianglePoint p, float a)
        {
            return new TrianglePoint(a * p.X, a * p.Y);
        }

        public static TrianglePoint operator /(TrianglePoint p, float a)
        {
            return new TrianglePoint(p.X / a, p.Y / a);
        }

        #endregion

        public static implicit operator PointF(TrianglePoint p)
        {
            return new PointF(p.X, p.Y);
        }

        public static implicit operator TrianglePoint(PointF p)
        {
            return new TrianglePoint(p.X, p.Y);
        }
    }
}
