using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
    public sealed class TemplatePartAttribute : Attribute
    {
        /// <summary>
        ///     Default constructor
        /// </summary>
        public TemplatePartAttribute()
        {
        }

        /// <summary>
        ///     Part name used by the class to indentify required element in the style
        /// </summary>
        public string? Name
        {
            get { return _name; }
            set { _name = value; }
        }

        /// <summary>
        ///     Type of the element that should be used as a part with name specified in TemplatePartAttribute.Name
        /// </summary>
        public Type? Type
        {
            get { return _type; }
            set { _type = value; }
        }

        private string? _name;
        private Type? _type;
    }
}
