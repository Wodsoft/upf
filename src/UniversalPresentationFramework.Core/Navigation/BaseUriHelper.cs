using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO.Packaging;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;

namespace Wodsoft.UI.Navigation
{
    public static class BaseUriHelper
    {
        private const string _SOOBASE = "SiteOfOrigin://";
        private static readonly Uri _SiteOfOriginBaseUri = PackUriHelper.Create(new Uri(_SOOBASE));
        private const string _APPBASE = "application://";
        private static readonly Uri _PackAppBaseUri = PackUriHelper.Create(new Uri(_APPBASE));

        private static Uri _BaseUri;

        // Cached result of calling
        // PackUriHelper.GetPackageUri(BaseUriHelper.PackAppBaseUri).GetComponents(
        // UriComponents.AbsoluteUri,
        // UriFormat.UriEscaped);
        private const string _PackageApplicationBaseUriEscaped = "application:///";
        private const string _PackageSiteOfOriginBaseUriEscaped = "siteoforigin:///";

        static BaseUriHelper()
        {
            _BaseUri = _PackAppBaseUri;
            //// Add an instance of the ResourceContainer to PreloadedPackages so that PackWebRequestFactory can find it
            //// and mark it as thread-safe so PackWebResponse won't protect returned streams with a synchronizing wrapper
            //PreloadedPackages.AddPackage(PackUriHelper.GetPackageUri(SiteOfOriginBaseUri), new SiteOfOriginContainer(), true);
        }

        #region public property and method

        /// <summary>
        ///     The DependencyProperty for BaseUri of current Element.
        ///
        ///     Flags: None
        ///     Default Value: null.
        /// </summary>
        public static readonly DependencyProperty BaseUriProperty =
                    DependencyProperty.RegisterAttached(
                                "BaseUri",
                                typeof(Uri),
                                typeof(BaseUriHelper),
                                new PropertyMetadata(null));


        /// <summary>
        /// Get BaseUri for a dependency object inside a tree.
        ///
        /// </summary>
        /// <param name="element">Dependency Object</param>
        /// <returns>BaseUri for the element</returns>
        /// <remarks>
        ///     Callers must have FileIOPermission(FileIOPermissionAccess.PathDiscovery) for the given Uri to call this API.
        /// </remarks>
        public static Uri GetBaseUri(DependencyObject element)
        {
            Uri? baseUri = GetBaseUriCore(element);

            //
            // Manipulate BaseUri after tree searching is done.
            //

            if (baseUri == null)
            {
                // If no BaseUri information is found from the current tree,
                // just take the Application's BaseUri.
                // Application's BaseUri must be an absolute Uri.

                baseUri = BaseUriHelper.BaseUri;
            }
            else
            {
                if (baseUri.IsAbsoluteUri == false)
                {
                    // Most likely the BaseUriDP in element or IUriContext.BaseUri
                    // is set to a relative Uri programmatically in user's code.
                    // For this case, we should resolve this relative Uri to PackAppBase
                    // to generate an absolute Uri.

                    //
                    // BamlRecordReader now always sets absolute Uri for UriContext
                    // element when the tree is generated from baml/xaml stream, this
                    // code path would not run for parser-loaded tree.
                    //

                    baseUri = new Uri(BaseUriHelper.BaseUri, baseUri);
                }
            }

            return baseUri;
        }


        #endregion public property and method

        #region internal properties and methods

        static internal Uri SiteOfOriginBaseUri
        {
            get
            {
                return _SiteOfOriginBaseUri;
            }
        }

        static internal Uri PackAppBaseUri
        {
            get
            {
                return _PackAppBaseUri;
            }
        }

