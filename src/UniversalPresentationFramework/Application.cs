using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Markup;
using Wodsoft.UI.Navigation;
using Wodsoft.UI.Providers;

namespace Wodsoft.UI
{
    public class Application
    {
        private bool _isRunning;
        public static Application? Current { get; private set; }

        [Ambient]
        public ResourceDictionary? Resources { get; set; }

        public bool IsRunning => _isRunning;

        #region Providers

        private IRendererProvider? _rendererProvider;
        public IRendererProvider? RendererProvider
        {
            get => _rendererProvider;
            set
            {
                if (_isRunning)
                    throw new InvalidOperationException("Could not set renderer provider while application is running.");
                _rendererProvider = value;
            }
        }

        private IWindowProvider? _windowProvider;
        public IWindowProvider? WindowProvider
        {
            get => _windowProvider;
            set
            {
                if (_isRunning)
                    throw new InvalidOperationException("Could not set window provider while application is running.");
                _windowProvider = value;
            }
        }

        private ILifecycleProvider? _lifecycleProvider;
        public ILifecycleProvider? LifecycleProvider
        {
            get => _lifecycleProvider;
            set
            {
                if (_isRunning)
                    throw new InvalidOperationException("Could not set lifecycle provider while application is running.");
                _lifecycleProvider = value;
            }
        }

        private IThemeProvider? _themeProvider;
        public IThemeProvider? ThemeProvider
        {
            get => _themeProvider;
            set
            {
                if (_isRunning)
                    throw new InvalidOperationException("Could not set theme provider while application is running.");
                _themeProvider = value;
            }
        }

        private IResourceProvider? _resourceProvider;
        public IResourceProvider? ResourceProvider
        {
            get => _resourceProvider;
            set
            {
                if (_isRunning)
                    throw new InvalidOperationException("Could not set resource provider while application is running.");
                _resourceProvider = value;
            }
        }

        private IParameterProvider? _parameterProvider;
        public IParameterProvider? ParameterProvider
        {
            get => _parameterProvider;
            set
            {
                if (_isRunning)
                    throw new InvalidOperationException("Could not set parameter provider while application is running.");
                _parameterProvider = value;
            }
        }

        private IInputProvider? _inputProvider;
        public IInputProvider? InputProvider
        {
            get => _inputProvider;
            set
            {
                if (_isRunning)
                    throw new InvalidOperationException("Could not set input provider while application is running.");
                _inputProvider = value;
            }
        }

        private IClockProvider? _clockProvider;
        public IClockProvider? ClockProvider
        {
            get => _clockProvider;
            set
            {
                if (_isRunning)
                    throw new InvalidOperationException("Could not set clock provider while application is running.");
                _clockProvider = value;
            }
        }

        public Uri? StartupUri { get; set; }

        #endregion

        public int Run()
        {
            return Run(null);
        }

        public int Run(Window? window)
        {
            if (_windowProvider == null)
                throw new InvalidOperationException("Can not start application because window provider is not set.");
            if (_lifecycleProvider == null)
                throw new InvalidOperationException("Can not start application because lifecycle provider is not set.");
            if (_rendererProvider == null)
                throw new InvalidOperationException("Can not start application because renderer provider is not set.");
            if (_isRunning)
                throw new InvalidOperationException("Application running already.");
            _isRunning = true;
            Current = this;
            FrameworkProvider.ResourceProvider = _resourceProvider;
            FrameworkProvider.ThemeProvider = _themeProvider;
            FrameworkProvider.ParameterProvider = _parameterProvider;
            _lifecycleProvider.Start();
            FrameworkCoreProvider.RendererProvider = _rendererProvider;
            FrameworkCoreProvider.ClockProvider = _clockProvider;
            FrameworkCoreProvider.InputProvider = _inputProvider;
            if (window == null)
            {
                if (StartupUri == null)
                    throw new InvalidOperationException("StartupUri is null.");
                var obj = LoadComponent(StartupUri);
                if (obj == null)
                    throw new InvalidOperationException("Resource at StartupUri has no content.");
                window = obj as Window;
                if (window == null)
                    throw new InvalidOperationException("Resource at StartupUri must be a Window.");
            }
            window.Show();
            _lifecycleProvider.WaitForEnd();
            return 0;
        }

