using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public sealed class MatrixTransform : Transform
    {
        #region Constructors

        ///<summary>
        ///
        ///</summary>
        public MatrixTransform()
        {
        }

        ///<summary>
        /// Create an arbitrary matrix transformation.
        ///</summary>
        ///<param name="m11">Matrix value at position 1,1</param>
        ///<param name="m12">Matrix value at position 1,2</param>
        ///<param name="m21">Matrix value at position 2,1</param>
        ///<param name="m22">Matrix value at position 2,2</param>
        ///<param name="offsetX">Matrix value at position 3,1</param>
        ///<param name="offsetY">Matrix value at position 3,2</param>
        public MatrixTransform(
            float m11,
            float m12,
            float m21,
            float m22,
            float offsetX,
            float offsetY
            )
        {
            Matrix = new Matrix3x2(m11, m12, m21, m22, offsetX, offsetY);
        }

        ///<summary>
        /// Create a matrix transformation from constant transform.
        ///</summary>
        ///<param name="matrix">The constant matrix transformation.</param>
        public MatrixTransform(Matrix3x2 matrix)
        {
            Matrix = matrix;
        }

        #endregion

        #region Properties

        public static readonly DependencyProperty MatrixProperty = DependencyProperty.Register("Matrix",
                                   typeof(Matrix3x2),
                                   typeof(MatrixTransform),
                                   new PropertyMetadata(Matrix3x2.Identity));
        public Matrix3x2 Matrix
        {
            get { return (Matrix3x2)GetValue(MatrixProperty)!; }
            set { SetValue(MatrixProperty, value); }
        }

        public override Matrix3x2 Value => Matrix;

        #endregion

        #region Freezable

        protected override Freezable CreateInstanceCore() => new MatrixTransform();

        public new MatrixTransform Clone()
        {
            return (MatrixTransform)base.Clone();
        }

        public new MatrixTransform CloneCurrentValue()
        {
            return (MatrixTransform)base.CloneCurrentValue();
        }

        #endregion

        #region Format

        protected override string? ConvertToString(string? format, IFormatProvider? provider)
        {
            return Matrix.ToString();
        }

        #endregion
    }
}
