﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    public sealed class ClockController
    {
        private readonly Clock _clock;

        internal ClockController(Clock clock)
        {
            _clock = clock;
        }

        public Clock Clock => _clock;

        public float SpeedRatio
        {
            get => _clock.SpeedRatio; set
            {
                if (value < 0 || value > float.MaxValue || float.IsNaN(value))
                    throw new ArgumentException("Speed ratio value invalid.", "value");
                _clock.SpeedRatio = value;
            }
        }

        /// <summary>
        /// Schedules a begin at the next tick.
        /// </summary>
        /// <remarks>
        /// <p>If the Clock is not enabled then this method has no effect.</p>
        ///
        /// <p>This method has no effect on the timing tree until the next time
        /// a tick is processed. As a side-effect, the appropriate events will also
        /// not be raised until then.</p>
        /// </remarks>
        // Internally, Begin works similarly to Seek, in that it preschedules a seek in
        // the Clock to the zero position.
        public void Begin()
        {
            _clock.Begin();
        }

        /// <summary>
        /// Moves this clock to the end of its active period and then performs
        /// whatever behavior is specified by the FillBehavior property.
        /// </summary>
        /// <remarks>
        /// <p>This method only has an effect if the Clock's CurrentState is ClockState.Active.</p>
        /// <p>This method has no effect on the timing tree until the next time
        /// a tick is processed. As a side-effect, the appropriate events will also
        /// not be raised until then.</p>
        /// </remarks>
        // Internally, SkipToFill works similarly to Seek, in that it preschedules a seek in
        // the Clock to the Fill position.
        public void SkipToFill()
        {
            if (_clock.TotalDuration == Duration.Forever)
                throw new InvalidOperationException("Clock is indefinite.");
            _clock.SkipToFill();
        }

        /// <summary>
        /// Pauses the Clock and its children.
        /// </summary>
        /// <remarks>
        /// <p>This method stops this Clock from moving. As a side effect,
        /// the Clocks's children are also paused. The Clock remains
        /// paused until one of the following events take place:</p>
        /// <list type="bullet">
        ///     <item>
        ///     The Clock is allowed to continue with a call to the
        ///     <see cref="ClockController.Resume"/> method.
        ///     </item>
        ///     <item>
        ///     The Clock is restarted or ended with a call to the
        ///     <see cref="ClockController.Begin"/> or <see cref="ClockController.SkipToFill"/> methods.
        ///     </item>
        ///     <item>
        ///     The Clock is restarted due to a scheduled begin time.
        ///     </item>
        ///     <item>
        ///     The Clock is removed from the timing tree.
        ///     </item>
        /// </list>
        /// 
        /// <p>This method has no effect if the Clock is not active, or is currently paused.</p>
        /// </remarks>
        /// <seealso cref="ClockController.Resume"/>
        public void Pause()
        {
            _clock.Pause();
        }

        /// <summary>
        /// Allows a Clock to progress again after a call to <see cref="ClockController.Pause"/>.
        /// </summary>
        /// <remarks>
        /// <p>Resuming a Clock also automatically resumes all of the Clock's children.</p>
        ///
        /// <p>This method has no effect if the Clock is not active and in the paused
        /// state. In addition, only a Clock that was previously explicitly paused with
        /// a call to <see cref="ClockController.Pause"/> can be resumed with this method.</p>
        ///
        /// <p>For more details, see the remarks for the <see cref="ClockController.Pause"/>
        /// method.</p>
        /// </remarks>
        /// <seealso cref="ClockController.Pause"/>
        public void Resume()
        {
            _clock.Resume();
        }

        /// <summary>
        /// Seeks a Clock to a new position.
        /// </summary>
        /// <param name="offset">
        /// The seek offset, measured in the Clock's simple time frame of
        /// reference. The meaning of this parameter depends on the value of the origin parameter.
        /// </param>
        /// <param name="origin">
        /// The meaning of the offset parameter. See the <see cref="TimeSeekOrigin"/> enumeration
        /// for possible values.
        /// </param>
        /// <remarks>
        /// <p>If the Clock is a container, its children's Clocks are also updated accordingly.</p>
        ///
        /// <p>The seek is measured in the Clock's simple time frame of reference, meaning that the
        /// actual wall-clock playback time skipped (or replayed) may be different than that specified
        /// by the offset parameter, if time manipulations from the Speed, Acceleration or Deceleration
        /// properties are in effect for this Clock.</p>
        ///
        /// <p>The seek operation may only span the current simple duration of the Clock. Seeking to a
        /// time earlier than the begin time positions the Clock at the begin point, whereas
        /// seeking beyond the end simply puts the Clock at the end point.</p>
        /// </remarks>
        public void Seek(TimeSpan offset, TimeSeekOrigin origin)
        {
            // IF YOU CHANGE THIS CODE:
            // This code is very similar to that in Seek and is duplicated
            // in each method so that exceptions will be thrown from the public 
            // method the user has called. You probably need to change both methods.

            if (origin == TimeSeekOrigin.Duration)
            {
                Duration duration = _clock.Duration;

                if (!duration.HasTimeSpan)
                {
                    // Can't seek relative to the Duration if it has been specified as Forevor or if
                    // it has not yet been resolved.
                    throw new InvalidOperationException("Duration is indefinite.");
                }
                //else
                //{
                //    offset = offset + duration.TimeSpan;
                //}
            }

            // Any offset greater than zero is OK here. If it's past the effective
            // duration it means execute the FillBehavior.
            if (offset < TimeSpan.Zero)
            {
                throw new InvalidOperationException("Offset can't be negative.");
            }

            if (origin == TimeSeekOrigin.Duration)
                _clock.ApplyTick(offset);
            else
                _clock.ApplyTimeSpan(_clock.BeginTime + offset, false);
        }

        /// <summary>
        /// Process all information that occured until now
        /// </summary>

        public void SeekAlignedToLastTick(TimeSpan offset, TimeSeekOrigin origin)
        {
            // IF YOU CHANGE THIS CODE:
            // This code is very similar to that in Seek and is duplicated
            // in each method so that exceptions will be thrown from the public 
            // method the user has called. You probably need to change both methods.

            if (origin == TimeSeekOrigin.Duration)
            {
                Duration duration = _clock.Duration;

                if (!duration.HasTimeSpan)
                {
                    // Can't seek relative to the Duration if it has been specified as Forevor or if
                    // it has not yet been resolved.
                    throw new InvalidOperationException("Duration is indefinite.");
                }
                //else
                //{
                //    offset = offset + duration.TimeSpan;
                //}
            }

            // Any offset greater than zero is OK here. If it's past the effective
            // duration it means execute the FillBehavior.
            if (offset < TimeSpan.Zero)
            {
                throw new InvalidOperationException("Offset can't be negative.");
            }

            if (origin == TimeSeekOrigin.Duration)
                _clock.ApplyTick(offset);
            else
                _clock.ApplyTimeSpan(_clock.BeginTime + offset, false);
        }

        /// <summary>
        /// Interactively stops the Clock.
        /// </summary>
        /// <remarks>
        /// This takes the Clock out of its active or fill period and leaves it
        /// in the off state.  It may be reactivated with an interactive Begin().
        /// </remarks>
        public void Stop()
        {
            _clock.Stop();
        }

        /// <summary>
        /// Interactively moves the Clock into a Completed state, fires Remove event.
        /// </summary>
        /// <remarks>
        /// This takes the Clock out of its active or fill period and leaves it
        /// in the off state.  It may be reactivated with an interactive Begin().
        /// </remarks>
        public void Remove()
        {
            _clock.Remove();
        }
    }
}
