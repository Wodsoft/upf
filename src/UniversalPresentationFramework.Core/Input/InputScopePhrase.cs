using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xaml.Markup;
using Wodsoft.UI.Markup;

namespace Wodsoft.UI.Input
{
    [ContentProperty("Name")]
    public class InputScopePhrase : IAddChild
    {
        // NOTE: this is a class rather than a simple string so that we can add more hint information
        //           for input phrase such as typing stroke, pronouciation etc.
        //           should be enhanced as needed.

        //---------------------------------------------------------------------------
        //
        // Public Methods
        //
        //--------------------------------------------------------------------------- 

        #region Public Methods

        ///<summary>
        /// Default Constructor necesary for parser
        ///</summary>
        public InputScopePhrase()
        {
        }

        ///<summary>
        /// Constructor that takes name
        ///</summary>
        public InputScopePhrase(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            _phraseName = name;
        }

        #region implementation of IAddChild 
        ///<summary>
        /// Called to Add the object as a Child. For InputScopePhrase tag this is ignored
        ///</summary>
        ///<param name="value">
        /// Object to add as a child
        ///</param>
        public void AddChild(object value)
        {
            throw new System.NotImplementedException();
        }

        /// <summary>
        ///  if text is present between InputScopePhrase tags, the text is added as a phrase name 
        /// </summary>
        ///<param name="name">
        /// Text string to add
        ///</param>
        public void AddText(string name)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }
            _phraseName = name;
        }

        #endregion IAddChild

        #endregion Public Methods

        #region class public properties

        ///<summary>
        /// Name property - this is used when accessing the string that is set to InputScopePhrase
        ///</summary>
        public string? Name
        {
            get { return _phraseName; }
            set { _phraseName = value; }
        }
        #endregion class public properties

        private string? _phraseName;
    }
}
