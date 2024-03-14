using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Media
{
    public class FontFamily
    {
        /// <summary>
        /// Constructs FontFamily from a string.
        /// </summary>
        /// <param name="familyName">Specifies one or more comma-separated family names, each
        /// of which may be either a regular family name string (e.g., "Arial") or a URI
        /// (e.g., "file:///c:/windows/fonts/#Arial").</param>
        public FontFamily(string familyName) : this(null, familyName)
        { }

        /// <summary>
        /// Constructs FontFamily from a string and an optional base URI.
        /// </summary>
        /// <param name="baseUri">Specifies the base URI used to resolve family names, typically 
        /// the URI of the document or element that refers to the font family. Can be null.</param>
        /// <param name="familyName">Specifies one or more comma-separated family names, each
        /// of which may be either a regular family name string (e.g., "Arial") or a URI
        /// (e.g., "file:///c:/windows/fonts/#Arial").</param>
        public FontFamily(Uri? baseUri, string familyName)
        {
            if (familyName == null)
                throw new ArgumentNullException("familyName");

            if (baseUri != null && !baseUri.IsAbsoluteUri)
                throw new ArgumentException("Uri must be absolute.", "baseUri");
        }

        //internal FontFamily(FontFamilyIdentifier familyIdentifier)
        //{
        //    _familyIdentifier = familyIdentifier;
        //}

        /// <summary>
        /// Construct an anonymous font family, i.e., a composite font that is created
        /// programatically instead of referenced by name or URI.
        /// </summary>
        public FontFamily()
        {

        }
    }
}
