using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public abstract class Transform : GeneralTransform
    {
        private static Transform _Identity;
        static Transform()
        {
            _Identity = new MatrixTransform(Matrix3x2.Identity);
            _Identity.Freeze();
        }
        public static Transform Identity => _Identity;

        public abstract Matrix3x2 Value { get; }

        public override bool TryTransform(Point inPoint, out Point result)
        {
            TransformPoint(ref inPoint);
            result = inPoint;
            return true;
        }

        private void TransformPoint(ref Point point)
        {
            ref var vector = ref Unsafe.As<Point, Vector2>(ref point);
            vector = Vector2.Transform(vector, Matrix3x2.Identity);
        }

        /// <summary>
        /// Transforms the bounding box to the smallest axis aligned bounding box
        /// that contains all the points in the original bounding box
        /// </summary>
        /// <param name="rect">Bounding box</param>
        /// <returns>The transformed bounding box</returns>
        public override Rect TransformBounds(Rect rect)
        {
            var matrix = Value;
            MatrixUtil.TransformRect(ref rect, ref matrix);
            return rect;
        }

        public override GeneralTransform? Inverse
        {
            get
            {
                var matrix = Value;
                Matrix3x2.Invert(matrix, out var result);
                return new MatrixTransform(matrix);
            }
        }

        internal void OnInheritanceContextChangedInternal()
        {
            OnInheritanceContextChanged();
        }

        #region Format

        protected override string? ConvertToString(string? format, IFormatProvider? provider)
        {
            return Value.ToString();
        }

        #endregion
    }
}
