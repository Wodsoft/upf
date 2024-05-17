using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Media;

namespace Wodsoft.UI
{
    public class FrameworkContentElement : ContentElement
    {
        #region NameScope

        public static readonly DependencyProperty NameProperty =
            DependencyProperty.Register(
                        "Name",
                        typeof(string),
                        typeof(FrameworkContentElement),
                        new FrameworkPropertyMetadata(
                            string.Empty,                           // defaultValue
                            FrameworkPropertyMetadataOptions.None,  // flags
                            null,                                   // propertyChangedCallback
                            null,                                   // coerceValueCallback
                            true),                                  // isAnimationProhibited
                        new ValidateValueCallback(NameValidationCallback));
        private static bool NameValidationCallback(object? candidateName)
        {
            string? name = candidateName as string;
            if (name != null)
            {
                // Non-null string, ask the XAML validation code for blessing.
                return NameScope.IsValidIdentifierName(name);
            }
            else if (candidateName == null)
            {
                // Null string is allowed
                return true;
            }
            else
            {
                // candiateName is not a string object.
                return false;
            }
        }
        public string? Name { get { return (string?)GetValue(NameProperty); } set { SetValue(NameProperty, value); } }

        /// <summary>
        /// Find the object with given name in the
        /// NameScope that the current element belongs to.
        /// </summary>
        /// <param name="name">string name to index</param>
        /// <returns>context if found, else null</returns>
        public object? FindName(string name)
        {
            return FindScope()?.FindName(name);
        }

        private INameScope? FindScope()
        {
            LogicalObject? element = this;
            while (element != null)
            {
                INameScope? nameScope = NameScope.NameScopeFromObject(element);
                if (nameScope != null)
                    return nameScope;
                element = LogicalTreeHelper.GetParent(element);
            }
            return null;
        }

        #endregion

        #region Resource

        private ResourceDictionary? _resources;
        [Ambient]
        public ResourceDictionary Resources
        {
            get
            {
                if (_resources == null)
                    _resources = new ResourceDictionary();
                return _resources;
            }
            set
            {
                _resources = value;
            }
        }

        #endregion
    }
}
