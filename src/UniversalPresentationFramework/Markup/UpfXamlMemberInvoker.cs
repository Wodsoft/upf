using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Schema;

namespace Wodsoft.UI.Markup
{
    internal class UpfXamlMemberInvoker : XamlMemberInvoker
    {
        private readonly UpfXamlMember _member;

        public UpfXamlMemberInvoker(UpfXamlMember member) : base(member)
        {
            _member = member;
        }

        public override object? GetValue(object instance)
        {
            if (instance is DependencyObject d)
            {
                if (_member.DependencyProperty != null)
                    return d.GetValue(_member.DependencyProperty);
            }
            return base.GetValue(instance);
        }

        public override void SetValue(object instance, object? value)
        {
            if (instance is DependencyObject d)
            {
                if (_member.DependencyProperty != null)
                {
                    d.SetValue(_member.DependencyProperty, value);
                    return;
                }
            }
            base.SetValue(instance, value);
        }
    }
}