        public static object? LoadComponent(Uri resourceLocator)
        {
            if (resourceLocator == null)
                throw new ArgumentNullException("resourceLocator");
            if (resourceLocator.OriginalString == null)
                throw new ArgumentException("OriginalString must not be null.", "resourceLocator");
            if (resourceLocator.IsAbsoluteUri == true)
                throw new ArgumentException("Absolute uri is not allowed.", "resourceLocator");

            Uri currentUri = new Uri(BaseUriHelper.PackAppBaseUri, resourceLocator);

            Uri packAppUri = BaseUriHelper.PackAppBaseUri;
            Uri resolvedUri = GetResolvedUri(packAppUri, resourceLocator);

            Uri packageUri = PackUriHelper.GetPackageUri(resolvedUri);
            Uri? partUri = PackUriHelper.GetPartUri(resolvedUri);

            if (partUri == null)
                throw new ArgumentException("Resolve component path failed.", "resourceLocator");

            BaseUriHelper.GetAssemblyNameAndPart(partUri, out var partName, out var assemblyName, out var assemblyVersion, out var assemblyKey);

            Assembly assembly;
            if (string.IsNullOrEmpty(assemblyName))
            {
                if (Current == null)
                    throw new InvalidOperationException("Application is not running.");
                assembly = Current.GetType().Assembly;
            }
            else
            {
                assembly = BaseUriHelper.GetLoadedAssembly(assemblyName, assemblyVersion, assemblyKey);
            }
            var loadResourcesFunction = BamlResource.GetLoadResourcesFunction(assembly);
            if (loadResourcesFunction == null)
                throw new InvalidOperationException("Assembly doesn't contains baml resources.");
            return loadResourcesFunction(partName);
        }


        public static void LoadComponent(object component, Uri resourceLocator)
        {
            LoadComponent(component, resourceLocator, false);
        }

        public static void LoadComponent(object component, Uri resourceLocator, Func<BamlResource> resourceProvider)
        {
            if (!Debugger.IsAttached && resourceProvider != null)
            {
                XamlReader.Load(resourceProvider().GetReader(), component);
                return;
            }
            LoadComponent(component, resourceLocator, true);
        }

        //private static object? LoadComponent()

        private static void LoadComponent(object component, Uri resourceLocator, bool skipBaml)
        {
            if (component == null)
                throw new ArgumentNullException("component");
            if (resourceLocator == null)
                throw new ArgumentNullException("resourceLocator");
            if (resourceLocator.OriginalString == null)
                throw new ArgumentException("OriginalString must not be null.", "resourceLocator");
            if (resourceLocator.IsAbsoluteUri == true)
                throw new ArgumentException("Absolute uri is not allowed.", "resourceLocator");

            Uri currentUri = new Uri(BaseUriHelper.PackAppBaseUri, resourceLocator);

            Uri packAppUri = BaseUriHelper.PackAppBaseUri;
            Uri resolvedUri = GetResolvedUri(packAppUri, resourceLocator);

            Uri packageUri = PackUriHelper.GetPackageUri(resolvedUri);
            Uri? partUri = PackUriHelper.GetPartUri(resolvedUri);

            if (partUri == null)
                throw new ArgumentException("Resolve component path failed.", "resourceLocator");

            BaseUriHelper.GetAssemblyNameAndPart(partUri, out var partName, out var assemblyName, out var assemblyVersion, out var assemblyKey);
            //if (skipBaml && Debugger.IsAttached)
            //{

            //}
            //else
            //{
            Assembly assembly;
            if (assemblyName == null)
            {
                if (Current == null)
                    throw new InvalidOperationException("Application is not running.");
                assembly = Current.GetType().Assembly;
            }
            else
            {
                assembly = BaseUriHelper.GetLoadedAssembly(assemblyName, assemblyVersion, assemblyKey);
            }
            var loadComponentFunction = BamlResource.GetLoadComponentFunction(assembly);
            if (loadComponentFunction == null)
                throw new InvalidOperationException("Assembly doesn't contains baml resources.");
            loadComponentFunction(partName, component);
            //}
        }

        static internal Uri GetResolvedUri(Uri baseUri, Uri orgUri)
        {
            Uri newUri;
            if (orgUri.IsAbsoluteUri == false)
            {
                // if the orgUri is an absolute Uri, don't need to resolve it again.

                Uri baseuri = (baseUri == null) ? BaseUriHelper.BaseUri : baseUri;
                newUri = new Uri(baseuri, orgUri);
            }
            else
            {
                newUri = BaseUriHelper.FixFileUri(orgUri);
            }
            return newUri;
        }
    }
}
