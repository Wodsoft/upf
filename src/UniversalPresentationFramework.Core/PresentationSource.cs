using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Input;
using Wodsoft.UI.Media;
using Wodsoft.UI.Threading;

namespace Wodsoft.UI
{
    public abstract class PresentationSource : DispatcherObject
    {
        private static List<PresentationSource> _Sources = new List<PresentationSource>();

        #region Methods

        public static PresentationSource? FromVisual(Visual visual)
        {
            var rootVisual = InputElement.GetRootVisual(visual);
            if (rootVisual == null)
                return null;
            return (PresentationSource?)rootVisual.GetValue(_RootSourceProperty);
        }

        public static PresentationSource? FromDependencyObject(DependencyObject dependencyObject)
        {
            var rootVisual = InputElement.GetRootVisual(dependencyObject);
            if (rootVisual == null)
                return null;
            return (PresentationSource?)rootVisual.GetValue(_RootSourceProperty);
        }

        #endregion

        #region Properties

        private static readonly DependencyProperty _RootSourceProperty
            = DependencyProperty.RegisterAttached("RootSource", typeof(PresentationSource), typeof(PresentationSource),
                                          new PropertyMetadata(null));

        public abstract Visual RootVisual { get; }

        public abstract bool IsDisposed { get; }

        public static IEnumerable<PresentationSource> CurrentSources
        {
            get
            {
                List<PresentationSource> sources;
                lock (_Sources)
                    sources = new List<PresentationSource>(_Sources);
                return sources;
            }
        }

        #endregion

        #region Methods

        protected void AddRootSource(Visual root)
        {
            root.SetValue(_RootSourceProperty, this);
        }

        protected void RemoveRootSource(Visual root)
        {
            root.ClearValue(_RootSourceProperty);
        }

        protected void AddSource()
        {
            lock (_Sources)
                _Sources.Add(this);
        }

        protected void RemoveSource()
        {
            lock (_Sources)
                _Sources.Remove(this);
        }

        #endregion

        #region Events

        public event EventHandler? ContentRendered;

        #endregion
    }
}
