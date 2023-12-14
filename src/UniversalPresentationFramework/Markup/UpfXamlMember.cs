using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;
using System.Xaml.Markup;

namespace Wodsoft.UI.Markup
{
    public class UpfXamlMember : XamlMember, IProvideValueTarget
    {
        public UpfXamlMember(XamlType xamlType, DependencyProperty dp, bool isAttachable) : base(dp.Name, xamlType, isAttachable)
        {
            DependencyProperty = dp;
        }

        public DependencyProperty DependencyProperty { get; }

        object IProvideValueTarget.TargetObject => throw new NotSupportedException();

        object IProvideValueTarget.TargetProperty => DependencyProperty;

        protected override bool LookupIsUnknown()
        {
            return false;
        }

        protected override bool LookupIsReadOnly()
        {
            return DependencyProperty.ReadOnly;
        }
    }
}
