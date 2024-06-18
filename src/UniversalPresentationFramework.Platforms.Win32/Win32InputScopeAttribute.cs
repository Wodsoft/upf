using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32.Foundation;
using Windows.Win32.UI.TextServices;
using UIInputScope = Wodsoft.UI.Input.InputScope;

namespace Wodsoft.UI.Platforms.Win32
{
    internal class Win32InputScopeAttribute : ITfInputScope
    {
        #region Constructors

        // Creates a new InputScopeAttribute instance.
        internal Win32InputScopeAttribute(UIInputScope? inputscope)
        {
            _inputScope = inputscope;
        }

        #endregion Constructors

        #region Private Fields

        // InputScope value for this instance for ITfInputScope.
        private UIInputScope? _inputScope;

        public unsafe void GetInputScopes(InputScope** pprgInputScopes, out uint pcCount)
        {
            if (_inputScope != null)
            {
                *pprgInputScopes = (InputScope*)Marshal.AllocCoTaskMem(sizeof(InputScope) * _inputScope.Names.Count);
                pcCount = (uint)_inputScope.Names.Count;
                for (int i = 0; i < _inputScope.Names.Count; i++)
                {
                    *(*pprgInputScopes + i) = (InputScope)_inputScope.Names[i].NameValue;
                }
            }
            else
            {
                *pprgInputScopes = (InputScope*)Marshal.AllocCoTaskMem(sizeof(InputScope));
                **pprgInputScopes = InputScope.IS_DEFAULT;
                pcCount = 1;
            }
        }

        public unsafe void GetPhrase(BSTR** ppbstrPhrases, out uint pcCount)
        {
            if (_inputScope != null)
            {
                try
                {
                    *ppbstrPhrases = (BSTR*)Marshal.AllocCoTaskMem(Marshal.SizeOf(typeof(nint)) * _inputScope.PhraseList.Count);
                }
                catch (OutOfMemoryException)
                {
                    throw new COMException("Out of memory.");
                }
                for (int i = 0; i < _inputScope.PhraseList.Count; i++)
                {
                    *(ppbstrPhrases + i) = (BSTR*)Marshal.StringToBSTR(_inputScope!.PhraseList[i].Name);
                }
                pcCount = (uint)_inputScope.PhraseList.Count;
            }
            else
            {
                pcCount = 0;
            }
        }

        public unsafe void GetRegularExpression(BSTR* pbstrRegExp)
        {
            if (_inputScope != null && _inputScope.RegularExpression != null)
                *pbstrRegExp = (BSTR)Marshal.StringToBSTR(_inputScope.RegularExpression);
        }

        public unsafe void GetSRGS(BSTR* pbstrSRGS)
        {
            if (_inputScope != null && _inputScope.SrgsMarkup != null)
                *pbstrSRGS = (BSTR)Marshal.StringToBSTR(_inputScope.SrgsMarkup);
        }

        public unsafe void GetXML(BSTR* pbstrXML)
        {

        }

        #endregion Private Fields
    }
}
