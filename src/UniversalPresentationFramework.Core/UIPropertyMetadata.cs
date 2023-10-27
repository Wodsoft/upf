using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public class UIPropertyMetadata : PropertyMetadata
    {/// <summary>
     ///     UI metadata construction
     /// </summary>
        public UIPropertyMetadata() :
            base()
        {
        }

        /// <summary>
        ///     UI metadata construction
        /// </summary>
        /// <param name="defaultValue">Default value of property</param>
        public UIPropertyMetadata(object? defaultValue) :
            base(defaultValue)
        {
        }

        /// <summary>
        ///     UI metadata construction
        /// </summary>
        /// <param name="propertyChangedCallback">Called when the property has been changed</param>
        public UIPropertyMetadata(PropertyChangedCallback? propertyChangedCallback) :
            base(propertyChangedCallback)
        {
        }

        /// <summary>
        ///     UI metadata construction
        /// </summary>
        /// <param name="defaultValue">Default value of property</param>
        /// <param name="propertyChangedCallback">Called when the property has been changed</param>
        public UIPropertyMetadata(object? defaultValue,
                                  PropertyChangedCallback? propertyChangedCallback) :
            base(defaultValue, propertyChangedCallback)
        {
        }

        /// <summary>
        ///     UI metadata construction
        /// </summary>
        /// <param name="defaultValue">Default value of property</param>
        /// <param name="propertyChangedCallback">Called when the property has been changed</param>
        /// <param name="coerceValueCallback">Called on update of value</param>
        public UIPropertyMetadata(object? defaultValue,
                                PropertyChangedCallback? propertyChangedCallback,
                                CoerceValueCallback? coerceValueCallback) :
            base(defaultValue, propertyChangedCallback, coerceValueCallback)
        {
        }

        /// <summary>
        ///     UI metadata construction
        /// </summary>
        /// <param name="defaultValue">Default value of property</param>
        /// <param name="propertyChangedCallback">Called when the property has been changed</param>
        /// <param name="coerceValueCallback">Called on update of value</param>
        /// <param name="isAnimationProhibited">Should animation be prohibited?</param>
        public UIPropertyMetadata(object? defaultValue,
                                PropertyChangedCallback? propertyChangedCallback,
                                CoerceValueCallback? coerceValueCallback,
                                bool isAnimationProhibited) :
            base(defaultValue, propertyChangedCallback, coerceValueCallback)
        {

        }
    }
}
