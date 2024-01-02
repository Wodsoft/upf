using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;

namespace Wodsoft.UI
{
    public class FrameworkContentElement : LogicalObject
    {
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


        private ResourceDictionary? _resources;
        [Ambient]
        public ResourceDictionary? Resources
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
    }
}
