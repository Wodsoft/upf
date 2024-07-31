using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public abstract class NumberAnimation<T> : GenericAnimationBase<T>
        where T : struct, INumber<T>
    {
        #region Data

        /// <summary>
        /// This is used if the user has specified From, To, and/or By values.
        /// </summary>
        private T[]? _keyValues;

        private AnimationType _animationType;
        private bool _isAnimationFunctionValid;

        #endregion

        #region Constructors

        static NumberAnimation()
        {
            Type typeofProp = typeof(T?);
            Type typeofThis = typeof(NumberAnimation<T>);
            PropertyChangedCallback propCallback = new PropertyChangedCallback(AnimationFunction_Changed);
            ValidateValueCallback validateCallback = new ValidateValueCallback(ValidateFromToOrByValue);

            FromProperty = DependencyProperty.Register(
                "From",
                typeofProp,
                typeofThis,
                new PropertyMetadata((T?)null, propCallback),
                validateCallback);

            ToProperty = DependencyProperty.Register(
                "To",
                typeofProp,
                typeofThis,
                new PropertyMetadata((T?)null, propCallback),
                validateCallback);

            ByProperty = DependencyProperty.Register(
                "By",
                typeofProp,
                typeofThis,
                new PropertyMetadata((T?)null, propCallback),
                validateCallback);

            EasingFunctionProperty = DependencyProperty.Register(
                "EasingFunction",
                typeof(IEasingFunction),
                typeofThis);
        }


        /// <summary>
        /// Creates a new NumberAnimation with all properties set to
        /// their default values.
        /// </summary>
        public NumberAnimation()
            : base()
        {
        }

        /// <summary>
        /// Creates a new NumberAnimation that will animate a
        /// <typeparamref name="T">T</typeparamref> property from its base value to the value specified
        /// by the "toValue" parameter of this constructor.
        /// </summary>
        public NumberAnimation(T toValue, Duration duration)
            : this()
        {
            To = toValue;
            Duration = duration;
        }

        /// <summary>
        /// Creates a new NumberAnimation that will animate a
        /// <typeparamref name="T">T</typeparamref> property from its base value to the value specified
        /// by the "toValue" parameter of this constructor.
        /// </summary>
        public NumberAnimation(T toValue, Duration duration, FillBehavior fillBehavior)
            : this()
        {
            To = toValue;
            Duration = duration;
            FillBehavior = fillBehavior;
        }

        /// <summary>
        /// Creates a new GenericAnimation that will animate a
        /// <typeparamref name="T">T</typeparamref> property from the "fromValue" parameter of this constructor
        /// to the "toValue" parameter.
        /// </summary>
        public NumberAnimation(T fromValue, T toValue, Duration duration)
            : this()
        {
            From = fromValue;
            To = toValue;
            Duration = duration;
        }

        /// <summary>
        /// Creates a new NumberAnimation that will animate a
        /// <typeparamref name="T">T</typeparamref> property from the "fromValue" parameter of this constructor
        /// to the "toValue" parameter.
        /// </summary>
        public NumberAnimation(T fromValue, T toValue, Duration duration, FillBehavior fillBehavior)
            : this()
        {
            From = fromValue;
            To = toValue;
            Duration = duration;
            FillBehavior = fillBehavior;
        }

        #endregion

        #region Methods

        protected virtual T Interpolate(T from, T to, float progress) => from + Scale(to - from, progress);

        protected abstract T Scale(T value, float factor);

        /// <summary>
        /// Calculates the value this animation believes should be the current value for the property.
        /// </summary>
        /// <param name="defaultOriginValue">
        /// This value is the suggested origin value provided to the animation
        /// to be used if the animation does not have its own concept of a
        /// start value. If this animation is the first in a composition chain
        /// this value will be the snapshot value if one is available or the
        /// base property value if it is not; otherise this value will be the 
        /// value returned by the previous animation in the chain with an 
        /// animationClock that is not Stopped.
        /// </param>
        /// <param name="defaultDestinationValue">
        /// This value is the suggested destination value provided to the animation
        /// to be used if the animation does not have its own concept of an
        /// end value. This value will be the base value if the animation is
        /// in the first composition layer of animations on a property; 
        /// otherwise this value will be the output value from the previous 
        /// composition layer of animations for the property.
        /// </param>
        /// <param name="animationClock">
        /// This is the animationClock which can generate the CurrentTime or
        /// CurrentProgress value to be used by the animation to generate its
        /// output value.
        /// </param>
        /// <returns>
        /// The value this animation believes should be the current value for the property.
        /// </returns>
        protected override T GetCurrentValueCore(T defaultOriginValue, T defaultDestinationValue, AnimationClock animationClock)
        {
            if (!_isAnimationFunctionValid)
            {
                ValidateAnimationFunction();
            }

            float progress = animationClock.CurrentProgress;

            IEasingFunction? easingFunction = EasingFunction;
            if (easingFunction != null)
            {
                progress = easingFunction.Ease(progress);
            }

            T from = default;
            T to = default;
            T accumulated = default;
            T foundation = default;

            // need to validate the default origin and destination values if 
            // the animation uses them as the from, to, or foundation values
            bool validateOrigin = false;
            bool validateDestination = false;

            switch (_animationType)
            {
                case AnimationType.Automatic:

                    from = defaultOriginValue;
                    to = defaultDestinationValue;

                    validateOrigin = true;
                    validateDestination = true;

                    break;

                case AnimationType.From:

                    from = _keyValues![0];
                    to = defaultDestinationValue;

                    validateDestination = true;

                    break;

                case AnimationType.To:

                    from = defaultOriginValue;
                    to = _keyValues![0];

                    validateOrigin = true;

                    break;

                case AnimationType.By:

                    // According to the SMIL specification, a By animation is
                    // always additive.  But we don't force this so that a
                    // user can re-use a By animation and have it replace the
                    // animations that precede it in the list without having
                    // to manually set the From value to the base value.

                    to = _keyValues![0];
                    foundation = defaultOriginValue;

                    validateOrigin = true;

                    break;

                case AnimationType.FromTo:

                    from = _keyValues![0];
                    to = _keyValues[1];

                    if (IsAdditive)
                    {
                        foundation = defaultOriginValue;
                        validateOrigin = true;
                    }

                    break;

                case AnimationType.FromBy:

                    from = _keyValues![0];
                    to = _keyValues[0] + _keyValues[1];

                    if (IsAdditive)
                    {
                        foundation = defaultOriginValue;
                        validateOrigin = true;
                    }

                    break;

                default:

                    Debug.Fail("Unknown animation type.");

                    break;
            }

            if (validateOrigin
                && !IsValidAnimationValue(defaultOriginValue))
            {
                throw new InvalidOperationException("Invalid default origin value.");
            }

            if (validateDestination
                && !IsValidAnimationValue(defaultDestinationValue))
            {
                throw new InvalidOperationException("Invalid default destination value.");
            }


            if (IsCumulative)
            {
                float currentRepeat = animationClock.CurrentIteration - 1;

                if (currentRepeat > 0.0)
                {
                    T accumulator = to - from;

                    accumulated = Scale(accumulator, currentRepeat);
                }
            }

            // return foundation + accumulated + from + ((to - from) * progress)

            return foundation + accumulated + Interpolate(from, to, progress);
        }

        private void ValidateAnimationFunction()
        {
            _animationType = AnimationType.Automatic;
            _keyValues = null;

            if (From.HasValue)
            {
                if (To.HasValue)
                {
                    _animationType = AnimationType.FromTo;
                    _keyValues = new T[2];
                    _keyValues[0] = From.Value;
                    _keyValues[1] = To.Value;
                }
                else if (By.HasValue)
                {
                    _animationType = AnimationType.FromBy;
                    _keyValues = new T[2];
                    _keyValues[0] = From.Value;
                    _keyValues[1] = By.Value;
                }
                else
                {
                    _animationType = AnimationType.From;
                    _keyValues = new T[1];
                    _keyValues[0] = From.Value;
                }
            }
            else if (To.HasValue)
            {
                _animationType = AnimationType.To;
                _keyValues = new T[1];
                _keyValues[0] = To.Value;
            }
            else if (By.HasValue)
            {
                _animationType = AnimationType.By;
                _keyValues = new T[1];
                _keyValues[0] = By.Value;
            }

            _isAnimationFunctionValid = true;
        }

        protected virtual bool IsValidAnimationValue(in T value) => !T.IsInfinity(value) && !T.IsNaN(value);

        #endregion

        #region Properties

        private static void AnimationFunction_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((NumberAnimation<T>)d)._isAnimationFunctionValid = false;
        }

        private static bool ValidateFromToOrByValue(object? value)
        {
            T? typedValue = (T?)value;

            if (typedValue.HasValue)
            {
                return !T.IsInfinity(typedValue.Value) && !T.IsNaN(typedValue.Value);
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// FromProperty
        /// </summary>                                 
        public static readonly DependencyProperty FromProperty;

        /// <summary>
        /// From
        /// </summary>
        public T? From
        {
            get
            {
                return (T?)GetValue(FromProperty);
            }
            set
            {
                SetValue(FromProperty, value);
            }
        }

        /// <summary>
        /// ToProperty
        /// </summary>
        public static readonly DependencyProperty ToProperty;

        /// <summary>
        /// To
        /// </summary>
        public T? To
        {
            get
            {
                return (T?)GetValue(ToProperty);
            }
            set
            {
                SetValue(ToProperty, value);
            }
        }

        /// <summary>
        /// ByProperty
        /// </summary>
        public static readonly DependencyProperty ByProperty;

        /// <summary>
        /// By
        /// </summary>
        public T? By
        {
            get
            {
                return (T?)GetValue(ByProperty);
            }
            set
            {
                SetValue(ByProperty, value);
            }
        }


        /// <summary>
        /// EasingFunctionProperty
        /// </summary>                                 
        public static readonly DependencyProperty EasingFunctionProperty;

        /// <summary>
        /// EasingFunction
        /// </summary>
        public IEasingFunction? EasingFunction
        {
            get
            {
                return (IEasingFunction?)GetValue(EasingFunctionProperty);
            }
            set
            {
                SetValue(EasingFunctionProperty, value);
            }
        }

        /// <summary>
        /// If this property is set to true the animation will add its value to
        /// the base value instead of replacing it entirely.
        /// </summary>
        public bool IsAdditive
        {
            get
            {
                return (bool)GetValue(IsAdditiveProperty)!;
            }
            set
            {
                SetValue(IsAdditiveProperty, value);
            }
        }

        /// <summary>
        /// It this property is set to true, the animation will accumulate its
        /// value over repeats.  For instance if you have a From value of 0.0 and
        /// a To value of 1.0, the animation return values from 1.0 to 2.0 over
        /// the second reteat cycle, and 2.0 to 3.0 over the third, etc.
        /// </summary>
        public bool IsCumulative
        {
            get
            {
                return (bool)GetValue(IsCumulativeProperty)!;
            }
            set
            {
                SetValue(IsCumulativeProperty, value);
            }
        }

        #endregion
    }
}
