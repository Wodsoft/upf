using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media.Animation
{
    /// <summary>
    /// This collection is used in conjunction with a KeyFrameBooleanAnimation
    /// to animate a Boolean property value along a set of key frames.
    /// </summary>
    public class BooleanKeyFrameCollection : KeyFrameCollection<BooleanKeyFrame>
    {
        #region Data

        private static BooleanKeyFrameCollection? _EmptyCollection;

        #endregion

        #region Static Methods

        /// <summary>
        /// An empty BooleanKeyFrameCollection.
        /// </summary>
        public static BooleanKeyFrameCollection Empty
        {
            get
            {
                if (_EmptyCollection == null)
                {
                    BooleanKeyFrameCollection emptyCollection = new BooleanKeyFrameCollection();
                    emptyCollection.Freeze();
                    _EmptyCollection = emptyCollection;
                }

                return _EmptyCollection;
            }
        }

        #endregion

        #region Freezable

        /// <summary>
        /// Creates a freezable copy of this BooleanKeyFrameCollection.
        /// </summary>
        /// <returns>The copy</returns>
        public new BooleanKeyFrameCollection Clone()
        {
            return (BooleanKeyFrameCollection)base.Clone();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CreateInstanceCore">Freezable.CreateInstanceCore</see>.
        /// </summary>
        /// <returns>The new Freezable.</returns>
        protected override Freezable CreateInstanceCore()
        {
            return new BooleanKeyFrameCollection();
        }

        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CloneCore(System.Windows.Freezable)">Freezable.CloneCore</see>.
        /// </summary>
        protected override void CloneCore(Freezable sourceFreezable)
        {
            BooleanKeyFrameCollection sourceCollection = (BooleanKeyFrameCollection)sourceFreezable;
            base.CloneCore(sourceFreezable);

            int count = sourceCollection.Count;

            KeyFrames = new List<BooleanKeyFrame>(count);

            for (int i = 0; i < count; i++)
            {
                BooleanKeyFrame keyFrame = (BooleanKeyFrame)sourceCollection.KeyFrames[i].Clone();
                KeyFrames.Add(keyFrame);
                OnFreezablePropertyChanged(null, keyFrame);
            }
        }


        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.CloneCurrentValueCore(System.Windows.Freezable)">Freezable.CloneCurrentValueCore</see>.
        /// </summary>
        protected override void CloneCurrentValueCore(Freezable sourceFreezable)
        {
            BooleanKeyFrameCollection sourceCollection = (BooleanKeyFrameCollection)sourceFreezable;
            base.CloneCurrentValueCore(sourceFreezable);

            int count = sourceCollection.KeyFrames.Count;

            KeyFrames = new List<BooleanKeyFrame>(count);

            for (int i = 0; i < count; i++)
            {
                BooleanKeyFrame keyFrame = (BooleanKeyFrame)sourceCollection.KeyFrames[i].CloneCurrentValue();
                KeyFrames.Add(keyFrame);
                OnFreezablePropertyChanged(null, keyFrame);
            }
        }


        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.GetAsFrozenCore(System.Windows.Freezable)">Freezable.GetAsFrozenCore</see>.
        /// </summary>
        protected override void GetAsFrozenCore(Freezable sourceFreezable)
        {
            BooleanKeyFrameCollection sourceCollection = (BooleanKeyFrameCollection)sourceFreezable;
            base.GetAsFrozenCore(sourceFreezable);

            int count = sourceCollection.KeyFrames.Count;

            KeyFrames = new List<BooleanKeyFrame>(count);

            for (int i = 0; i < count; i++)
            {
                BooleanKeyFrame keyFrame = (BooleanKeyFrame)sourceCollection.KeyFrames[i].GetAsFrozen();
                KeyFrames.Add(keyFrame);
                OnFreezablePropertyChanged(null, keyFrame);
            }
        }


        /// <summary>
        /// Implementation of <see cref="System.Windows.Freezable.GetCurrentValueAsFrozenCore(System.Windows.Freezable)">Freezable.GetCurrentValueAsFrozenCore</see>.
        /// </summary>
        protected override void GetCurrentValueAsFrozenCore(Freezable sourceFreezable)
        {
            BooleanKeyFrameCollection sourceCollection = (BooleanKeyFrameCollection)sourceFreezable;
            base.GetCurrentValueAsFrozenCore(sourceFreezable);

            int count = sourceCollection.KeyFrames.Count;

            KeyFrames = new List<BooleanKeyFrame>(count);

            for (int i = 0; i < count; i++)
            {
                BooleanKeyFrame keyFrame = (BooleanKeyFrame)sourceCollection.KeyFrames[i].GetCurrentValueAsFrozen();
                KeyFrames.Add(keyFrame);
                OnFreezablePropertyChanged(null, keyFrame);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected override bool FreezeCore(bool isChecking)
        {
            bool canFreeze = base.FreezeCore(isChecking);

            for (int i = 0; i < KeyFrames.Count && canFreeze; i++)
            {
                canFreeze &= Freezable.Freeze(KeyFrames[i], isChecking);
            }

            return canFreeze;
        }

        #endregion
    }
}
