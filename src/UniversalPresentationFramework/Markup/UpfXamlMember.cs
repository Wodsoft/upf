using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;
using System.Xaml.Markup;
using System.Xaml.Schema;

namespace Wodsoft.UI.Markup
{
    internal class UpfXamlMember : XamlMember, IProvideValueTarget
    {
        public UpfXamlMember(XamlType xamlType, DependencyProperty dp, XamlType memberType, bool isAttachable) : base(dp.Name, xamlType, isAttachable)
        {
            DependencyProperty = dp;
            _memberType = memberType;
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

        protected override XamlType LookupType()
        {
            return _memberType;
        }

        private XamlMemberInvoker? _invoker;
        private readonly XamlType _memberType;

        protected override XamlMemberInvoker LookupInvoker()
        {
            if (_invoker == null)
                _invoker = new UpfXamlMemberInvoker(this);
            return _invoker;
        }
    }
}
