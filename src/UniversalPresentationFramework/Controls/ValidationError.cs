using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Data;

namespace Wodsoft.UI.Controls
{
    public class ValidationError
    {
        public ValidationError(BindingExpressionBase bindingInError, object? errorContent, Exception? exception)
        {
            BindingInError = bindingInError;
            ErrorContent = errorContent;
            Exception = exception;
        }

        public ValidationError(BindingExpressionBase bindingInError) : this(bindingInError, null, null) { }

        public object? ErrorContent { get; }

        public Exception? Exception { get; }

        public BindingExpressionBase BindingInError { get; }
    }
}
