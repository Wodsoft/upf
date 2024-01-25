using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI
{
    public static class LogicalTreeHelper
    {
        public static LogicalObject? GetParent(LogicalObject current) => current.LogicalParent;

        public static IEnumerable<LogicalObject>? GetChildren(LogicalObject current) => current.LogicalChildren;

        public static LogicalObject GetRoot(LogicalObject current) => current.LogicalRoot ?? current;

        public static LogicalObject? FindMentor(DependencyObject d)
        {
            LogicalObject? logicalObject = d as LogicalObject;
            if (logicalObject == null)
            {
                var obj = d;
                while (obj != null)
                {
                    if (obj is LogicalObject)
                    {
                        logicalObject = (LogicalObject)obj;
                        break;
                    }
                    obj = obj.InheritanceContext;
                }
            }
            return logicalObject;
        }
    }
}
