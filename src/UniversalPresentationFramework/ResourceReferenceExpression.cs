using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Providers;

namespace Wodsoft.UI
{
    internal class ResourceReferenceExpression : Expression
    {
        private readonly object _resourceKey;
        private LogicalObject? _logicalObject;
        private LogicalObject? _logicalRoot;
        private IThemeProvider? _themeProvider;
        public ResourceReferenceExpression(object resourceKey)
        {
            _resourceKey = resourceKey;
        }

        public override bool CanUpdateSource => false;

        public override bool CanUpdateTarget => true;

        protected override object? GetSourceValue()
        {
            if (_logicalObject == null)
                return Expression.NoValue;
            var value = ResourceHelper.FindResource(_logicalObject, _resourceKey);
            if (value == DependencyProperty.UnsetValue)
                return NoValue;
            return value;
        }

        protected override void OnAttach()
        {
            if (AttachedObject is not LogicalObject)
                AttachedObject!.InheritanceContextChanged += AttachedObject_InheritanceContextChanged;
            Bind();
        }

        protected override void OnDetach()
        {
            if (AttachedObject is not LogicalObject)
                AttachedObject!.InheritanceContextChanged -= AttachedObject_InheritanceContextChanged;
            if (_logicalObject != null)
            {
                _logicalObject.LogicalRootChanged -= LogicalObject_LogicalRootChanged;
                _logicalObject = null;
            }
            if (_logicalRoot != null)
            {
                if (_logicalRoot is FrameworkElement fe)
                    fe.TemplatedParentChanged -= TemplatedParentChanged;
                _logicalRoot = null;
            }
            if (_themeProvider != null)
            {
                _themeProvider.ThemeChanged -= ThemeChanged;
                _themeProvider = null;
            }
        }

        protected override void SetSourceValue(object? value)
        {
            throw new NotSupportedException("Resource reference expression can not set source value.");
        }

        private void Bind()
        {
            LogicalObject? logicalObject = LogicalTreeHelper.FindMentor(AttachedObject!);

            if (logicalObject == null)
                return;

            _logicalObject = logicalObject;
            logicalObject.LogicalRootChanged += LogicalObject_LogicalRootChanged;
            _logicalRoot = _logicalObject.LogicalRoot;
            if (_logicalRoot is FrameworkElement fe)
                fe.TemplatedParentChanged += TemplatedParentChanged;
            _themeProvider = FrameworkProvider.ThemeProvider;
            if (_themeProvider != null)
                _themeProvider.ThemeChanged += ThemeChanged;
        }

        private void ThemeChanged(object? sender, EventArgs e)
        {
            UpdateTarget();
        }

        private void TemplatedParentChanged(object? sender, EventArgs e)
        {
            Rebind();
            UpdateTarget();
        }

        private void Rebind()
        {
            if (_logicalObject != null)
            {
                _logicalObject.LogicalRootChanged -= LogicalObject_LogicalRootChanged;
                _logicalObject = null;
            }
            if (_logicalRoot != null)
            {
                if (_logicalRoot is FrameworkElement fe)
                    fe.TemplatedParentChanged -= TemplatedParentChanged;
                _logicalRoot = null;
            }
            Bind();
        }

        #region Resource Changed

        private void AttachedObject_InheritanceContextChanged(object? sender, EventArgs e)
        {
            Rebind();
            UpdateTarget();
        }

        private void LogicalObject_LogicalRootChanged(object? sender, LogicalRootChangedEventArgs e)
        {
            Rebind();
            UpdateTarget();
        }

        #endregion
    }
}
