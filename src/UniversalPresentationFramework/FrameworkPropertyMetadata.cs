using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public class FrameworkPropertyMetadata : UIPropertyMetadata
    {
        /// <summary>
        ///     Framework type metadata construction.  Marked as no inline to reduce code size.
        /// </summary>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public FrameworkPropertyMetadata() :
            base()
        {
        }

        /// <summary>
        ///     Framework type metadata construction.  Marked as no inline to reduce code size.
        /// </summary>
        /// <param name="defaultValue">Default value of property</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public FrameworkPropertyMetadata(object? defaultValue) :
            base(defaultValue)
        {
        }

        /// <summary>
        ///     Framework type metadata construction.  Marked as no inline to reduce code size.
        /// </summary>
        /// <param name="propertyChangedCallback">Called when the property has been changed</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public FrameworkPropertyMetadata(PropertyChangedCallback? propertyChangedCallback) :
            base(propertyChangedCallback)
        {
        }

        /// <summary>
        ///     Framework type metadata construction.  Marked as no inline to reduce code size.
        /// </summary>
        /// <param name="propertyChangedCallback">Called when the property has been changed</param>
        /// <param name="coerceValueCallback">Called on update of value</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public FrameworkPropertyMetadata(PropertyChangedCallback? propertyChangedCallback,
                                            CoerceValueCallback? coerceValueCallback) :
            base(null, propertyChangedCallback, coerceValueCallback)
        {
        }

        /// <summary>
        ///     Framework type metadata construction.  Marked as no inline to reduce code size.
        /// </summary>
        /// <param name="defaultValue">Default value of property</param>
        /// <param name="propertyChangedCallback">Called when the property has been changed</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public FrameworkPropertyMetadata(object? defaultValue,
                                         PropertyChangedCallback? propertyChangedCallback) :
            base(defaultValue, propertyChangedCallback)
        {
        }

        /// <summary>
        ///     Framework type metadata construction.  Marked as no inline to reduce code size.
        /// </summary>
        /// <param name="defaultValue">Default value of property</param>
        /// <param name="propertyChangedCallback">Called when the property has been changed</param>
        /// <param name="coerceValueCallback">Called on update of value</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public FrameworkPropertyMetadata(object? defaultValue,
                                PropertyChangedCallback? propertyChangedCallback,
                                CoerceValueCallback? coerceValueCallback) :
            base(defaultValue, propertyChangedCallback, coerceValueCallback)
        {
        }

        /// <summary>
        ///     Framework type metadata construction.  Marked as no inline to reduce code size.
        /// </summary>
        /// <param name="defaultValue">Default value of property</param>
        /// <param name="flags">Metadata option flags</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public FrameworkPropertyMetadata(object? defaultValue, FrameworkPropertyMetadataOptions flags) :
            base(defaultValue)
        {
            TranslateFlags(flags);
        }

        /// <summary>
        ///     Framework type metadata construction.  Marked as no inline to reduce code size.
        /// </summary>
        /// <param name="defaultValue">Default value of property</param>
        /// <param name="flags">Metadata option flags</param>
        /// <param name="propertyChangedCallback">Called when the property has been changed</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public FrameworkPropertyMetadata(object? defaultValue,
                                         FrameworkPropertyMetadataOptions flags,
                                         PropertyChangedCallback? propertyChangedCallback) :
            base(defaultValue, propertyChangedCallback)
        {
            TranslateFlags(flags);
        }

        /// <summary>
        ///     Framework type metadata construction.  Marked as no inline to reduce code size.
        /// </summary>
        /// <param name="defaultValue">Default value of property</param>
        /// <param name="flags">Metadata option flags</param>
        /// <param name="propertyChangedCallback">Called when the property has been changed</param>
        /// <param name="coerceValueCallback">Called on update of value</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public FrameworkPropertyMetadata(object? defaultValue,
                                         FrameworkPropertyMetadataOptions flags,
                                         PropertyChangedCallback? propertyChangedCallback,
                                         CoerceValueCallback? coerceValueCallback) :
            base(defaultValue, propertyChangedCallback, coerceValueCallback)
        {
            TranslateFlags(flags);
        }

        /// <summary>
        ///     Framework type metadata construction.  Marked as no inline to reduce code size.
        /// </summary>
        /// <param name="defaultValue">Default value of property</param>
        /// <param name="flags">Metadata option flags</param>
        /// <param name="propertyChangedCallback">Called when the property has been changed</param>
        /// <param name="coerceValueCallback">Called on update of value</param>
        /// <param name="isAnimationProhibited">Should animation of this property be prohibited?</param>
        [System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        public FrameworkPropertyMetadata(object? defaultValue,
                                         FrameworkPropertyMetadataOptions flags,
                                         PropertyChangedCallback? propertyChangedCallback,
                                         CoerceValueCallback? coerceValueCallback,
                                         bool isAnimationProhibited) :
            base(defaultValue, propertyChangedCallback, coerceValueCallback, isAnimationProhibited)
        {
            TranslateFlags(flags);
        }

        ///// <summary>
        /////     Framework type metadata construction.  Marked as no inline to reduce code size.
        ///// </summary>
        ///// <param name="defaultValue">Default value of property</param>
        ///// <param name="flags">Metadata option flags</param>
        ///// <param name="propertyChangedCallback">Called when the property has been changed</param>
        ///// <param name="coerceValueCallback">Called on update of value</param>
        ///// <param name="isAnimationProhibited">Should animation of this property be prohibited?</param>
        ///// <param name="defaultUpdateSourceTrigger">The UpdateSourceTrigger to use for bindings that have UpdateSourceTriger=Default.</param>
        //[System.Runtime.CompilerServices.MethodImpl(System.Runtime.CompilerServices.MethodImplOptions.NoInlining)]
        //public FrameworkPropertyMetadata(object defaultValue,
        //                                 FrameworkPropertyMetadataOptions flags,
        //                                 PropertyChangedCallback propertyChangedCallback,
        //                                 CoerceValueCallback coerceValueCallback,
        //                                 bool isAnimationProhibited,
        //                                 UpdateSourceTrigger defaultUpdateSourceTrigger) :
        //    base(defaultValue, propertyChangedCallback, coerceValueCallback, isAnimationProhibited)
        //{
        //    if (!BindingOperations.IsValidUpdateSourceTrigger(defaultUpdateSourceTrigger))
        //        throw new InvalidEnumArgumentException("defaultUpdateSourceTrigger", (int)defaultUpdateSourceTrigger, typeof(UpdateSourceTrigger));
        //    if (defaultUpdateSourceTrigger == UpdateSourceTrigger.Default)
        //        throw new ArgumentException(SR.Get(SRID.NoDefaultUpdateSourceTrigger), "defaultUpdateSourceTrigger");

        //    TranslateFlags(flags);
        //    DefaultUpdateSourceTrigger = defaultUpdateSourceTrigger;
        //}

        private void TranslateFlags(FrameworkPropertyMetadataOptions flags)
        {
            Flags = flags;
        }

        public FrameworkPropertyMetadataOptions Flags { get; private set; }
    }

    [Flags]
    public enum FrameworkPropertyMetadataOptions : int
    {
        /// <summary>No flags</summary>
        None = 0x000,

        /// <summary>This property affects measurement</summary>
        AffectsMeasure = 0x001,

        /// <summary>This property affects arragement</summary>
        AffectsArrange = 0x002,

        /// <summary>This property affects parent's measurement</summary>
        AffectsParentMeasure = 0x004,

        /// <summary>This property affects parent's arrangement</summary>
        AffectsParentArrange = 0x008,

        /// <summary>This property affects rendering</summary>
        AffectsRender = 0x010,

        /// <summary>This property inherits to children</summary>
        Inherits = 0x020,

        /// <summary>
        /// This property causes inheritance and resource lookup to override values 
        /// of InheritanceBehavior that may be set on any FE in the path of lookup
        /// </summary>
        OverridesInheritanceBehavior = 0x040,

        /// <summary>This property does not support data binding</summary>
        NotDataBindable = 0x080,

        /// <summary>Data bindings on this property default to two-way</summary>
        BindsTwoWayByDefault = 0x100,

        /// <summary>This property should be saved/restored when journaling/navigating by URI</summary>
        Journal = 0x400,

        /// <summary>
        ///     This property's subproperties do not affect rendering.
        ///     For instance, a property X may have a subproperty Y.
        ///     Changing X.Y does not require rendering to be updated.
        /// </summary>
        SubPropertiesDoNotAffectRender = 0x800,
    }
}
