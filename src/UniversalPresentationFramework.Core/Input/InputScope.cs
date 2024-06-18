using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Input
{
    [TypeConverter(typeof(InputScopeConverter))]
    public class InputScope
    {
        private readonly IList<InputScopeName> _scopeNames = new List<InputScopeName>();
        private readonly IList<InputScopePhrase> _phraseList = new List<InputScopePhrase>();
        private string? _regexString;
        private string? _srgsMarkup;

        ///<summary>
        /// Names is of type InputScopeName enum. This is the simpliest way to specify InputScope for an element.
        /// PhraseList is a collection of suggested input patterns. 
        /// Each phrase is of type InputScopePhrase
        ///</summary>
        ///<remarks>
        /// We should support combination of input scope enum values in the future
        ///</remarks>
        public IList<InputScopeName> Names => _scopeNames;

        ///<summary>
        /// SrgsMarkup is currently speech specific. Will be used in non-speech 
        /// input methods in the near future too
        ///</summary>
        [DefaultValue(null)]
        public string? SrgsMarkup
        {
            get
            {
                return _srgsMarkup;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _srgsMarkup = value;
            }
        }

        ///<summary>
        /// RegularExpression is used as a suggested input text pattern
        /// for input processors.
        ///</summary>
        [DefaultValue(null)]
        public string? RegularExpression
        {
            get
            {
                return _regexString;
            }
            set
            {
                if (value == null)
                {
                    throw new ArgumentNullException("value");
                }
                _regexString = value;
            }
        }
        ///<summary>
        /// PhraseList is a collection of suggested input patterns. 
        /// Each phrase is of type InputScopePhrase
        ///</summary>
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public IList<InputScopePhrase> PhraseList => _phraseList;
    }
}
