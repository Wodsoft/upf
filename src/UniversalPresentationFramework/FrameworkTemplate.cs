using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml;
using System.Xaml.Markup;
using Wodsoft.UI.Controls;

namespace Wodsoft.UI
{
    [ContentProperty("Template")]
    public abstract class FrameworkTemplate : Sealable, INameScope, IQueryAmbient, IHaveResources
    {
        private readonly NameScope _nameScope = new NameScope();
        private ResourceDictionary? _resources;
        private bool _hasTemplate;
        private TemplateContent? _template;
        private FrameworkElementFactory? _visualTree;

        #region Methods

        bool IQueryAmbient.IsAmbientPropertyAvailable(string propertyName)
        {
            // We want to make sure that StaticResource resolution checks the .Resources
            // Ie.  The Ambient search should look at Resources if it is set.
            // Even if it wasn't set from XAML (eg. the Ctor (or derived Ctor) added stuff)
            if (propertyName == "Resources" && _resources == null)
            {
                return false;
            }
            else if (propertyName == "Template" && !_hasTemplate)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Load the content of a template as an instance of an object.  Calling this multiple times
        /// will return separate instances.
        /// </summary>
        public FrameworkElement? LoadContent(FrameworkElement container)
        {
            return LoadContent(container, out _);
        }

        protected internal virtual FrameworkElement? LoadContent(FrameworkElement container, out INameScope? nameScope)
        {
            if (_visualTree != null)
            {
                return _visualTree.Create(out nameScope);
            }
            else if (_template != null)
            {
                return _template.Create(out nameScope);
            }
            else
            {
                nameScope = null;
                return null;
            }
        }

        #endregion

        #region Properties

        [Ambient]
        public ResourceDictionary Resources
        {
            get
            {
                if (_resources == null)
                    _resources = new ResourceDictionary();

                //if (_isSealed)
                //{
                //    _resources.IsReadOnly = true;
                //}

                return _resources;
            }
            set
            {
                CheckSealed();
                _resources = value;
            }
        }

        [Ambient]
        [DefaultValue(null)]
        public TemplateContent? Template
        {
            get
            {
                return _template;
            }
            set
            {
                CheckSealed();

                if (_hasTemplate)
                    throw new InvalidOperationException("Template content can not set twice.");
                if (value == null)
                    return;
                value.OwnerTemplate = this;
                value.Parse();
                _template = value;
                _hasTemplate = true;
            }
        }

        /// <summary>
        ///     Root node of the template
        /// </summary>
        public FrameworkElementFactory? VisualTree
        {
            get
            {
                return _visualTree;
            }
            set
            {
                CheckSealed();
                if (value != null && !typeof(FrameworkElement).IsAssignableFrom(value.Type))
                    throw new ArgumentException("Framework element factory type must be a FrameworkElement.");
                _visualTree = value;
            }
        }

        protected internal abstract Type TargetTypeInternal { get; }

        protected internal virtual TriggerCollection? TriggersInternal => null;

        #endregion

        #region INameScope

        /// <summary>
        /// Registers the name - Context combination
        /// </summary>
        /// <param name="name">Name to register</param>
        /// <param name="scopedElement">Element where name is defined</param>
        public void RegisterName(string name, object scopedElement)
        {
            _nameScope.RegisterName(name, scopedElement);
        }

        /// <summary>
        /// Unregisters the name - element combination
        /// </summary>
        /// <param name="name">Name of the element</param>
        public void UnregisterName(string name)
        {
            _nameScope.UnregisterName(name);
        }

        /// <summary>
        /// Find the element given name
        /// </summary>
        /// <param name="name">Name of the element</param>
        object? INameScope.FindName(string name)
        {
            return _nameScope.FindName(name);
        }

        #endregion INameScope
    }
}
