using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    /// <summary>
    /// This class is used as part of a BooleanKeyFrameCollection in
    /// conjunction with a KeyFrameBooleanAnimation to animate a
    /// bool property value along a set of key frames.
    /// </summary>
    public abstract class BooleanKeyFrame : GenericAnimationKeyFrame<bool>
    {
        protected BooleanKeyFrame()
        {
        }

        protected BooleanKeyFrame(bool value) : base(value)
        {
        }

        protected BooleanKeyFrame(bool value, KeyTime keyTime) : base(value, keyTime)
        {
        }
    }


    /// <summary>
    /// This class is used as part of a ByteKeyFrameCollection in
    /// conjunction with a KeyFrameByteAnimation to animate a
    /// byte property value along a set of key frames.
    /// </summary>
    public abstract class ByteKeyFrame : GenericAnimationKeyFrame<byte>
    {
        protected ByteKeyFrame()
        {
        }

        protected ByteKeyFrame(byte value) : base(value)
        {
        }

        protected ByteKeyFrame(byte value, KeyTime keyTime) : base(value, keyTime)
        {
        }
    }


    /// <summary>
    /// This class is used as part of a CharKeyFrameCollection in
    /// conjunction with a KeyFrameCharAnimation to animate a
    /// Char property value along a set of key frames.
    /// </summary>
    public abstract class CharKeyFrame : GenericAnimationKeyFrame<char>
    {
        protected CharKeyFrame()
        {
        }

        protected CharKeyFrame(char value) : base(value)
        {
        }

        protected CharKeyFrame(char value, KeyTime keyTime) : base(value, keyTime)
        {
        }
    }


    /// <summary>
    /// This class is used as part of a ColorKeyFrameCollection in
    /// conjunction with a KeyFrameColorAnimation to animate a
    /// Color property value along a set of key frames.
    /// </summary>
    public abstract class ColorKeyFrame : GenericAnimationKeyFrame<Color>
    {
        protected ColorKeyFrame()
        {
        }

        protected ColorKeyFrame(Color value) : base(value)
        {
        }

        protected ColorKeyFrame(Color value, KeyTime keyTime) : base(value, keyTime)
        {
        }
    }


    /// <summary>
    /// This class is used as part of a DecimalKeyFrameCollection in
    /// conjunction with a KeyFrameDecimalAnimation to animate a
    /// Decimal property value along a set of key frames.
    /// </summary>
    public abstract class DecimalKeyFrame : GenericAnimationKeyFrame<decimal>
    {
        protected DecimalKeyFrame()
        {
        }

        protected DecimalKeyFrame(decimal value) : base(value)
        {
        }

        protected DecimalKeyFrame(decimal value, KeyTime keyTime) : base(value, keyTime)
        {
        }
    }


    /// <summary>
    /// This class is used as part of a DoubleKeyFrameCollection in
    /// conjunction with a KeyFrameDoubleAnimation to animate a
    /// float property value along a set of key frames.
    /// </summary>
    public abstract class DoubleKeyFrame : GenericAnimationKeyFrame<double>
    {
        protected DoubleKeyFrame()
        {
        }

        protected DoubleKeyFrame(double value) : base(value)
        {
        }

        protected DoubleKeyFrame(double value, KeyTime keyTime) : base(value, keyTime)
        {
        }
    }


    /// <summary>
    /// This class is used as part of a Int16KeyFrameCollection in
    /// conjunction with a KeyFrameInt16Animation to animate a
    /// Int16 property value along a set of key frames.
    /// </summary>
    public abstract class Int16KeyFrame : GenericAnimationKeyFrame<short>
    {
        protected Int16KeyFrame()
        {
        }

        protected Int16KeyFrame(short value) : base(value)
        {
        }

        protected Int16KeyFrame(short value, KeyTime keyTime) : base(value, keyTime)
        {
        }
    }


    /// <summary>
    /// This class is used as part of a Int32KeyFrameCollection in
    /// conjunction with a KeyFrameInt32Animation to animate a
    /// Int32 property value along a set of key frames.
    /// </summary>
    public abstract class Int32KeyFrame : GenericAnimationKeyFrame<int>
    {
        protected Int32KeyFrame()
        {
        }

        protected Int32KeyFrame(int value) : base(value)
        {
        }

        protected Int32KeyFrame(int value, KeyTime keyTime) : base(value, keyTime)
        {
        }
    }


    /// <summary>
    /// This class is used as part of a Int64KeyFrameCollection in
    /// conjunction with a KeyFrameInt64Animation to animate a
    /// Int64 property value along a set of key frames.
    /// </summary>
    public abstract class Int64KeyFrame : GenericAnimationKeyFrame<long>
    {
        protected Int64KeyFrame()
        {
        }

        protected Int64KeyFrame(long value) : base(value)
        {
        }

        protected Int64KeyFrame(long value, KeyTime keyTime) : base(value, keyTime)
        {
        }
    }

    /// <summary>
    /// This class is used as part of a MatrixKeyFrameCollection in
    /// conjunction with a KeyFrameMatrixAnimation to animate a
    /// Matrix property value along a set of key frames.
    /// </summary>
    public abstract class MatrixKeyFrame : GenericAnimationKeyFrame<Matrix3x2>
    {
        protected MatrixKeyFrame()
        {
        }

        protected MatrixKeyFrame(Matrix3x2 value) : base(value)
        {
        }

        protected MatrixKeyFrame(Matrix3x2 value, KeyTime keyTime) : base(value, keyTime)
        {
        }
    }


    /// <summary>
    /// This class is used as part of a ObjectKeyFrameCollection in
    /// conjunction with a KeyFrameObjectAnimation to animate a
    /// Object property value along a set of key frames.
    /// </summary>
    public abstract class ObjectKeyFrame : GenericAnimationKeyFrame<object?>
    {
        protected ObjectKeyFrame()
        {
        }

        protected ObjectKeyFrame(object? value) : base(value)
        {
        }

        protected ObjectKeyFrame(object? value, KeyTime keyTime) : base(value, keyTime)
        {
        }
    }


    /// <summary>
    /// This class is used as part of a PointKeyFrameCollection in
    /// conjunction with a KeyFramePointAnimation to animate a
    /// Point property value along a set of key frames.
    /// </summary>
    public abstract class PointKeyFrame : GenericAnimationKeyFrame<Point>
    {
        protected PointKeyFrame()
        {
        }

        protected PointKeyFrame(Point value) : base(value)
        {
        }

        protected PointKeyFrame(Point value, KeyTime keyTime) : base(value, keyTime)
        {
        }
    }


    ///// <summary>
    ///// This class is used as part of a Point3DKeyFrameCollection in
    ///// conjunction with a KeyFramePoint3DAnimation to animate a
    ///// Point3D property value along a set of key frames.
    ///// </summary>
    //public abstract class Point3DKeyFrame : Freezable, IKeyFrame
    //{
    //    #region Constructors

    //    /// <summary>
    //    /// Creates a new Point3DKeyFrame.
    //    /// </summary>
    //    protected Point3DKeyFrame()
    //        : base()
    //    {
    //    }

    //    /// <summary>
    //    /// Creates a new Point3DKeyFrame.
    //    /// </summary>
    //    protected Point3DKeyFrame(Point3D value)
    //        : this()
    //    {
    //        Value = value;
    //    }

    //    /// <summary>
    //    /// Creates a new DiscretePoint3DKeyFrame.
    //    /// </summary>
    //    protected Point3DKeyFrame(Point3D value, KeyTime keyTime)
    //        : this()
    //    {
    //        Value = value;
    //        KeyTime = keyTime;
    //    }

    //    #endregion

    //    #region IKeyFrame

    //    /// <summary>
    //    /// KeyTime Property
    //    /// </summary>
    //    public static readonly DependencyProperty KeyTimeProperty =
    //        DependencyProperty.Register(
    //                "KeyTime",
    //                typeof(KeyTime),
    //                typeof(Point3DKeyFrame),
    //                new PropertyMetadata(KeyTime.Uniform));

    //    /// <summary>
    //    /// The time at which this KeyFrame's value should be equal to the Value
    //    /// property.
    //    /// </summary>
    //    public KeyTime KeyTime
    //    {
    //        get
    //        {
    //            return (KeyTime)GetValue(KeyTimeProperty)!;
    //        }
    //        set
    //        {
    //            SetValue(KeyTimeProperty, value);
    //        }
    //    }

    //    /// <summary>
    //    /// Value Property
    //    /// </summary>
    //    public static readonly DependencyProperty ValueProperty =
    //        DependencyProperty.Register(
    //                "Value",
    //                typeof(Point3D),
    //                typeof(Point3DKeyFrame),
    //                new PropertyMetadata());

    //    /// <summary>
    //    /// The value of this key frame at the KeyTime specified.
    //    /// </summary>
    //    object? IKeyFrame.Value
    //    {
    //        get
    //        {
    //            return Value;
    //        }
    //        set
    //        {
    //            Value = (Point3D)value;
    //        }
    //    }

    //    /// <summary>
    //    /// The value of this key frame at the KeyTime specified.
    //    /// </summary>
    //    public Point3D Value
    //    {
    //        get
    //        {
    //            return (Point3D)GetValue(ValueProperty);
    //        }
    //        set
    //        {
    //            SetValue(ValueProperty, value);
    //        }
    //    }

    //    #endregion

    //    #region Public Methods

    //    /// <summary>
    //    /// Gets the interpolated value of the key frame at the progress value
    //    /// provided.  The progress value should be calculated in terms of this 
    //    /// specific key frame.
    //    /// </summary>
    //    public Point3D InterpolateValue(
    //        Point3D baseValue,
    //        float keyFrameProgress)
    //    {
    //        if (keyFrameProgress < 0.0
    //            || keyFrameProgress > 1.0)
    //        {
    //            throw new ArgumentOutOfRangeException("keyFrameProgress");
    //        }

    //        return InterpolateValueCore(baseValue, keyFrameProgress);
    //    }

    //    #endregion

    //    #region Protected Methods

    //    /// <summary>
    //    /// This method should be implemented by derived classes to calculate
    //    /// the value of this key frame at the progress value provided.
    //    /// </summary>
    //    protected abstract Point3D InterpolateValueCore(
    //        Point3D baseValue,
    //        float keyFrameProgress);

    //    #endregion
    //}


    /// <summary>
    /// This class is used as part of a QuaternionKeyFrameCollection in
    /// conjunction with a KeyFrameQuaternionAnimation to animate a
    /// Quaternion property value along a set of key frames.
    /// </summary>
    public abstract class QuaternionKeyFrame : GenericAnimationKeyFrame<Quaternion>
    {
        protected QuaternionKeyFrame()
        {
        }

        protected QuaternionKeyFrame(Quaternion value) : base(value)
        {
        }

        protected QuaternionKeyFrame(Quaternion value, KeyTime keyTime) : base(value, keyTime)
        {
        }
    }


    ///// <summary>
    ///// This class is used as part of a Rotation3DKeyFrameCollection in
    ///// conjunction with a KeyFrameRotation3DAnimation to animate a
    ///// Rotation3D property value along a set of key frames.
    ///// </summary>
    //public abstract class Rotation3DKeyFrame : Freezable, IKeyFrame
    //{
    //    #region Constructors

    //    /// <summary>
    //    /// Creates a new Rotation3DKeyFrame.
    //    /// </summary>
    //    protected Rotation3DKeyFrame()
    //        : base()
    //    {
    //    }

    //    /// <summary>
    //    /// Creates a new Rotation3DKeyFrame.
    //    /// </summary>
    //    protected Rotation3DKeyFrame(Rotation3D value)
    //        : this()
    //    {
    //        Value = value;
    //    }

    //    /// <summary>
    //    /// Creates a new DiscreteRotation3DKeyFrame.
    //    /// </summary>
    //    protected Rotation3DKeyFrame(Rotation3D value, KeyTime keyTime)
    //        : this()
    //    {
    //        Value = value;
    //        KeyTime = keyTime;
    //    }

    //    #endregion

    //    #region IKeyFrame

    //    /// <summary>
    //    /// KeyTime Property
    //    /// </summary>
    //    public static readonly DependencyProperty KeyTimeProperty =
    //        DependencyProperty.Register(
    //                "KeyTime",
    //                typeof(KeyTime),
    //                typeof(Rotation3DKeyFrame),
    //                new PropertyMetadata(KeyTime.Uniform));

    //    /// <summary>
    //    /// The time at which this KeyFrame's value should be equal to the Value
    //    /// property.
    //    /// </summary>
    //    public KeyTime KeyTime
    //    {
    //        get
    //        {
    //            return (KeyTime)GetValue(KeyTimeProperty)!;
    //        }
    //        set
    //        {
    //            SetValue(KeyTimeProperty, value);
    //        }
    //    }

    //    /// <summary>
    //    /// Value Property
    //    /// </summary>
    //    public static readonly DependencyProperty ValueProperty =
    //        DependencyProperty.Register(
    //                "Value",
    //                typeof(Rotation3D),
    //                typeof(Rotation3DKeyFrame),
    //                new PropertyMetadata());

    //    /// <summary>
    //    /// The value of this key frame at the KeyTime specified.
    //    /// </summary>
    //    object? IKeyFrame.Value
    //    {
    //        get
    //        {
    //            return Value;
    //        }
    //        set
    //        {
    //            Value = (Rotation3D)value;
    //        }
    //    }

    //    /// <summary>
    //    /// The value of this key frame at the KeyTime specified.
    //    /// </summary>
    //    public Rotation3D Value
    //    {
    //        get
    //        {
    //            return (Rotation3D)GetValue(ValueProperty);
    //        }
    //        set
    //        {
    //            SetValue(ValueProperty, value);
    //        }
    //    }

    //    #endregion

    //    #region Public Methods

    //    /// <summary>
    //    /// Gets the interpolated value of the key frame at the progress value
    //    /// provided.  The progress value should be calculated in terms of this 
    //    /// specific key frame.
    //    /// </summary>
    //    public Rotation3D InterpolateValue(
    //        Rotation3D baseValue,
    //        float keyFrameProgress)
    //    {
    //        if (keyFrameProgress < 0.0
    //            || keyFrameProgress > 1.0)
    //        {
    //            throw new ArgumentOutOfRangeException("keyFrameProgress");
    //        }

    //        return InterpolateValueCore(baseValue, keyFrameProgress);
    //    }

    //    #endregion

    //    #region Protected Methods

    //    /// <summary>
    //    /// This method should be implemented by derived classes to calculate
    //    /// the value of this key frame at the progress value provided.
    //    /// </summary>
    //    protected abstract Rotation3D InterpolateValueCore(
    //        Rotation3D baseValue,
    //        float keyFrameProgress);

    //    #endregion
    //}


    /// <summary>
    /// This class is used as part of a RectKeyFrameCollection in
    /// conjunction with a KeyFrameRectAnimation to animate a
    /// Rect property value along a set of key frames.
    /// </summary>
    public abstract class RectKeyFrame : GenericAnimationKeyFrame<Rect>
    {
        protected RectKeyFrame()
        {
        }

        protected RectKeyFrame(Rect value) : base(value)
        {
        }

        protected RectKeyFrame(Rect value, KeyTime keyTime) : base(value, keyTime)
        {
        }
    }


    /// <summary>
    /// This class is used as part of a SingleKeyFrameCollection in
    /// conjunction with a KeyFrameSingleAnimation to animate a
    /// Single property value along a set of key frames.
    /// </summary>
    public abstract class SingleKeyFrame : GenericAnimationKeyFrame<float>
    {
        protected SingleKeyFrame()
        {
        }

        protected SingleKeyFrame(float value) : base(value)
        {
        }

        protected SingleKeyFrame(float value, KeyTime keyTime) : base(value, keyTime)
        {
        }
    }


    /// <summary>
    /// This class is used as part of a SizeKeyFrameCollection in
    /// conjunction with a KeyFrameSizeAnimation to animate a
    /// Size property value along a set of key frames.
    /// </summary>
    public abstract class SizeKeyFrame : GenericAnimationKeyFrame<Size>
    {
        protected SizeKeyFrame()
        {
        }

        protected SizeKeyFrame(Size value) : base(value)
        {
        }

        protected SizeKeyFrame(Size value, KeyTime keyTime) : base(value, keyTime)
        {
        }
    }


    /// <summary>
    /// This class is used as part of a StringKeyFrameCollection in
    /// conjunction with a KeyFrameStringAnimation to animate a
    /// String property value along a set of key frames.
    /// </summary>
    public abstract class StringKeyFrame : GenericAnimationKeyFrame<string>
    {
        protected StringKeyFrame()
        {
        }

        protected StringKeyFrame(string value) : base(value)
        {
        }

        protected StringKeyFrame(string value, KeyTime keyTime) : base(value, keyTime)
        {
        }
    }


    /// <summary>
    /// This class is used as part of a VectorKeyFrameCollection in
    /// conjunction with a KeyFrameVectorAnimation to animate a
    /// Vector2 property value along a set of key frames.
    /// </summary>
    public abstract class VectorKeyFrame : GenericAnimationKeyFrame<Vector2>
    {
        protected VectorKeyFrame()
        {
        }

        protected VectorKeyFrame(Vector2 value) : base(value)
        {
        }

        protected VectorKeyFrame(Vector2 value, KeyTime keyTime) : base(value, keyTime)
        {
        }
    }


    /// <summary>
    /// This class is used as part of a Vector3DKeyFrameCollection in
    /// conjunction with a KeyFrameVector3DAnimation to animate a
    /// Vector3 property value along a set of key frames.
    /// </summary>
    public abstract class Vector3DKeyFrame : GenericAnimationKeyFrame<Vector3>
    {
        protected Vector3DKeyFrame()
        {
        }

        protected Vector3DKeyFrame(Vector3 value) : base(value)
        {
        }

        protected Vector3DKeyFrame(Vector3 value, KeyTime keyTime) : base(value, keyTime)
        {
        }
    }
}
