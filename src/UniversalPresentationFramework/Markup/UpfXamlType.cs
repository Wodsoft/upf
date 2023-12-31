using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;
using System.Xaml.Schema;

namespace Wodsoft.UI.Markup
{
    internal class UpfXamlType : XamlType
    {
        private static readonly HashSet<Type> _TypeInitialized = new HashSet<Type>();
        private readonly Type _type;
        private readonly Dictionary<string, XamlMember?> _members = new Dictionary<string, XamlMember?>();

        public UpfXamlType(Type underlyingType, XamlSchemaContext schemaContext) : base(underlyingType, schemaContext)
        {
            if (typeof(DependencyObject).IsAssignableFrom(underlyingType))
                InitType(underlyingType);
            _type = underlyingType;
        }

        public static void InitType(Type type)
        {
            if (_TypeInitialized.Contains(type))
                return;
            if (type != typeof(DependencyObject) && type.BaseType != null)
                InitType(type.BaseType);
            System.Runtime.CompilerServices.RuntimeHelpers.RunClassConstructor(type.TypeHandle);
            _TypeInitialized.Add(type);
        }

        protected override XamlMember LookupMember(string name, bool skipReadOnlyCheck)
        {
            if (!_members.TryGetValue(name, out XamlMember? member))
                member = FindMember(name, false);
            if (member == null)
                return base.LookupMember(name, skipReadOnlyCheck);
            return member;
        }

        protected override XamlMember LookupAttachableMember(string name)
        {
            if (!_members.TryGetValue(name, out XamlMember? member))
                member = FindMember(name, true);
            if (member == null)
                return base.LookupAttachableMember(name);
            return member;
        }

        private XamlMember? FindMember(string name, bool isAttachable)
        {
            if (_members.TryGetValue(name, out var member))
                return member;
            var dp = DependencyProperty.FromName(name, _type);
            if (dp == null)
                member = null;
            else
                member = new UpfXamlMember(this, dp, SchemaContext.GetXamlType(dp.PropertyType), isAttachable);
            _members.Add(name, member);
            return member;
        }
    }
}