        /// <summary>
        /// Checks whether the input uri is in the "pack://application:,,," form
        /// </summary>
        internal static bool IsPackApplicationUri(Uri uri)
        {
            return
                // Is the "outer" URI absolute?
                uri.IsAbsoluteUri &&

                // Does the "outer" URI have the pack: scheme?
                string.Equals(uri.Scheme, System.IO.Packaging.PackUriHelper.UriSchemePack, StringComparison.OrdinalIgnoreCase) &&

                 // Does the "inner" URI have the application: scheme
                 string.Equals(
                    PackUriHelper.GetPackageUri(uri).GetComponents(UriComponents.AbsoluteUri, UriFormat.UriEscaped),
                    _PackageApplicationBaseUriEscaped, StringComparison.OrdinalIgnoreCase);
        }

        // The method accepts a relative or absolute Uri and returns the appropriate assembly.
        //
        // For absolute Uri, it accepts only "pack://application:,,,/...", throw exception for
        // any other absolute Uri.
        //
        // If the first segment of that Uri contains ";component", returns the assembly whose
        // assembly name matches the text string in the first segment. otherwise, this method
        // would return EntryAssembly in the AppDomain.
        //
        internal static void GetAssemblyAndPartNameFromPackAppUri(Uri uri, out Assembly? assembly, out string partName)
        {
            // The input Uri is assumed to be a valid absolute pack application Uri.
            // The caller should guarantee that.
            // Perform a sanity check to make sure the assumption stays.

            // Generate a relative Uri which gets rid of the pack://application:,,, authority part.
            Uri partUri = new Uri(uri.AbsolutePath, UriKind.Relative);

            string assemblyName;
            string assemblyVersion;
            string assemblyKey;

            GetAssemblyNameAndPart(partUri, out partName, out assemblyName, out assemblyVersion, out assemblyKey);

            if (String.IsNullOrEmpty(assemblyName))
            {
                // The uri doesn't contain ";component". it should map to the enty application assembly.
                assembly = ResourceAssembly;
            }
            else
            {
                assembly = GetLoadedAssembly(assemblyName, assemblyVersion, assemblyKey);
            }
        }


        //
        //
        internal static Assembly GetLoadedAssembly(string assemblyName, string assemblyVersion, string assemblyKey)
        {
            Assembly? assembly;
            AssemblyName asmName = new AssemblyName(assemblyName);

            // We always use the primary assembly (culture neutral) for resource manager.
            // if the required resource lives in satellite assembly, ResourceManager can find
            // the right satellite assembly later.
            asmName.CultureInfo = new CultureInfo(string.Empty);

            if (!string.IsNullOrEmpty(assemblyVersion))
            {
                asmName.Version = new Version(assemblyVersion);
            }

            byte[]? keyToken = ParseAssemblyKey(assemblyKey);

            if (keyToken != null)
            {
                asmName.SetPublicKeyToken(keyToken);
            }

            assembly = GetLoadedAssembly(asmName);

            if (assembly == null)
            {
                // The assembly is not yet loaded to the AppDomain, try to load it with information specified in resource Uri.
                assembly = Assembly.Load(asmName);
            }

            return assembly;
        }

        internal static Assembly? GetLoadedAssembly(AssemblyName assemblyName)
        {
            Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();

            Version? reqVersion = assemblyName.Version;
            CultureInfo? reqCulture = assemblyName.CultureInfo;
            byte[]? reqKeyToken = assemblyName.GetPublicKeyToken();

            for (int i = assemblies.Length - 1; i >= 0; i--)
            {
                AssemblyName curAsmName = assemblies[i].GetName();
                Version? curVersion = curAsmName.Version;
                CultureInfo? curCulture = curAsmName.CultureInfo;
                byte[]? curKeyToken = curAsmName.GetPublicKeyToken();

                if ((string.Compare(curAsmName.Name, assemblyName.Name, true, CultureInfo.InvariantCulture) == 0) &&
                     (reqVersion == null || reqVersion.Equals(curVersion)) &&
                     (reqCulture == null || reqCulture.Equals(curCulture)) &&
                     (reqKeyToken == null || (curKeyToken != null && reqKeyToken.SequenceEqual(curKeyToken))))
                {
                    return assemblies[i];
                }
            }
            return null;
        }

