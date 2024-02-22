using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Controls;

namespace Wodsoft.UI
{
    [DictionaryKeyProperty("DataTemplateKey")]
    public class DataTemplate : FrameworkTemplate
    {
        private Type? _dataType;
        private TriggerCollection? _triggers;

        [DefaultValue(null)]
        [Ambient]
        public Type? DataType
        {
            get { return _dataType; }
            set
            {
                CheckSealed();
                _dataType = value;
            }
        }

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

        public DataTemplateKey? DataTemplateKey
        {
            get
            {
                return (DataType != null) ? new DataTemplateKey(DataType) : null;
            }
        }

        protected internal override Type TargetTypeInternal => typeof(ContentPresenter);

        protected internal override TriggerCollection? TriggersInternal => _triggers;
    }
}
