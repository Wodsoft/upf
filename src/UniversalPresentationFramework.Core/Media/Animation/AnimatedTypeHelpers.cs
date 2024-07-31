using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    internal static class AnimatedTypeHelpers
    {
        #region Interpolation Methods

        internal static byte InterpolateByte(in byte from, in byte to, in float progress)
        {
            return (byte)(from + (int)((to - from + 0.5f) * progress));
        }

        internal static Color InterpolateColor(in Color from, in Color to, in float progress)
        {
            return from + ((to - from) * progress);
        }

        internal static decimal InterpolateDecimal(in decimal from, in decimal to, in float progress)
        {
            return from + ((to - from) * (decimal)progress);
        }

        internal static double InterpolateDouble(in double from, in double to, in float progress)
        {
            return from + ((to - from) * progress);
        }

        internal static short InterpolateInt16(in short from, in short to, in float progress)
        {
            if (progress == 0.0)
            {
                return from;
            }
            else if (progress == 1.0)
            {
                return to;
            }
            else
            {
                double addend = (float)(to - from);
                addend *= progress;
                addend += (addend > 0.0) ? 0.5 : -0.5;

                return (short)(from + (short)addend);
            }
        }

        internal static int InterpolateInt32(in int from, in int to, in float progress)
        {
            if (progress == 0.0)
            {
                return from;
            }
            else if (progress == 1.0)
            {
                return to;
            }
            else
            {
                double addend = (float)(to - from);
                addend *= progress;
                addend += (addend > 0.0) ? 0.5 : -0.5;

                return from + (int)addend;
            }
        }

        internal static long InterpolateInt64(in long from, in long to, in float progress)
        {
            if (progress == 0.0)
            {
                return from;
            }
            else if (progress == 1.0)
            {
                return to;
            }
            else
            {
                double addend = (float)(to - from);
                addend *= progress;
                addend += (addend > 0.0) ? 0.5 : -0.5;

                return from + (long)addend;
            }
        }

        internal static Point InterpolatePoint(in Point from, in Point to, in float progress)
        {
            return from + ((to - from) * (float)progress);
        }

        //internal static Point3D InterpolatePoint3D(in Point3D from, in Point3D to, in float progress)
        //{
        //    return from + ((to - from) * progress);
        //}

        internal static Quaternion InterpolateQuaternion(in Quaternion from, in Quaternion to, float progress, in bool useShortestPath)
        {
            return Quaternion.Slerp(from, to, (float)progress);
        }

        internal static Rect InterpolateRect(in Rect from, in Rect to, in float progress)
        {
            Rect temp = new Rect();

            // from + ((from - to) * progress)
            temp.Location = new Point(
                from.Location.X + ((to.Location.X - from.Location.X) * (float)progress),
                from.Location.Y + ((to.Location.Y - from.Location.Y) * (float)progress));
            temp.Size = new Size(
                from.Size.Width + ((to.Size.Width - from.Size.Width) * (float)progress),
                from.Size.Height + ((to.Size.Height - from.Size.Height) * (float)progress));

            return temp;
        }

        //internal static Rotation3D InterpolateRotation3D(in Rotation3D from, in Rotation3D to, in float progress)
        //{
        //    return new QuaternionRotation3D(InterpolateQuaternion(from.InternalQuaternion, to.InternalQuaternion, progress, /* useShortestPath = */ true));
        //}

        internal static float InterpolateSingle(in float from, in float to, in float progress)
        {
            return from + (float)((to - from) * progress);
        }

        internal static Size InterpolateSize(in Size from, in Size to, in float progress)
        {
            return (Size)InterpolateVector((Vector2)from, (Vector2)to, progress);
        }

        internal static Vector2 InterpolateVector(in Vector2 from, in Vector2 to, in float progress)
        {
            return from + ((to - from) * (float)progress);
        }

        internal static Vector3 InterpolateVector3D(in Vector3 from, in Vector3 to, in float progress)
        {
            return from + ((to - from) * progress);
        }

        #endregion

        #region Add Methods

        internal static byte AddByte(in byte value1, in byte value2)
        {
            return (byte)(value1 + value2);
        }

        internal static Color AddColor(in Color value1, in Color value2)
        {
            return value1 + value2;
        }

        internal static decimal AddDecimal(in decimal value1, in decimal value2)
        {
            return value1 + value2;
        }

        internal static double AddDouble(in double value1, in double value2)
        {
            return value1 + value2;
        }

        internal static short AddInt16(in short value1, in short value2)
        {
            return (short)(value1 + value2);
        }

        internal static int AddInt32(in int value1, in int value2)
        {
            return value1 + value2;
        }

        internal static long AddInt64(in long value1, in long value2)
        {
            return value1 + value2;
        }

        internal static Point AddPoint(in Point value1, in Point value2)
        {
            return new Point(
                value1.X + value2.X,
                value1.Y + value2.Y);
        }

        //internal static Point3D AddPoint3D(in Point3D value1, in Point3D value2)
        //{
        //    return new Point3D(
        //        value1.X + value2.X,
        //        value1.Y + value2.Y,
        //        value1.Z + value2.Z);
        //}

        internal static Quaternion AddQuaternion(in Quaternion value1, in Quaternion value2)
        {
            return value1 * value2;
        }

        internal static float AddSingle(in float value1, in float value2)
        {
            return value1 + value2;
        }

        internal static Size AddSize(in Size value1, in Size value2)
        {
            return new Size(
                value1.Width + value2.Width,
                value1.Height + value2.Height);
        }

        internal static Vector2 AddVector(in Vector2 value1, in Vector2 value2)
        {
            return value1 + value2;
        }

        internal static Vector3 AddVector3D(in Vector3 value1, in Vector3 value2)
        {
            return value1 + value2;
        }

        internal static Rect AddRect(in Rect value1, in Rect value2)
        {
            return new Rect(
                AddPoint(value1.Location, value2.Location),
                AddSize(value1.Size, value2.Size));
        }

        //internal static Rotation3D AddRotation3D(in Rotation3D value1, in Rotation3D value2)
        //{
        //    if (value1 == null)
        //    {
        //        value1 = Rotation3D.Identity;
        //    }
        //    if (value2 == null)
        //    {
        //        value2 = Rotation3D.Identity;
        //    }

        //    return new QuaternionRotation3D(AddQuaternion(value1.InternalQuaternion, value2.InternalQuaternion));
        //}

        #endregion

        #region Subtract Methods

        internal static byte SubtractByte(in byte value1, in byte value2)
        {
            return (byte)(value1 - value2);
        }

        internal static Color SubtractColor(in Color value1, in Color value2)
        {
            return value1 - value2;
        }

        internal static decimal SubtractDecimal(in decimal value1, in decimal value2)
        {
            return value1 - value2;
        }

        internal static double SubtractDouble(in double value1, in double value2)
        {
            return value1 - value2;
        }

        internal static short SubtractInt16(in short value1, in short value2)
        {
            return (short)(value1 - value2);
        }

        internal static int SubtractInt32(in int value1, in int value2)
        {
            return value1 - value2;
        }

        internal static long SubtractInt64(in long value1, in long value2)
        {
            return value1 - value2;
        }

        internal static Point SubtractPoint(in Point value1, in Point value2)
        {
            return new Point(
                value1.X - value2.X,
                value1.Y - value2.Y);
        }

        //internal static Point3D SubtractPoint3D(in Point3D value1, in Point3D value2)
        //{
        //    return new Point3D(
        //        value1.X - value2.X,
        //        value1.Y - value2.Y,
        //        value1.Z - value2.Z);
        //}

        internal static Quaternion SubtractQuaternion(in Quaternion value1, in Quaternion value2)
        {
            return value1 - value2;
            //value2.Invert();
            //return value1 * value2;
        }

        internal static float SubtractSingle(in float value1, in float value2)
        {
            return value1 - value2;
        }

        internal static Size SubtractSize(in Size value1, in Size value2)
        {
            return new Size(
                value1.Width - value2.Width,
                value1.Height - value2.Height);
        }

        internal static Vector2 SubtractVector(in Vector2 value1, in Vector2 value2)
        {
            return value1 - value2;
        }

        internal static Vector3 SubtractVector3D(in Vector3 value1, in Vector3 value2)
        {
            return value1 - value2;
        }

        internal static Rect SubtractRect(in Rect value1, in Rect value2)
        {
            return new Rect(
                SubtractPoint(value1.Location, value2.Location),
                SubtractSize(value1.Size, value2.Size));
        }

        //internal static Rotation3D SubtractRotation3D(in Rotation3D value1, in Rotation3D value2)
        //{
        //    return new QuaternionRotation3D(SubtractQuaternion(value1.InternalQuaternion, value2.InternalQuaternion));
        //}

        #endregion

        #region GetSegmentLength Methods

        internal static float GetSegmentLengthBoolean(in bool from, in bool to)
        {
            if (from != to)
            {
                return 1.0f;
            }
            else
            {
                return 0.0f;
            }
        }

        internal static float GetSegmentLengthByte(in byte from, in byte to)
        {
            return Math.Abs(to - from);
        }

        internal static float GetSegmentLengthChar(in char from, in char to)
        {
            if (from != to)
            {
                return 1.0f;
            }
            else
            {
                return 0.0f;
            }
        }

        internal static float GetSegmentLengthColor(in Color from, in Color to)
        {
            return Math.Abs(to.ScA - from.ScA)
                 + Math.Abs(to.ScR - from.ScR)
                 + Math.Abs(to.ScG - from.ScG)
                 + Math.Abs(to.ScB - from.ScB);
        }

        internal static float GetSegmentLengthDecimal(in decimal from, in decimal to)
        {
            // We may lose precision here, but it's not likely going to be a big deal
            // for the purposes of this method.  The relative lengths of decimal
            // segments will still be adequately represented.
            return (float)Math.Abs(to - from);
        }

        internal static float GetSegmentLengthDouble(in double from, in double to)
        {
            return (float)Math.Abs(to - from);
        }

        internal static float GetSegmentLengthInt16(in short from, in short to)
        {
            return Math.Abs(to - from);
        }

        internal static float GetSegmentLengthInt32(in int from, in int to)
        {
            return Math.Abs(to - from);
        }

        internal static float GetSegmentLengthInt64(in long from, in long to)
        {
            return Math.Abs(to - from);
        }

        internal static float GetSegmentLengthMatrix(in Matrix3x2 from, in Matrix3x2 to)
        {
            if (from != to)
            {
                return 1.0f;
            }
            else
            {
                return 0.0f;
            }
        }

        internal static float GetSegmentLengthObject(object? from, object? to)
        {
            return 1.0f;
        }

        internal static float GetSegmentLengthPoint(in Point from, in Point to)
        {
            return Math.Abs((to - from).Length());
        }

        //internal static float GetSegmentLengthPoint3D(in Point3D from, in Point3D to)
        //{
        //    return Math.Abs((to - from).Length);
        //}

        internal static float GetSegmentLengthQuaternion(in Quaternion from, in Quaternion to)
        {
            var value = to * -from;

            float msin = MathF.Sqrt(value.X * value.X + value.Y * value.Y + value.Z * value.Z);
            float mcos = value.W;

            if (!(msin <= float.MaxValue))
            {
                // Overflowed probably in squaring, so let's scale
                // the values.  We don't need to include _w in the
                // scale factor because we're not going to square
                // it.
                float maxcoeff = Math.Max(Math.Abs(value.X), Math.Max(Math.Abs(value.Y), Math.Abs(value.Z)));
                float x = value.X / maxcoeff;
                float y = value.Y / maxcoeff;
                float z = value.Z / maxcoeff;
                msin = MathF.Sqrt(x * x + y * y + z * z);
                // Scale mcos too.
                mcos = value.W / maxcoeff;
            }

            // Atan2 is better than acos.  (More precise and more efficient.)
            return MathF.Atan2(msin, mcos) * (360.0f / MathF.PI);

            //from.Invert();
            //return (to * from).Angle;
        }

        internal static float GetSegmentLengthRect(in Rect from, in Rect to)
        {
            // This seems to me to be the most logical way to define the
            // distance between two rects.  Lots of sqrt, but since paced
            // rectangle animations are such a rare thing, we may as well do
            // them right since the user obviously knows what they want.
            float a = GetSegmentLengthPoint(from.Location, to.Location);
            float b = GetSegmentLengthSize(from.Size, to.Size);

            // Return c.
            return MathF.Sqrt((a * a) + (b * b));
        }

        //internal static float GetSegmentLengthRotation3D(in Rotation3D from, in Rotation3D to)
        //{
        //    return GetSegmentLengthQuaternion(from.InternalQuaternion, to.InternalQuaternion);
        //}

        internal static float GetSegmentLengthSingle(in float from, in float to)
        {
            return Math.Abs(to - from);
        }

        internal static float GetSegmentLengthSize(in Size from, in Size to)
        {
            return Math.Abs(((Vector2)to - (Vector2)from).Length());
        }

        internal static float GetSegmentLengthString(in string from, in string to)
        {
            if (from != to)
            {
                return 1.0f;
            }
            else
            {
                return 0.0f;
            }
        }

        internal static float GetSegmentLengthVector(in Vector2 from, in Vector2 to)
        {
            return Math.Abs((to - from).Length());
        }

        internal static float GetSegmentLengthVector3D(in Vector3 from, in Vector3 to)
        {
            return Math.Abs((to - from).Length());
        }

        #endregion

        #region Scale Methods

        internal static byte ScaleByte(in byte value, in float factor)
        {
            return (byte)(value * factor);
        }

        internal static Color ScaleColor(in Color value, in float factor)
        {
            return value * factor;
        }

        internal static decimal ScaleDecimal(in decimal value, in float factor)
        {
            return value * (decimal)factor;
        }

        internal static double ScaleDouble(in double value, in float factor)
        {
            return value * factor;
        }

        internal static short ScaleInt16(in short value, in float factor)
        {
            return (short)(value * factor);
        }

        internal static int ScaleInt32(in int value, in float factor)
        {
            return (int)(value * factor);
        }

        internal static long ScaleInt64(in long value, in float factor)
        {
            return (long)(value * factor);
        }

        internal static Point ScalePoint(in Point value, in float factor)
        {
            return new Point(
                value.X * factor,
                value.Y * factor);
        }

        //internal static Point3D ScalePoint3D(in Point3D value, in float factor)
        //{
        //    return new Point3D(
        //        value.X * factor,
        //        value.Y * factor,
        //        value.Z * factor);
        //}

        internal static Quaternion ScaleQuaternion(in Quaternion value, in float factor)
        {
            return Quaternion.Multiply(value, factor);
            //return new Quaternion(value.Axis, value.Angle * factor);
        }

        internal static Rect ScaleRect(in Rect value, in float factor)
        {
            Rect temp = new Rect();

            temp.Location = new Point(
                value.Location.X * factor,
                value.Location.Y * factor);
            temp.Size = new Size(
                value.Size.Width * factor,
                value.Size.Height * factor);

            return temp;
        }

        //internal static Rotation3D ScaleRotation3D(in Rotation3D value, in float factor)
        //{
        //    return new QuaternionRotation3D(ScaleQuaternion(value.InternalQuaternion, factor));
        //}

        internal static float ScaleSingle(in float value, in float factor)
        {
            return (float)((float)value * factor);
        }

        internal static Size ScaleSize(in Size value, in float factor)
        {
            return (Size)((Vector2)value * factor);
        }

        internal static Vector2 ScaleVector(in Vector2 value, in float factor)
        {
            return value * factor;
        }

        internal static Vector3 ScaleVector3D(in Vector3 value, in float factor)
        {
            return value * factor;
        }

        #endregion

        #region EnsureValidAnimationValue Methods

        internal static bool IsValidAnimationValueBoolean(in bool value)
        {
            return true;
        }

        internal static bool IsValidAnimationValueByte(in byte value)
        {
            return true;
        }

        internal static bool IsValidAnimationValueChar(in char value)
        {
            return true;
        }

        internal static bool IsValidAnimationValueColor(in Color value)
        {
            return true;
        }

        internal static bool IsValidAnimationValueDecimal(in decimal value)
        {
            return true;
        }

        internal static bool IsValidAnimationValueDouble(in double value)
        {
            if (IsInvalidDouble(value))
            {
                return false;
            }

            return true;
        }

        internal static bool IsValidAnimationValueInt16(in short value)
        {
            return true;
        }

        internal static bool IsValidAnimationValueInt32(in int value)
        {
            return true;
        }

        internal static bool IsValidAnimationValueInt64(in long value)
        {
            return true;
        }

        internal static bool IsValidAnimationValueMatrix(in Matrix3x2 value)
        {
            return true;
        }

        internal static bool IsValidAnimationValuePoint(in Point value)
        {
            if (IsInvalidDouble(value.X) || IsInvalidDouble(value.Y))
            {
                return false;
            }

            return true;
        }

        //internal static bool IsValidAnimationValuePoint3D(in Point3D value)
        //{
        //    if (IsInvalidDouble(value.X) || IsInvalidDouble(value.Y) || IsInvalidDouble(value.Z))
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        internal static bool IsValidAnimationValueQuaternion(in Quaternion value)
        {
            if (IsInvalidDouble(value.X) || IsInvalidDouble(value.Y)
                || IsInvalidDouble(value.Z) || IsInvalidDouble(value.W))
            {
                return false;
            }

            return true;
        }

        internal static bool IsValidAnimationValueRect(in Rect value)
        {
            if (IsInvalidDouble(value.Location.X) || IsInvalidDouble(value.Location.Y)
                || IsInvalidDouble(value.Size.Width) || IsInvalidDouble(value.Size.Height)
                || value.IsEmpty)
            {
                return false;
            }

            return true;
        }

        //internal static bool IsValidAnimationValueRotation3D(in Rotation3D value)
        //{
        //    return IsValidAnimationValueQuaternion(value.InternalQuaternion);
        //}

        internal static bool IsValidAnimationValueSingle(in float value)
        {
            if (float.IsInfinity(value) || float.IsNaN(value))
            {
                return false;
            }

            return true;
        }

        internal static bool IsValidAnimationValueSize(in Size value)
        {
            if (IsInvalidDouble(value.Width) || IsInvalidDouble(value.Height))
            {
                return false;
            }

            return true;
        }

        internal static bool IsValidAnimationValueString(in string value)
        {
            return true;
        }

        internal static bool IsValidAnimationValueVector(in Vector2 value)
        {
            if (IsInvalidDouble(value.X) || IsInvalidDouble(value.Y))
            {
                return false;
            }

            return true;
        }

        internal static bool IsValidAnimationValueVector3D(in Vector3 value)
        {
            if (IsInvalidDouble(value.X) || IsInvalidDouble(value.Y) || IsInvalidDouble(value.Z))
            {
                return false;
            }

            return true;
        }

        #endregion

        #region GetZeroValueMethods

        internal static byte GetZeroValueByte(in byte baseValue)
        {
            return 0;
        }

        internal static Color GetZeroValueColor(in Color baseValue)
        {
            return Color.FromScRgb(0.0F, 0.0F, 0.0F, 0.0F);
        }

        internal static decimal GetZeroValueDecimal(in decimal baseValue)
        {
            return decimal.Zero;
        }

        internal static double GetZeroValueDouble(in double baseValue)
        {
            return 0.0;
        }

        internal static short GetZeroValueInt16(in short baseValue)
        {
            return 0;
        }

        internal static int GetZeroValueInt32(in int baseValue)
        {
            return 0;
        }

        internal static long GetZeroValueInt64(in long baseValue)
        {
            return 0;
        }

        internal static Point GetZeroValuePoint(in Point baseValue)
        {
            return new Point();
        }

        //internal static Point3D GetZeroValuePoint3D(in Point3D baseValue)
        //{
        //    return new Point3D();
        //}

        internal static Quaternion GetZeroValueQuaternion(in Quaternion baseValue)
        {
            return Quaternion.Identity;
        }

        internal static float GetZeroValueSingle(in float baseValue)
        {
            return 0.0F;
        }

        internal static Size GetZeroValueSize(in Size baseValue)
        {
            return new Size();
        }

        internal static Vector2 GetZeroValueVector(in Vector2 baseValue)
        {
            return Vector2.Zero;
        }

        internal static Vector3 GetZeroValueVector3D(in Vector3 baseValue)
        {
            return Vector3.Zero;
        }

        internal static Rect GetZeroValueRect(in Rect baseValue)
        {
            return new Rect(new Point(), new Vector2());
        }

        //internal static Rotation3D GetZeroValueRotation3D(in Rotation3D baseValue)
        //{
        //    return Rotation3D.Identity;
        //}

        #endregion

        #region Helpers

        private static bool IsInvalidDouble(in double value)
        {
            return double.IsInfinity(value)
                || double.IsNaN(value);
        }

        #endregion
    }
}