        //
        // Return assembly Name, Version, Key and package Part from a relative Uri.
        //
        internal static void GetAssemblyNameAndPart(Uri uri, out string partName, out string assemblyName, out string assemblyVersion, out string assemblyKey)
        {
            string original = uri.ToString(); // only relative Uri here (enforced by Package)

            // Start and end points for the first segment in the Uri.
            int start = 0;
            int end;

            if (original[0] == '/')
            {
                start = 1;
            }

            partName = original.Substring(start);

            assemblyName = string.Empty;
            assemblyVersion = string.Empty;
            assemblyKey = string.Empty;

            end = original.IndexOf('/', start);

            string firstSegment = String.Empty;
            bool fHasComponent = false;

            if (end > 0)
            {
                // get the first section
                firstSegment = original.Substring(start, end - start);

                // The resource comes from dll
                if (firstSegment.EndsWith(_COMPONENT, StringComparison.OrdinalIgnoreCase))
                {
                    partName = original.Substring(end + 1);
                    fHasComponent = true;
                }
            }

            if (fHasComponent)
            {
                string[] assemblyInfo = firstSegment.Split(_COMPONENT_DELIMITER);

                int count = assemblyInfo.Length;

                if ((count > 4) || (count < 2))
                {
                    throw new UriFormatException("Wrong first segment");
                }

                //
                // if the uri contains escaping character,
                // Convert it back to normal unicode string
                // so that the string as assembly name can be
                // recognized by Assembly.Load later.
                //
                assemblyName = Uri.UnescapeDataString(assemblyInfo[0]);

                for (int i = 1; i < count - 1; i++)
                {
                    if (assemblyInfo[i].StartsWith(_VERSION, StringComparison.OrdinalIgnoreCase))
                    {
                        if (string.IsNullOrEmpty(assemblyVersion))
                        {
                            assemblyVersion = assemblyInfo[i].Substring(1);  // Get rid of the leading "v"
                        }
                        else
                        {
                            throw new UriFormatException("Wrong first segment");
                        }
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(assemblyKey))
                        {
                            assemblyKey = assemblyInfo[i];
                        }
                        else
                        {
                            throw new UriFormatException("Wrong first segment");
                        }
                    }
                } // end of for loop

            } // end of if fHasComponent
        }

