using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;

namespace Wodsoft.UI.Markup
{
    internal class UpfXamlSchemaContext : XamlSchemaContext
    {
        private readonly Dictionary<Type, UpfXamlType> _xamlTypes = new Dictionary<Type, UpfXamlType>();

        public override XamlType GetXamlType(Type type)
        {
            if (_xamlTypes.TryGetValue(type, out var xamlType))
                return xamlType;
            xamlType = new UpfXamlType(type, this);
            _xamlTypes.Add(type, xamlType);
            return xamlType;
        }
    }
}
