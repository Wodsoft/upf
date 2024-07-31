using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    [TypeConverter(typeof(KeySplineConverter))]
    public class KeySpline : Freezable, IFormattable
    {
        #region Constructors

        /// <summary>
        /// Creates a new KeySpline.
        /// </summary>
        /// <remarks>
        /// Default values for control points are (0,0) and (1,1) which will
        /// have no effect on the progress of an animation or key frame.
        /// </remarks>
        public KeySpline()
            : base()
        {
            _controlPoint1 = new Point(0.0f, 0.0f);
            _controlPoint2 = new Point(1.0f, 1.0f);
        }

        /// <summary>
        /// Float constructor
        /// </summary>
        /// <param name="x1">x value for the 0,0 endpoint's control point</param>
        /// <param name="y1">y value for the 0,0 endpoint's control point</param>
        /// <param name="x2">x value for the 1,1 endpoint's control point</param>
        /// <param name="y2">y value for the 1,1 endpoint's control point</param>
        public KeySpline(float x1, float y1, float x2, float y2)
            : this(new Point(x1, y1), new Point(x2, y2))
        {
        }

        /// <summary>
        /// Point constructor
        /// </summary>
        /// <param name="controlPoint1">the control point for the 0,0 endpoint</param>
        /// <param name="controlPoint2">the control point for the 1,1 endpoint</param>
        public KeySpline(Point controlPoint1, Point controlPoint2)
            : base()
        {
            if (!IsValidControlPoint(controlPoint1))
            {
                throw new ArgumentException($"Invalid value \"{controlPoint1}\".", "controlPoint1");
            }

            if (!IsValidControlPoint(controlPoint2))
            {
                throw new ArgumentException($"Invalid value \"{controlPoint2}\".", "controlPoint2");
            }

            _controlPoint1 = controlPoint1;
            _controlPoint2 = controlPoint2;

            _isDirty = true;
        }

        #endregion Constructors

        #region Freezable

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new KeySpline();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CloneCore(System.Windows.Freezable)">Freezable.CloneCore</see>.
        /// </summary>
        /// <param name="sourceFreezable">The KeySpline to copy.</param>
        protected override void CloneCore(Freezable sourceFreezable)
        {
            KeySpline sourceKeySpline = (KeySpline)sourceFreezable;
            base.CloneCore(sourceFreezable);
            CloneCommon(sourceKeySpline);
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CloneCurrentValueCore(System.Windows.Freezable)">Freezable.CloneCurrentValueCore</see>.
        /// </summary>
        /// <param name="sourceFreezable">The KeySpline to copy.</param>
        protected override void CloneCurrentValueCore(Freezable sourceFreezable)
        {
            KeySpline sourceKeySpline = (KeySpline)sourceFreezable;
            base.CloneCurrentValueCore(sourceFreezable);
            CloneCommon(sourceKeySpline);
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.GetAsFrozenCore(System.Windows.Freezable)">Freezable.GetAsFrozenCore</see>.
        /// </summary>
        /// <param name="sourceFreezable">The KeySpline to copy.</param>
        protected override void GetAsFrozenCore(Freezable sourceFreezable)
        {
            KeySpline sourceKeySpline = (KeySpline)sourceFreezable;
            base.GetAsFrozenCore(sourceFreezable);
            CloneCommon(sourceKeySpline);
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.GetCurrentValueAsFrozenCore(System.Windows.Freezable)">Freezable.GetCurrentValueAsFrozenCore</see>.
        /// </summary>
        /// <param name="sourceFreezable">The KeySpline to copy.</param>
        protected override void GetCurrentValueAsFrozenCore(Freezable sourceFreezable)
        {
            KeySpline sourceKeySpline = (KeySpline)sourceFreezable;
            base.GetCurrentValueAsFrozenCore(sourceFreezable);
            CloneCommon(sourceKeySpline);
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.OnChanged">Freezable.OnChanged</see>.
        /// </summary>
        protected override void OnChanged()
        {
            _isDirty = true;

            base.OnChanged();
        }

        #endregion

        #region Public

        /// <summary>
        /// 
        /// </summary>
        public Point ControlPoint1
        {
            get
            {
                ReadPreamble();

                return _controlPoint1;
            }
            set
            {
                WritePreamble();

                if (value != _controlPoint1)
                {
                    if (!IsValidControlPoint(value))
                    {
                        throw new ArgumentException($"Invalid value \"{value}\".", "value");
                    }

                    _controlPoint1 = value;

                    WritePostscript();
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        public Point ControlPoint2
        {
            get
            {
                ReadPreamble();

                return _controlPoint2;
            }
            set
            {
                WritePreamble();

                if (value != _controlPoint2)
                {
                    if (!IsValidControlPoint(value))
                    {
                        throw new ArgumentException($"Invalid value \"{value}\".", "value");
                    }

                    _controlPoint2 = value;

                    WritePostscript();
                }
            }
        }

        /// <summary>
        /// Calculates spline progress from a linear progress.
        /// </summary>
        /// <param name="linearProgress">the linear progress</param>
        /// <returns>the spline progress</returns>
        public float GetSplineProgress(float linearProgress)
        {
            ReadPreamble();

            if (_isDirty)
            {
                Build();
            }

            if (!_isSpecified)
            {
                return linearProgress;
            }
            else
            {
                SetParameterFromX(linearProgress);

                return GetBezierValue(_by, _cy, _parameter);
            }
        }

        #endregion

        #region Private

        private bool IsValidControlPoint(Point point)
        {
            return point.X >= 0.0
                && point.X <= 1.0;
        }

        /// <summary>
        /// Compute cached coefficients.
        /// </summary>
        private void Build()
        {
            Debug.Assert(_isDirty);

            if (_controlPoint1 == new Point(0, 0)
                && _controlPoint2 == new Point(1, 1))
            {
                // This KeySpline would have no effect on the progress.

                _isSpecified = false;
            }
            else
            {
                _isSpecified = true;

                _parameter = 0;

                // X coefficients
                _bx = 3 * _controlPoint1.X;
                _cx = 3 * _controlPoint2.X;
                _cx_Bx = 2 * (_cx - _bx);
                _three_Cx = 3 - _cx;

                // Y coefficients
                _by = 3 * _controlPoint1.Y;
                _cy = 3 * _controlPoint2.Y;
            }

            _isDirty = false;
        }

        /// <summary>
        /// Get an X or Y value with the Bezier formula.
        /// </summary>
        /// <param name="b">the second Bezier coefficient</param>
        /// <param name="c">the third Bezier coefficient</param>
        /// <param name="t">the parameter value to evaluate at</param>
        /// <returns>the value of the Bezier function at the given parameter</returns>
        static private float GetBezierValue(float b, float c, float t)
        {
            float s = 1.0f - t;
            float t2 = t * t;

            return b * t * s * s + c * t2 * s + t2 * t;
        }

        /// <summary>
        /// Get X and dX/dt at a given parameter
        /// </summary>
        /// <param name="t">the parameter value to evaluate at</param>
        /// <param name="x">the value of x there</param>
        /// <param name="dx">the value of dx/dt there</param>
        private void GetXAndDx(float t, out float x, out float dx)
        {
            Debug.Assert(_isSpecified);

            float s = 1.0f - t;
            float t2 = t * t;
            float s2 = s * s;

            x = _bx * t * s2 + _cx * t2 * s + t2 * t;
            dx = _bx * s2 + _cx_Bx * s * t + _three_Cx * t2;
        }

        /// <summary>
        /// Compute the parameter value that corresponds to a given X value, using a modified
        /// clamped Newton-Raphson algorithm to solve the equation X(t) - time = 0. We make 
        /// use of some known properties of this particular function:
        /// * We are only interested in solutions in the interval [0,1]
        /// * X(t) is increasing, so we can assume that if X(t) > time t > solution.  We use
        ///   that to clamp down the search interval with every probe.
        /// * The derivative of X and Y are between 0 and 3.
        /// </summary>
        /// <param name="time">the time, scaled to fit in [0,1]</param>
        private void SetParameterFromX(float time)
        {
            Debug.Assert(_isSpecified);

            // Dynamic search interval to clamp with
            float bottom = 0;
            float top = 1;

            if (time == 0)
            {
                _parameter = 0;
            }
            else if (time == 1)
            {
                _parameter = 1;
            }
            else
            {
                // Loop while improving the guess
                while (top - bottom > _Fuzz)
                {
                    float x, dx, absdx;

                    // Get x and dx/dt at the current parameter
                    GetXAndDx(_parameter, out x, out dx);
                    absdx = Math.Abs(dx);

                    // Clamp down the search interval, relying on the monotonicity of X(t)
                    if (x > time)
                    {
                        top = _parameter;      // because parameter > solution
                    }
                    else
                    {
                        bottom = _parameter;  // because parameter < solution
                    }

                    // The desired accuracy is in ultimately in y, not in x, so the
                    // accuracy needs to be multiplied by dx/dy = (dx/dt) / (dy/dt).
                    // But dy/dt <=3, so we omit that
                    if (Math.Abs(x - time) < _Accuracy * absdx)
                    {
                        break; // We're there
                    }

                    if (absdx > _Fuzz)
                    {
                        // Nonzero derivative, use Newton-Raphson to obtain the next guess
                        float next = _parameter - (x - time) / dx;

                        // If next guess is out of the search interval then clamp it in
                        if (next >= top)
                        {
                            _parameter = (_parameter + top) / 2;
                        }
                        else if (next <= bottom)
                        {
                            _parameter = (_parameter + bottom) / 2;
                        }
                        else
                        {
                            // Next guess is inside the search interval, accept it
                            _parameter = next;
                        }
                    }
                    else    // Zero derivative, halve the search interval
                    {
                        _parameter = (bottom + top) / 2;
                    }
                }
            }
        }


        /// <summary>
        /// Copy the common fields for the various Clone methods
        /// </summary>
        /// <param name="sourceKeySpline">The KeySpline to copy.</param>
        private void CloneCommon(KeySpline sourceKeySpline)
        {
            _controlPoint1 = sourceKeySpline._controlPoint1;
            _controlPoint2 = sourceKeySpline._controlPoint2;
            _isDirty = true;
        }

        #endregion

        #region IFormattable

        /// <summary>
        /// Creates a string representation of this KeySpline based on the current culture.
        /// </summary>
        /// <returns>
        /// A string representation of this KeySpline.
        /// </returns>
        public override string ToString()
        {
            ReadPreamble();

            return InternalConvertToString(null, null);
        }

        /// <summary>
        /// Creates a string representation of this KeySpline based on the IFormatProvider
        /// passed in.  
        /// </summary>
        /// <param name="formatProvider">
        /// The format provider to use.  If the provider is null, the CurrentCulture is used.
        /// </param>
        /// <returns>
        /// A string representation of this KeySpline.
        /// </returns>
        public string ToString(IFormatProvider formatProvider)
        {
            ReadPreamble();

            return InternalConvertToString(null, formatProvider);
        }

        /// <summary>
        /// Creates a string representation of this KeySpline based on the IFormatProvider
        /// passed in.  
        /// </summary>
        /// <param name="format">
        /// The format string to use.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider to use.  If the provider is null, the CurrentCulture is used.
        /// </param>
        /// <returns>
        /// A string representation of this KeySpline.
        /// </returns>
        string IFormattable.ToString(string? format, IFormatProvider? formatProvider)
        {
            ReadPreamble();

            return InternalConvertToString(format, formatProvider);
        }

        /// <summary>
        /// Creates a string representation of this KeySpline based on the IFormatProvider
        /// passed in.  
        /// </summary>
        /// <param name="format">
        /// The format string to use.
        /// </param>
        /// <param name="formatProvider">
        /// The format provider to use.  If the provider is null, the CurrentCulture is used.
        /// </param>
        /// <returns>
        /// A string representation of this KeySpline.
        /// </returns>
        internal string InternalConvertToString(string? format, IFormatProvider? formatProvider)
        {
            // Helper to get the numeric list separator for a given culture.
            char separator = TokenizerHelper.GetNumericListSeparator(formatProvider);

            return String.Format(
                formatProvider,
                "{1}{0}{2}",
                separator,
                _controlPoint1,
                _controlPoint2);
        }

        #endregion

        #region Data

        // This structure is way to large for, well, a structure.  I think the
        // animation class can allocate some data structures for calculation
        // purposes only when needed that this class can just hold the two
        // points and the bool.

        // Control points
        private Point _controlPoint1;
        private Point _controlPoint2;
        private bool _isSpecified;
        private bool _isDirty;

        // The parameter that corresponds to the most recent time
        private float _parameter;

        // Cached coefficients
        private float _bx;        // 3*points[0].X
        private float _cx;        // 3*points[1].X
        private float _cx_Bx;     // 2*(Cx - Bx)
        private float _three_Cx;  // 3 - Cx

        private float _by;        // 3*points[0].Y
        private float _cy;        // 3*points[1].Y

        // constants
        private const float _Accuracy = .001f;   // 1/3 the desired accuracy in X
        private const float _Fuzz = .000001f;    // computational zero

        #endregion
    }
}