        static internal bool IsComponentEntryAssembly(string component)
        {
            if (component.EndsWith(_COMPONENT, StringComparison.OrdinalIgnoreCase))
            {
                string[] assemblyInfo = component.Split(_COMPONENT_DELIMITER);
                // Check whether the assembly name is the same as the EntryAssembly.
                int count = assemblyInfo.Length;
                if ((count >= 2) && (count <= 4))
                {
                    string assemblyName = Uri.UnescapeDataString(assemblyInfo[0]);

                    Assembly? assembly = ResourceAssembly;

                    if (assembly != null)
                    {
                        return (string.Equals(assembly.GetName().Name, assemblyName, StringComparison.OrdinalIgnoreCase));
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }

        static internal Uri GetResolvedUri(Uri baseUri, Uri orgUri)
        {
            return new Uri(baseUri, orgUri);
        }

        static internal Uri MakeRelativeToSiteOfOriginIfPossible(Uri sUri)
        {
            if (Uri.Compare(sUri, SiteOfOriginBaseUri, UriComponents.Scheme, UriFormat.UriEscaped, StringComparison.OrdinalIgnoreCase) == 0)
            {
                Uri packageUri = PackUriHelper.GetPackageUri(sUri);
                if (string.Equals(packageUri.GetComponents(UriComponents.AbsoluteUri, UriFormat.UriEscaped), _PackageSiteOfOriginBaseUriEscaped, StringComparison.OrdinalIgnoreCase))
                {
                    return (new Uri(sUri.GetComponents(UriComponents.SchemeAndServer, UriFormat.UriEscaped))).MakeRelativeUri(sUri);
                }
            }

            return sUri;
        }

        //static internal Uri ConvertPackUriToAbsoluteExternallyVisibleUri(Uri packUri)
        //{
        //    Uri relative = MakeRelativeToSiteOfOriginIfPossible(packUri);

        //    if (!relative.IsAbsoluteUri)
        //    {
        //        return new Uri(SiteOfOriginContainer.SiteOfOrigin, relative);
        //    }
        //    else
        //    {
        //        throw new InvalidOperationException($"Cannot navigate to application resource \"{packUri}\" in WebBrowser.");
        //    }
        //}

        // If a Uri is constructed with a legacy path such as c:\foo\bar then the Uri
        // object will not correctly resolve relative Uris in some cases.  This method
        // detects and fixes this by constructing a new Uri with an original string
        // that contains the scheme file://.
        static internal Uri FixFileUri(Uri uri)
        {
            if (uri.IsAbsoluteUri && string.Equals(uri.Scheme, Uri.UriSchemeFile, StringComparison.OrdinalIgnoreCase) &&
                string.Compare(uri.OriginalString, 0, Uri.UriSchemeFile, 0, Uri.UriSchemeFile.Length, StringComparison.OrdinalIgnoreCase) != 0)
            {
                return new Uri(uri.AbsoluteUri);
            }
            return uri;
        }

        static internal Uri BaseUri
        {
            get
            {
                return _BaseUri;
            }
            set
            {
                // This setter should only be called from Framework through
                // BindUriHelper.set_BaseUri.
                _BaseUri = value;
            }
        }

        static internal Assembly? ResourceAssembly
        {
            get
            {
                if (_ResourceAssembly == null)
                {
                    _ResourceAssembly = Assembly.GetEntryAssembly();
                }
                return _ResourceAssembly;
            }
            set
            {
                // This should only be called from Framework through Application.ResourceAssembly setter.
                _ResourceAssembly = value;
            }
        }

        // If the Uri provided is a pack Uri calling out an assembly and the assembly name matches that from assemblyInfo
        // this method will append the version taken from assemblyInfo, provided the Uri does not already have a version,
        // if the Uri provided the public Key token we must also verify that it matches the one in assemblyInfo.
        // We only add the version if the Uri is missing both the version and the key, otherwise returns null.
        // If the Uri is not a pack Uri, or we can't extract the information we need this method returns null.
        static internal Uri? AppendAssemblyVersion(Uri uri, Assembly assemblyInfo)
        {
            Uri? source = null;
            Uri? baseUri = null;

            // assemblyInfo.GetName does not work in PartialTrust, so do this instead.
            AssemblyName currAssemblyName = new AssemblyName(assemblyInfo.FullName!);
            string? version = currAssemblyName.Version?.ToString();

            if (uri != null && !string.IsNullOrEmpty(version))
            {
                if (uri.IsAbsoluteUri)
                {
                    if (IsPackApplicationUri(uri))
                    {
                        // Extract the relative Uri and keep the base Uri so we can
                        // put them back together after we append the version.
                        source = new Uri(uri.AbsolutePath, UriKind.Relative);
                        baseUri = new Uri(uri.GetLeftPart(UriPartial.Authority), UriKind.Absolute);
                    }
                }
                else
                {
                    source = uri;
                }


                if (source != null)
                {
                    string appendedUri;
                    string assemblyName;
                    string assemblyVersion;
                    string assemblyKey;
                    string partName;

                    BaseUriHelper.GetAssemblyNameAndPart(source, out partName, out assemblyName, out assemblyVersion, out assemblyKey);

                    bool assemblyKeyProvided = !string.IsNullOrEmpty(assemblyKey);

                    // Make sure we:
                    //      1) Successfully extracted the assemblyName from the Uri.
                    //      2) No assembly version was already provided in the Uri.
                    //      3) The assembly short name matches the name in assemblyInfo.
                    //      4) If a public key token was provided in the Uri, verify
                    //          that it matches the token in asssemblyInfo.
                    if (!string.IsNullOrEmpty(assemblyName) && string.IsNullOrEmpty(assemblyVersion) &&
                        assemblyName.Equals(currAssemblyName.Name, StringComparison.Ordinal) &&
                        (!assemblyKeyProvided || AssemblyMatchesKeyString(currAssemblyName, assemblyKey)))
                    {
                        StringBuilder uriStringBuilder = new StringBuilder();

                        uriStringBuilder.Append('/');
                        uriStringBuilder.Append(assemblyName);
                        uriStringBuilder.Append(_COMPONENT_DELIMITER);
                        uriStringBuilder.Append(_VERSION);
                        uriStringBuilder.Append(version);
                        if (assemblyKeyProvided)
                        {
                            uriStringBuilder.Append(_COMPONENT_DELIMITER);
                            uriStringBuilder.Append(assemblyKey);
                        }
                        uriStringBuilder.Append(_COMPONENT);
                        uriStringBuilder.Append('/');
                        uriStringBuilder.Append(partName);

                        appendedUri = uriStringBuilder.ToString();

                        if (baseUri != null)
                        {
                            return new Uri(baseUri, appendedUri);
                        }

                        return new Uri(appendedUri, UriKind.Relative);
                    }
                }

            }

            return null;
        }

        #endregion internal properties and methods

        #region private methods

        /// <summary>
        /// Get BaseUri for a dependency object inside a tree.
        ///
        /// </summary>
        /// <param name="element">Dependency Object</param>
        /// <returns>BaseUri for the element</returns>
        /// <remarks>
        ///     Callers must have FileIOPermission(FileIOPermissionAccess.PathDiscovery) for the given Uri to call this API.
        /// </remarks>
        internal static Uri? GetBaseUriCore(DependencyObject? element)
        {
            Uri? baseUri = null;
            DependencyObject? doCurrent;

            if (element == null)
            {
                throw new ArgumentNullException("element");
            }

            //
            // Search the tree to find the closest parent which implements
            // IUriContext or have set value for BaseUri property.
            //
            doCurrent = element;

            while (doCurrent != null)
            {
                // Try to get BaseUri property value from current node.
                baseUri = doCurrent.GetValue(BaseUriProperty) as Uri;

                if (baseUri != null)
                {
                    // Got the right node which is the closest to original element.
                    // Stop searching here.
                    break;
                }

                IUriContext? uriContext = doCurrent as IUriContext;

                if (uriContext != null)
                {
                    // If the element implements IUriContext, and if the BaseUri
                    // is not null, just take the BaseUri from there.
                    // and stop the search loop.
                    baseUri = uriContext.BaseUri;

                    if (baseUri != null)
                        break;
                }

                if (doCurrent is LogicalObject logicalObject)
                    doCurrent = logicalObject.LogicalParent;
                else
                    doCurrent = doCurrent.InheritanceContext;
            }

            return baseUri;
        }

        private static bool AssemblyMatchesKeyString(AssemblyName asmName, string assemblyKey)
        {
            byte[]? parsedKeyToken = ParseAssemblyKey(assemblyKey);
            byte[]? assemblyKeyToken = asmName.GetPublicKeyToken();
            if (assemblyKeyToken == null && parsedKeyToken == null)
                return true;
            if (assemblyKeyToken == null || parsedKeyToken == null)
                return false;
            return parsedKeyToken.SequenceEqual(assemblyKeyToken);
        }

        private static byte[]? ParseAssemblyKey(string assemblyKey)
        {
            if (!string.IsNullOrEmpty(assemblyKey))
            {
                int byteCount = assemblyKey.Length / 2;
                byte[] keyToken = new byte[byteCount];
                for (int i = 0; i < byteCount; i++)
                {
                    keyToken[i] = byte.Parse(assemblyKey.AsSpan(i * 2, 2), NumberStyles.HexNumber, CultureInfo.InvariantCulture);
                }

                return keyToken;
            }

            return null;
        }

        #endregion

        private const string _COMPONENT = ";component";
        private const string _VERSION = "v";
        private const char _COMPONENT_DELIMITER = ';';

        private static Assembly? _ResourceAssembly;
    }
}
