using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    internal class MatrixUtil
    {
        /// <summary>
        /// TransformRect - Internal helper for perf
        /// </summary>
        /// <param name="rect"> The Rect to transform. </param>
        /// <param name="matrix"> The Matrix with which to transform the Rect. </param>
        internal static void TransformRect(ref Rect rect, ref Matrix3x2 matrix)
        {
            if (rect.IsEmpty)
            {
                return;
            }

            // If the matrix is identity, don't worry.
            if (matrix.IsIdentity)
            {
                return;
            }

            // Scaling
            if (matrix.M11 != 0 && matrix.M22 != 0)
            {
                rect.X *= matrix.M11;
                rect.Y *= matrix.M22;
                rect.Width *= matrix.M11;
                rect.Height *= matrix.M22;

                // Ensure the width is always positive.  For example, if there was a reflection about the
                // y axis followed by a translation into the visual area, the width could be negative.
                if (rect.Width < 0.0)
                {
                    rect.X += rect.Width;
                    rect.Width = -rect.Width;
                }

                // Ensure the height is always positive.  For example, if there was a reflection about the
                // x axis followed by a translation into the visual area, the height could be negative.
                if (rect.Height < 0.0)
                {
                    rect.Y += rect.Height;
                    rect.Height = -rect.Height;
                }
            }

            // Translation
            if (matrix.M31 != 0 && matrix.M32 != 0)
            {
                // X
                rect.X += matrix.M31;

                // Y
                rect.Y += matrix.M32;
            }

            //if (matrixType == MatrixTypes.TRANSFORM_IS_UNKNOWN)
            //{
            //    // Al Bunny implementation.
            //    Point point0 = matrix.Transform(rect.TopLeft);
            //    Point point1 = matrix.Transform(rect.TopRight);
            //    Point point2 = matrix.Transform(rect.BottomRight);
            //    Point point3 = matrix.Transform(rect.BottomLeft);

            //    // Width and height is always positive here.
            //    rect._x = Math.Min(Math.Min(point0.X, point1.X), Math.Min(point2.X, point3.X));
            //    rect._y = Math.Min(Math.Min(point0.Y, point1.Y), Math.Min(point2.Y, point3.Y));

            //    rect._width = Math.Max(Math.Max(point0.X, point1.X), Math.Max(point2.X, point3.X)) - rect._x;
            //    rect._height = Math.Max(Math.Max(point0.Y, point1.Y), Math.Max(point2.Y, point3.Y)) - rect._y;
            //}
        }
    }
}
