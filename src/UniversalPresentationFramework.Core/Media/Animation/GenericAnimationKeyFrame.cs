using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public abstract class GenericAnimationKeyFrame<T> : Freezable, IKeyFrame
    {
        #region Constructors

        /// <summary>
        /// Creates a new NumberKeyFrame.
        /// </summary>
        protected GenericAnimationKeyFrame()
            : base()
        {
        }

        /// <summary>
        /// Creates a new NumberKeyFrame.
        /// </summary>
        protected GenericAnimationKeyFrame(T value)
            : this()
        {
            Value = value;
        }

        /// <summary>
        /// Creates a new DiscreteNumberKeyFrame.
        /// </summary>
        protected GenericAnimationKeyFrame(T value, KeyTime keyTime)
            : this()
        {
            Value = value;
            KeyTime = keyTime;
        }

        #endregion

        #region IKeyFrame

        /// <summary>
        /// KeyTime Property
        /// </summary>
        public static readonly DependencyProperty KeyTimeProperty =
            DependencyProperty.Register(
                    "KeyTime",
                    typeof(KeyTime),
                    typeof(GenericAnimationKeyFrame<T>),
                    new PropertyMetadata(KeyTime.Uniform));

        /// <summary>
        /// The time at which this KeyFrame's value should be equal to the Value
        /// property.
        /// </summary>
        public KeyTime KeyTime
        {
            get
            {
                return (KeyTime)GetValue(KeyTimeProperty)!;
            }
            set
            {
                SetValue(KeyTimeProperty, value);
            }
        }

        /// <summary>
        /// Value Property
        /// </summary>
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(
                    "Value",
                    typeof(T),
                    typeof(GenericAnimationKeyFrame<T>),
                    new PropertyMetadata());

        /// <summary>
        /// The value of this key frame at the KeyTime specified.
        /// </summary>
        object? IKeyFrame.Value
        {
            get
            {
                return Value;
            }
            set
            {
                Value = (T)value!;
            }
        }

        /// <summary>
        /// The value of this key frame at the KeyTime specified.
        /// </summary>
        public T Value
        {
            get
            {
                return (T)GetValue(ValueProperty)!;
            }
            set
            {
                SetValue(ValueProperty, value);
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the interpolated value of the key frame at the progress value
        /// provided.  The progress value should be calculated in terms of this 
        /// specific key frame.
        /// </summary>
        public T InterpolateValue(
            T baseValue,
            float keyFrameProgress)
        {
            if (keyFrameProgress < 0.0f
                || keyFrameProgress > 1.0f)
            {
                throw new ArgumentOutOfRangeException("keyFrameProgress");
            }

            return InterpolateValueCore(baseValue, keyFrameProgress);
        }

        #endregion

        #region Protected Methods

        /// <summary>
        /// This method should be implemented by derived classes to calculate
        /// the value of this key frame at the progress value provided.
        /// </summary>
        protected abstract T InterpolateValueCore(
            T baseValue,
            float keyFrameProgress);

        #endregion
    }

    public abstract class DiscreteGenericAnimationKeyFrame<T> : GenericAnimationKeyFrame<T>
    {
        #region Constructors

        /// <summary>
        /// Creates a new NumberKeyFrame.
        /// </summary>
        protected DiscreteGenericAnimationKeyFrame() : base() { }

        /// <summary>
        /// Creates a new NumberKeyFrame.
        /// </summary>
        protected DiscreteGenericAnimationKeyFrame(T value) : base(value) { }

        /// <summary>
        /// Creates a new DiscreteNumberKeyFrame.
        /// </summary>
        protected DiscreteGenericAnimationKeyFrame(T value, KeyTime keyTime) : base(value, keyTime) { }

        #endregion

        protected override T InterpolateValueCore(T baseValue, float keyFrameProgress)
        {
            if (keyFrameProgress < 1.0f)
            {
                return baseValue;
            }
            else
            {
                return Value;
            }
        }
    }
}
