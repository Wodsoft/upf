using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;

namespace Wodsoft.UI.Controls
{
    [DictionaryKeyProperty("TargetType")]
    public class ControlTemplate : FrameworkTemplate
    {
        private Type? _targetType;
        private TriggerCollection? _triggers;

        public ControlTemplate() { }

        /// <summary>
        ///     ControlTemplate Constructor
        /// </summary>
        public ControlTemplate(Type targetType)
        {
            ValidateTargetType(targetType);
            _targetType = targetType;
        }

        private void ValidateTargetType(Type targetType)
        {
            if (!typeof(Control).IsAssignableFrom(targetType))
                throw new InvalidOperationException("Invalid control template target type.");
        }

        #region Properties

        [Ambient]
        public Type TargetType
        {
            get { return _targetType ?? typeof(Control); }
            set
            {
                if (value == null)
                    throw new ArgumentNullException("value");
                ValidateTargetType(value);
                CheckSealed();
                _targetType = value;
            }
        }

        protected internal override Type TargetTypeInternal => TargetType;


        [DependsOn("VisualTree")]
        [DependsOn("Template")]
        public TriggerCollection Triggers
        {
            get
            {
                if (_triggers == null)
                {
                    _triggers = new TriggerCollection();
                    if (IsSealed)
                        _triggers.Seal();
                }
                return _triggers;
            }
        }

        protected internal override TriggerCollection? TriggersInternal => _triggers;

        #endregion

    }
}
