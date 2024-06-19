using SharpGen.Runtime.Win32;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Windows.Win32;
using Windows.Win32.Foundation;
using Windows.Win32.UI.Input.Ime;
using Windows.Win32.UI.TextServices;
using Windows.Win32.UI.WindowsAndMessaging;
using Wodsoft.UI.Input;
using Wodsoft.UI.Media;
using Wodsoft.UI.Threading;
using static System.Net.Mime.MediaTypeNames;
//using static Wodsoft.UI.Platforms.Win32.TextServicesFramework.UnsafeNativeMethods;

#pragma warning disable CA1416 // 验证平台兼容性
namespace Wodsoft.UI.Platforms.Win32
{
    public class Win32InputMethod : InputMethod,
        ITfContextOwnerCompositionSink, ITfContextOwner, ITfTextEditSink, ITfEditSession
    {
        private readonly WindowContext _windowContext;
        private ITfThreadMgr2? _tfThreadMgr;
        private ITfDocumentMgr? _tfDocumentMgr;
        private ITfContext? _tfContext;
        private uint _clientId, _editCookie, _contextSinkCookie, _editSinkCookie;
        private Win32InputMethodContext? _inputContext;
        private bool _isEndComposition, _isStartComposition, _isChangeAttribute, _isCompletedComposition = true;

        #region Constructor

        public Win32InputMethod(WindowContext windowContext)
        {
            _windowContext = windowContext;
        }

        #endregion

        #region Properties

        //public override InputMethodState ImeState
        //{
        //    get
        //    {
        //        var focusedElement = Keyboard.FocusedElement;
        //        if (focusedElement != null && focusedElement.Dispatcher is Win32Dispatcher dispatcher)
        //        {
        //            if (IsImm32ImeCurrent())
        //            {
        //                var hwnd = dispatcher.WindowContext.Hwnd;
        //                var himc = PInvoke.ImmGetContext(hwnd);
        //                var isOpen = PInvoke.ImmGetOpenStatus(himc);
        //                PInvoke.ImmReleaseContext(hwnd, himc);
        //                return isOpen ? InputMethodState.On : InputMethodState.Off;
        //            }
        //            else
        //            {
        //                var compartment = TextServicesCompartmentContext.GetCompartment(InputMethodStateType.ImeState);
        //                if (compartment != null)
        //                    return compartment.BooleanValue ? InputMethodState.On : InputMethodState.Off;
        //            }
        //        }
        //        return InputMethodState.Off;
        //    }
        //    set
        //    {
        //        var focusedElement = Keyboard.FocusedElement;
        //        if (focusedElement != null && focusedElement.Dispatcher is Win32Dispatcher dispatcher)
        //        {
        //            var compartment = TextServicesCompartmentContext.GetCompartment(InputMethodStateType.ImeState);
        //            if (compartment != null)
        //                compartment.BooleanValue = value == InputMethodState.On;
        //            if (_immEnabled)
        //            {
        //                var hwnd = dispatcher.WindowContext.Hwnd;
        //                var himc = PInvoke.ImmGetContext(hwnd);
        //                var isOpen = PInvoke.ImmGetOpenStatus(himc);
        //                if (isOpen != (value == InputMethodState.On))
        //                    PInvoke.ImmSetOpenStatus(himc, value == InputMethodState.On);
        //                PInvoke.ImmReleaseContext(hwnd, himc);
        //            }
        //        }
        //    }
        //}

        //public override InputMethodState MicrophoneState
        //{
        //    get
        //    {
        //        var focusedElement = Keyboard.FocusedElement;
        //        if (focusedElement != null && focusedElement.Dispatcher is Win32Dispatcher dispatcher)
        //        {
        //            var compartment = TextServicesCompartmentContext.GetCompartment(InputMethodStateType.MicrophoneState);
        //            if (compartment != null)
        //                return compartment.BooleanValue ? InputMethodState.On : InputMethodState.Off;
        //        }
        //        return InputMethodState.Off;
        //    }
        //    set
        //    {
        //        var focusedElement = Keyboard.FocusedElement;
        //        if (focusedElement != null && focusedElement.Dispatcher is Win32Dispatcher dispatcher)
        //        {
        //            var compartment = TextServicesCompartmentContext.GetCompartment(InputMethodStateType.MicrophoneState);
        //            if (compartment != null)
        //                compartment.BooleanValue = value == InputMethodState.On;
        //        }
        //    }
        //}

        //public override InputMethodState HandwritingState
        //{
        //    get
        //    {
        //        var focusedElement = Keyboard.FocusedElement;
        //        if (focusedElement != null && focusedElement.Dispatcher is Win32Dispatcher dispatcher)
        //        {
        //            var compartment = TextServicesCompartmentContext.GetCompartment(InputMethodStateType.HandwritingState);
        //            if (compartment != null)
        //                return compartment.BooleanValue ? InputMethodState.On : InputMethodState.Off;
        //        }
        //        return InputMethodState.Off;
        //    }
        //    set
        //    {
        //        var focusedElement = Keyboard.FocusedElement;
        //        if (focusedElement != null && focusedElement.Dispatcher is Win32Dispatcher dispatcher)
        //        {
        //            var compartment = TextServicesCompartmentContext.GetCompartment(InputMethodStateType.HandwritingState);
        //            if (compartment != null)
        //                compartment.BooleanValue = value == InputMethodState.On;
        //        }
        //    }
        //}

        //public override SpeechMode SpeechMode
        //{
        //    get
        //    {
        //        var focusedElement = Keyboard.FocusedElement;
        //        if (focusedElement != null && focusedElement.Dispatcher is Win32Dispatcher dispatcher)
        //        {
        //            var compartment = TextServicesCompartmentContext.GetCompartment(InputMethodStateType.SpeechMode);
        //            if (compartment != null)
        //            {
        //                var value = compartment.IntValue;
        //                if ((value & TF_DICTATION_ON) != 0)
        //                    return SpeechMode.Dictation;
        //                if ((value & TF_COMMANDING_ON) != 0)
        //                    return SpeechMode.Command;
        //            }
        //        }
        //        return SpeechMode.Indeterminate;
        //    }
        //    set
        //    {
        //        var focusedElement = Keyboard.FocusedElement;
        //        if (focusedElement != null && focusedElement.Dispatcher is Win32Dispatcher dispatcher)
        //        {
        //            var compartment = TextServicesCompartmentContext.GetCompartment(InputMethodStateType.SpeechMode);
        //            if (compartment != null)
        //            {
        //                int nValue = compartment.IntValue;
        //                if (value == SpeechMode.Dictation)
        //                {
        //                    nValue &= ~TF_COMMANDING_ON;
        //                    nValue |= TF_DICTATION_ON;
        //                    // we don't have to set compartment unless the value is changed.
        //                    if (compartment.IntValue != nValue)
        //                    {
        //                        compartment.IntValue = nValue;
        //                    }
        //                }
        //                else if (value == SpeechMode.Command)
        //                {
        //                    nValue &= ~TF_DICTATION_ON;
        //                    nValue |= TF_COMMANDING_ON;
        //                    // we don't have to set compartment unless the value is changed.
        //                    if (compartment.IntValue != nValue)
        //                    {
        //                        compartment.IntValue = nValue;
        //                    }
        //                }
        //            }
        //        }
        //    }
        //}

        //public unsafe override ImeConversionModeValues ImeConversionMode
        //{
        //    get
        //    {
        //        var focusedElement = Keyboard.FocusedElement;
        //        if (focusedElement != null && focusedElement.Dispatcher is Win32Dispatcher dispatcher)
        //        {
        //            if (IsImm32ImeCurrent())
        //            {
        //                var hwnd = dispatcher.WindowContext.Hwnd;
        //                var himc = PInvoke.ImmGetContext(hwnd);
        //                IME_CONVERSION_MODE convmode;
        //                IME_SENTENCE_MODE sentence;
        //                PInvoke.ImmGetConversionStatus(himc, &convmode, &sentence);
        //                PInvoke.ImmReleaseContext(hwnd, himc);
        //                ImeConversionModeValues ret = 0;
        //                if (((int)convmode & (IME_CMODE_NATIVE | IME_CMODE_KATAKANA)) == 0)
        //                    ret |= ImeConversionModeValues.Alphanumeric;
        //                if (((int)convmode & IME_CMODE_NATIVE) != 0)
        //                    ret |= ImeConversionModeValues.Native;
        //                if (((int)convmode & IME_CMODE_KATAKANA) != 0)
        //                    ret |= ImeConversionModeValues.Katakana;
        //                if (((int)convmode & IME_CMODE_FULLSHAPE) != 0)
        //                    ret |= ImeConversionModeValues.FullShape;
        //                if (((int)convmode & IME_CMODE_ROMAN) != 0)
        //                    ret |= ImeConversionModeValues.Roman;
        //                if (((int)convmode & IME_CMODE_CHARCODE) != 0)
        //                    ret |= ImeConversionModeValues.CharCode;
        //                if (((int)convmode & IME_CMODE_NOCONVERSION) != 0)
        //                    ret |= ImeConversionModeValues.NoConversion;
        //                if (((int)convmode & IME_CMODE_EUDC) != 0)
        //                    ret |= ImeConversionModeValues.Eudc;
        //                if (((int)convmode & IME_CMODE_SYMBOL) != 0)
        //                    ret |= ImeConversionModeValues.Symbol;
        //                if (((int)convmode & IME_CMODE_FIXED) != 0)
        //                    ret |= ImeConversionModeValues.Fixed;

        //                return ret;
        //            }
        //            else
        //            {
        //                var compartment = TextServicesCompartmentContext.GetCompartment(InputMethodStateType.ImeConversionModeValues);
        //                if (compartment != null)
        //                {
        //                    ConversionModeFlags convmode = (ConversionModeFlags)compartment.IntValue;
        //                    ImeConversionModeValues ret = 0;
        //                    if ((convmode & (ConversionModeFlags.TF_CONVERSIONMODE_NATIVE | ConversionModeFlags.TF_CONVERSIONMODE_KATAKANA)) == 0)
        //                        ret |= ImeConversionModeValues.Alphanumeric;
        //                    if ((convmode & ConversionModeFlags.TF_CONVERSIONMODE_NATIVE) != 0)
        //                        ret |= ImeConversionModeValues.Native;
        //                    if ((convmode & ConversionModeFlags.TF_CONVERSIONMODE_KATAKANA) != 0)
        //                        ret |= ImeConversionModeValues.Katakana;
        //                    if ((convmode & ConversionModeFlags.TF_CONVERSIONMODE_FULLSHAPE) != 0)
        //                        ret |= ImeConversionModeValues.FullShape;
        //                    if ((convmode & ConversionModeFlags.TF_CONVERSIONMODE_ROMAN) != 0)
        //                        ret |= ImeConversionModeValues.Roman;
        //                    if ((convmode & ConversionModeFlags.TF_CONVERSIONMODE_CHARCODE) != 0)
        //                        ret |= ImeConversionModeValues.CharCode;
        //                    if ((convmode & ConversionModeFlags.TF_CONVERSIONMODE_NOCONVERSION) != 0)
        //                        ret |= ImeConversionModeValues.NoConversion;
        //                    if ((convmode & ConversionModeFlags.TF_CONVERSIONMODE_EUDC) != 0)
        //                        ret |= ImeConversionModeValues.Eudc;
        //                    if ((convmode & ConversionModeFlags.TF_CONVERSIONMODE_SYMBOL) != 0)
        //                        ret |= ImeConversionModeValues.Symbol;
        //                    if ((convmode & ConversionModeFlags.TF_CONVERSIONMODE_FIXED) != 0)
        //                        ret |= ImeConversionModeValues.Fixed;
        //                    return ret;
        //                }
        //            }
        //        }
        //        return ImeConversionModeValues.Alphanumeric;
        //    }
        //    set
        //    {
        //        var focusedElement = Keyboard.FocusedElement;
        //        if (focusedElement != null && focusedElement.Dispatcher is Win32Dispatcher dispatcher)
        //        {
        //            var compartment = TextServicesCompartmentContext.GetCompartment(InputMethodStateType.ImeConversionModeValues);
        //            if (compartment != null)
        //            {
        //                ConversionModeFlags currentConvMode;
        //                if (_immEnabled)
        //                {
        //                    currentConvMode = Imm32ConversionModeToTSFConversionMode(dispatcher.WindowContext.Hwnd);
        //                }
        //                else
        //                {
        //                    currentConvMode = (ConversionModeFlags)compartment.IntValue;
        //                }

        //                ConversionModeFlags convmode = 0;

        //                // TF_CONVERSIONMODE_ALPHANUMERIC is 0.
        //                // if ((value & ImeConversionModeValues.Alphanumeric) != 0)
        //                //     convmode |= ConversionModeFlags.TF_CONVERSIONMODE_ALPHANUMERIC;
        //                if ((value & ImeConversionModeValues.Native) != 0)
        //                    convmode |= ConversionModeFlags.TF_CONVERSIONMODE_NATIVE;
        //                if ((value & ImeConversionModeValues.Katakana) != 0)
        //                    convmode |= ConversionModeFlags.TF_CONVERSIONMODE_KATAKANA;
        //                if ((value & ImeConversionModeValues.FullShape) != 0)
        //                    convmode |= ConversionModeFlags.TF_CONVERSIONMODE_FULLSHAPE;
        //                if ((value & ImeConversionModeValues.Roman) != 0)
        //                    convmode |= ConversionModeFlags.TF_CONVERSIONMODE_ROMAN;
        //                if ((value & ImeConversionModeValues.CharCode) != 0)
        //                    convmode |= ConversionModeFlags.TF_CONVERSIONMODE_CHARCODE;
        //                if ((value & ImeConversionModeValues.NoConversion) != 0)
        //                    convmode |= ConversionModeFlags.TF_CONVERSIONMODE_NOCONVERSION;
        //                if ((value & ImeConversionModeValues.Eudc) != 0)
        //                    convmode |= ConversionModeFlags.TF_CONVERSIONMODE_EUDC;
        //                if ((value & ImeConversionModeValues.Symbol) != 0)
        //                    convmode |= ConversionModeFlags.TF_CONVERSIONMODE_SYMBOL;
        //                if ((value & ImeConversionModeValues.Fixed) != 0)
        //                    convmode |= ConversionModeFlags.TF_CONVERSIONMODE_FIXED;

        //                // We don't have to set the value unless the value is changed.
        //                if (currentConvMode != convmode)
        //                {
        //                    ConversionModeFlags conversionModeClearBit = 0;

        //                    if (convmode == (ConversionModeFlags.TF_CONVERSIONMODE_NATIVE | ConversionModeFlags.TF_CONVERSIONMODE_FULLSHAPE))
        //                    {
        //                        // Chinese, Hiragana or Korean so clear Katakana
        //                        conversionModeClearBit = ConversionModeFlags.TF_CONVERSIONMODE_KATAKANA;
        //                    }
        //                    else if (convmode == (ConversionModeFlags.TF_CONVERSIONMODE_KATAKANA | ConversionModeFlags.TF_CONVERSIONMODE_NATIVE))
        //                    {
        //                        // Katakana Half
        //                        conversionModeClearBit = ConversionModeFlags.TF_CONVERSIONMODE_FULLSHAPE;
        //                    }
        //                    else if (convmode == ConversionModeFlags.TF_CONVERSIONMODE_FULLSHAPE)
        //                    {
        //                        // Alpha Full
        //                        conversionModeClearBit = ConversionModeFlags.TF_CONVERSIONMODE_KATAKANA | ConversionModeFlags.TF_CONVERSIONMODE_NATIVE;
        //                    }
        //                    else if (convmode == ConversionModeFlags.TF_CONVERSIONMODE_ALPHANUMERIC)
        //                    {
        //                        // Alpha Half
        //                        conversionModeClearBit = ConversionModeFlags.TF_CONVERSIONMODE_FULLSHAPE | ConversionModeFlags.TF_CONVERSIONMODE_KATAKANA | ConversionModeFlags.TF_CONVERSIONMODE_NATIVE;
        //                    }
        //                    else if (convmode == ConversionModeFlags.TF_CONVERSIONMODE_NATIVE)
        //                    {
        //                        // Hangul
        //                        conversionModeClearBit = ConversionModeFlags.TF_CONVERSIONMODE_FULLSHAPE;
        //                    }

        //                    // Set the new conversion mode bit and apply the clear bit
        //                    convmode |= currentConvMode;
        //                    convmode &= ~conversionModeClearBit;

        //                    compartment.IntValue = (int)convmode;
        //                }
        //            }
        //            if (_immEnabled)
        //            {
        //                var hwnd = dispatcher.WindowContext.Hwnd;
        //                var himc = PInvoke.ImmGetContext(hwnd);
        //                IME_CONVERSION_MODE convmode;
        //                IME_SENTENCE_MODE sentence;
        //                PInvoke.ImmGetConversionStatus(himc, &convmode, &sentence);

        //                IME_CONVERSION_MODE convmodeNew = 0;
        //                // IME_CMODE_ALPHANUMERIC is 0.
        //                // if ((value & ImeConversionModeValues.Alphanumeric) != 0)
        //                //     convmodeNew |= IME_CMODE_ALPHANUMERIC;
        //                if ((value & ImeConversionModeValues.Native) != 0)
        //                    convmodeNew |= IME_CONVERSION_MODE.IME_CMODE_NATIVE;
        //                if ((value & ImeConversionModeValues.Katakana) != 0)
        //                    convmodeNew |= IME_CONVERSION_MODE.IME_CMODE_KATAKANA;
        //                if ((value & ImeConversionModeValues.FullShape) != 0)
        //                    convmodeNew |= IME_CONVERSION_MODE.IME_CMODE_FULLSHAPE;
        //                if ((value & ImeConversionModeValues.Roman) != 0)
        //                    convmodeNew |= IME_CONVERSION_MODE.IME_CMODE_ROMAN;
        //                if ((value & ImeConversionModeValues.CharCode) != 0)
        //                    convmodeNew |= IME_CONVERSION_MODE.IME_CMODE_CHARCODE;
        //                if ((value & ImeConversionModeValues.NoConversion) != 0)
        //                    convmodeNew |= IME_CONVERSION_MODE.IME_CMODE_NOCONVERSION;
        //                if ((value & ImeConversionModeValues.Eudc) != 0)
        //                    convmodeNew |= IME_CONVERSION_MODE.IME_CMODE_EUDC;
        //                if ((value & ImeConversionModeValues.Symbol) != 0)
        //                    convmodeNew |= IME_CONVERSION_MODE.IME_CMODE_SYMBOL;
        //                if ((value & ImeConversionModeValues.Fixed) != 0)
        //                    convmodeNew |= IME_CONVERSION_MODE.IME_CMODE_FIXED;

        //                if (convmode != convmodeNew)
        //                {
        //                    IME_CONVERSION_MODE conversionModeClearBit = default;

        //                    if (convmodeNew == (IME_CONVERSION_MODE.IME_CMODE_NATIVE | IME_CONVERSION_MODE.IME_CMODE_FULLSHAPE))
        //                    {
        //                        // Chinese, Hiragana or Korean so clear Katakana
        //                        conversionModeClearBit = IME_CONVERSION_MODE.IME_CMODE_KATAKANA;
        //                    }
        //                    else if (convmodeNew == (IME_CONVERSION_MODE.IME_CMODE_KATAKANA | IME_CONVERSION_MODE.IME_CMODE_NATIVE))
        //                    {
        //                        // Katakana Half
        //                        conversionModeClearBit = IME_CONVERSION_MODE.IME_CMODE_FULLSHAPE;
        //                    }
        //                    else if (convmodeNew == IME_CONVERSION_MODE.IME_CMODE_FULLSHAPE)
        //                    {
        //                        // Alpha Full
        //                        conversionModeClearBit = IME_CONVERSION_MODE.IME_CMODE_KATAKANA | IME_CONVERSION_MODE.IME_CMODE_NATIVE;
        //                    }
        //                    else if (convmodeNew == IME_CONVERSION_MODE.IME_CMODE_ALPHANUMERIC)
        //                    {
        //                        // Alpha Half
        //                        conversionModeClearBit = IME_CONVERSION_MODE.IME_CMODE_FULLSHAPE | IME_CONVERSION_MODE.IME_CMODE_KATAKANA | IME_CONVERSION_MODE.IME_CMODE_NATIVE;
        //                    }
        //                    else if (convmodeNew == IME_CONVERSION_MODE.IME_CMODE_NATIVE)
        //                    {
        //                        // Hangul
        //                        conversionModeClearBit = IME_CONVERSION_MODE.IME_CMODE_FULLSHAPE;
        //                    }

        //                    // Set the new conversion mode bit and apply the clear bit
        //                    convmodeNew |= convmode;
        //                    convmodeNew &= ~conversionModeClearBit;

        //                    PInvoke.ImmSetConversionStatus(himc, convmodeNew, sentence);
        //                }
        //                PInvoke.ImmReleaseContext(hwnd, himc);
        //            }
        //        }
        //    }
        //}

        //public unsafe override ImeSentenceModeValues ImeSentenceMode
        //{
        //    get
        //    {
        //        var focusedElement = Keyboard.FocusedElement;
        //        if (focusedElement != null && focusedElement.Dispatcher is Win32Dispatcher dispatcher)
        //        {
        //            if (IsImm32ImeCurrent())
        //            {
        //                var hwnd = dispatcher.WindowContext.Hwnd;
        //                var himc = PInvoke.ImmGetContext(hwnd);
        //                IME_CONVERSION_MODE convmode;
        //                IME_SENTENCE_MODE sentence;
        //                PInvoke.ImmGetConversionStatus(himc, &convmode, &sentence);
        //                PInvoke.ImmReleaseContext(hwnd, himc);
        //                ImeSentenceModeValues ret = 0;

        //                // TF_SENTENCEMODE_ALPHANUMERIC is 0. 
        //                if (sentence == IME_SMODE_NONE)
        //                    return ImeSentenceModeValues.None;

        //                if ((sentence & IME_SENTENCE_MODE.IME_SMODE_PLAURALCLAUSE) != 0)
        //                    ret |= ImeSentenceModeValues.PluralClause;
        //                if ((sentence & IME_SENTENCE_MODE.IME_SMODE_SINGLECONVERT) != 0)
        //                    ret |= ImeSentenceModeValues.SingleConversion;
        //                if ((sentence & IME_SENTENCE_MODE.IME_SMODE_AUTOMATIC) != 0)
        //                    ret |= ImeSentenceModeValues.Automatic;
        //                if ((sentence & IME_SENTENCE_MODE.IME_SMODE_PHRASEPREDICT) != 0)
        //                    ret |= ImeSentenceModeValues.PhrasePrediction;
        //                if ((sentence & IME_SENTENCE_MODE.IME_SMODE_CONVERSATION) != 0)
        //                    ret |= ImeSentenceModeValues.Conversation;

        //                return ret;
        //            }
        //            else
        //            {
        //                var compartment = TextServicesCompartmentContext.GetCompartment(InputMethodStateType.ImeSentenceModeValues);
        //                if (compartment != null)
        //                {
        //                    SentenceModeFlags convmode = (SentenceModeFlags)compartment.IntValue;
        //                    ImeSentenceModeValues ret = 0;

        //                    // TF_SENTENCEMODE_ALPHANUMERIC is 0. 
        //                    if (convmode == SentenceModeFlags.TF_SENTENCEMODE_NONE)
        //                        return ImeSentenceModeValues.None;

        //                    if ((convmode & SentenceModeFlags.TF_SENTENCEMODE_PLAURALCLAUSE) != 0)
        //                        ret |= ImeSentenceModeValues.PluralClause;
        //                    if ((convmode & SentenceModeFlags.TF_SENTENCEMODE_SINGLECONVERT) != 0)
        //                        ret |= ImeSentenceModeValues.SingleConversion;
        //                    if ((convmode & SentenceModeFlags.TF_SENTENCEMODE_AUTOMATIC) != 0)
        //                        ret |= ImeSentenceModeValues.Automatic;
        //                    if ((convmode & SentenceModeFlags.TF_SENTENCEMODE_PHRASEPREDICT) != 0)
        //                        ret |= ImeSentenceModeValues.PhrasePrediction;
        //                    if ((convmode & SentenceModeFlags.TF_SENTENCEMODE_CONVERSATION) != 0)
        //                        ret |= ImeSentenceModeValues.Conversation;
        //                    return ret;
        //                }
        //            }
        //        }
        //        return ImeSentenceModeValues.None;
        //    }
        //    set
        //    {
        //        var focusedElement = Keyboard.FocusedElement;
        //        if (focusedElement != null && focusedElement.Dispatcher is Win32Dispatcher dispatcher)
        //        {
        //            var compartment = TextServicesCompartmentContext.GetCompartment(InputMethodStateType.ImeSentenceModeValues);
        //            if (compartment != null)
        //            {
        //                SentenceModeFlags convmode = 0;

        //                if ((value & ImeSentenceModeValues.PluralClause) != 0)
        //                    convmode |= SentenceModeFlags.TF_SENTENCEMODE_PLAURALCLAUSE;
        //                if ((value & ImeSentenceModeValues.SingleConversion) != 0)
        //                    convmode |= SentenceModeFlags.TF_SENTENCEMODE_SINGLECONVERT;
        //                if ((value & ImeSentenceModeValues.Automatic) != 0)
        //                    convmode |= SentenceModeFlags.TF_SENTENCEMODE_AUTOMATIC;
        //                if ((value & ImeSentenceModeValues.PhrasePrediction) != 0)
        //                    convmode |= SentenceModeFlags.TF_SENTENCEMODE_PHRASEPREDICT;
        //                if ((value & ImeSentenceModeValues.Conversation) != 0)
        //                    convmode |= SentenceModeFlags.TF_SENTENCEMODE_CONVERSATION;

        //                if (compartment.IntValue != (int)convmode)
        //                    compartment.IntValue = (int)convmode;
        //            }
        //            if (_immEnabled)
        //            {
        //                var hwnd = dispatcher.WindowContext.Hwnd;
        //                var himc = PInvoke.ImmGetContext(hwnd);
        //                IME_CONVERSION_MODE convmode;
        //                IME_SENTENCE_MODE sentence;
        //                PInvoke.ImmGetConversionStatus(himc, &convmode, &sentence);

        //                IME_SENTENCE_MODE sentenceNew = 0;

        //                if ((value & ImeSentenceModeValues.PluralClause) != 0)
        //                    sentenceNew |= IME_SENTENCE_MODE.IME_SMODE_PLAURALCLAUSE;
        //                if ((value & ImeSentenceModeValues.SingleConversion) != 0)
        //                    sentenceNew |= IME_SENTENCE_MODE.IME_SMODE_SINGLECONVERT;
        //                if ((value & ImeSentenceModeValues.Automatic) != 0)
        //                    sentenceNew |= IME_SENTENCE_MODE.IME_SMODE_AUTOMATIC;
        //                if ((value & ImeSentenceModeValues.PhrasePrediction) != 0)
        //                    sentenceNew |= IME_SENTENCE_MODE.IME_SMODE_PHRASEPREDICT;
        //                if ((value & ImeSentenceModeValues.Conversation) != 0)
        //                    sentenceNew |= IME_SENTENCE_MODE.IME_SMODE_CONVERSATION;

        //                if (sentence != sentenceNew)
        //                    PInvoke.ImmSetConversionStatus(himc, convmode, sentenceNew);

        //                PInvoke.ImmReleaseContext(hwnd, himc);
        //            }
        //        }
        //    }
        //}

        //internal TextServicesContext TextServicesContext
        //{
        //    get
        //    {
        //        if (_textServicesContext == null)
        //        {
        //            _textServicesContext = new TextServicesContext(_windowContext);
        //        }
        //        return _textServicesContext;
        //    }
        //}

        //internal TextServicesCompartmentContext TextServicesCompartmentContext
        //{
        //    get
        //    {
        //        if (_textServicesCompartmentContext == null)
        //            _textServicesCompartmentContext = new TextServicesCompartmentContext();
        //        return _textServicesCompartmentContext;
        //    }
        //}

        internal ITfThreadMgr2? TextServiceThreadManager => _tfThreadMgr;

        //private HIMC DefaultImc
        //{
        //    get
        //    {
        //        if (_defaultImc == null)
        //        {
        //            // 
        //            //  Get the default HIMC from default IME window.
        //            // 
        //            var hwnd = PInvoke.ImmGetDefaultIMEWnd(HWND.Null);
        //            var himc = PInvoke.ImmGetContext(hwnd);

        //            // Store the default imc to _defaultImc.
        //            _defaultImc = himc;

        //            PInvoke.ImmReleaseContext(hwnd, himc);
        //        }
        //        return _defaultImc.Value;
        //    }
        //}

        #endregion

        #region Events

        public override event InputMethodStateChangedEventHandler? StateChanged;

        #endregion

        #region Methods

        protected override void Disable(IInputElement inputElement)
        {
            if (inputElement == null || inputElement.Dispatcher is not Win32Dispatcher dispatcher)
                return;

            if (Thread.CurrentThread == dispatcher.WindowContext.WindowThread)
            {
                DisableCore();
            }
            else
            {
                _windowContext.ProcessInWindowThreadAsync(DisableCore);
            }
        }

        private void DisableCore()
        {
            if (_tfThreadMgr == null)
                return;
            _tfThreadMgr.SuspendKeystrokeHandling();
            ((ITfContextOwnerCompositionServices)_tfContext!).TerminateComposition(null);
            ((ITfContextOwnerServices)_tfContext!).OnAttributeChange(_GUID_PROP_INPUTSCOPE);
        }

        protected override void Enable(IInputElement inputElement)
        {
            if (inputElement == null || inputElement.Dispatcher is not Win32Dispatcher dispatcher)
                return;

            if (Thread.CurrentThread == dispatcher.WindowContext.WindowThread)
            {
                EnableCore();
            }
            else
            {
                _windowContext.ProcessInWindowThreadAsync(EnableCore);
            }
        }

        private void EnableCore()
        {
            if (_tfThreadMgr == null)
                return;
            _tfThreadMgr.ResumeKeystrokeHandling();
            ((ITfContextOwnerServices)_tfContext!).OnAttributeChange(_GUID_PROP_INPUTSCOPE);
        }

        protected override bool IsValidFocus(DependencyObject focus)
        {
            return focus.Dispatcher is Win32Dispatcher;
        }

        //protected override bool ShowConfigureUI(UIElement? element, bool show)
        //{
        //    throw new NotImplementedException();
        //}

        //protected override bool ShowRegisterWordUI(UIElement? element, bool show, string registeredText)
        //{
        //    throw new NotImplementedException();
        //}

        //private bool IsImm32ImeCurrent()
        //{
        //    if (!_immEnabled)
        //        return false;
        //    IntPtr hkl = PInvoke.GetKeyboardLayout(0);
        //    return IsImm32Ime(hkl);
        //}

        //private bool IsImm32Ime(IntPtr hkl)
        //{
        //    if (hkl == IntPtr.Zero)
        //        return false;
        //    return ((unchecked((int)hkl.ToInt64()) & 0xf0000000) == 0xe0000000);
        //}

        //private unsafe ConversionModeFlags Imm32ConversionModeToTSFConversionMode(HWND hwnd)
        //{
        //    ConversionModeFlags convMode = 0;
        //    if (hwnd != IntPtr.Zero)
        //    {
        //        IME_CONVERSION_MODE immConvMode = 0;
        //        IME_SENTENCE_MODE sentence = 0;
        //        var himc = PInvoke.ImmGetContext(hwnd);
        //        PInvoke.ImmGetConversionStatus(himc, &immConvMode, &sentence);
        //        PInvoke.ImmReleaseContext(hwnd, himc);

        //        if (((int)immConvMode & IME_CMODE_NATIVE) != 0)
        //            convMode |= ConversionModeFlags.TF_CONVERSIONMODE_NATIVE;
        //        if (((int)immConvMode & IME_CMODE_KATAKANA) != 0)
        //            convMode |= ConversionModeFlags.TF_CONVERSIONMODE_KATAKANA;
        //        if (((int)immConvMode & IME_CMODE_FULLSHAPE) != 0)
        //            convMode |= ConversionModeFlags.TF_CONVERSIONMODE_FULLSHAPE;
        //        if (((int)immConvMode & IME_CMODE_ROMAN) != 0)
        //            convMode |= ConversionModeFlags.TF_CONVERSIONMODE_ROMAN;
        //        if (((int)immConvMode & IME_CMODE_CHARCODE) != 0)
        //            convMode |= ConversionModeFlags.TF_CONVERSIONMODE_CHARCODE;
        //        if (((int)immConvMode & IME_CMODE_NOCONVERSION) != 0)
        //            convMode |= ConversionModeFlags.TF_CONVERSIONMODE_NOCONVERSION;
        //        if (((int)immConvMode & IME_CMODE_EUDC) != 0)
        //            convMode |= ConversionModeFlags.TF_CONVERSIONMODE_EUDC;
        //        if (((int)immConvMode & IME_CMODE_SYMBOL) != 0)
        //            convMode |= ConversionModeFlags.TF_CONVERSIONMODE_SYMBOL;
        //        if (((int)immConvMode & IME_CMODE_FIXED) != 0)
        //            convMode |= ConversionModeFlags.TF_CONVERSIONMODE_FIXED;
        //    }
        //    return convMode;
        //}

        public override IInputMethodContext? CreateContext(IInputMethodSource source)
        {
            return new Win32InputMethodContext(this, _windowContext, source);
        }

        internal void Initialize()
        {
            _tfThreadMgr = TextServicesLoader.Load();
            if (_tfThreadMgr == null)
                return;

            _tfThreadMgr.ActivateEx(out _clientId, 4u);
            _tfThreadMgr.CreateDocumentMgr(out _tfDocumentMgr);
            _tfDocumentMgr.CreateContext(_clientId, 0, this, out _tfContext, out _editCookie);
            _tfDocumentMgr.Push(_tfContext);

            ITfSource tfSource = (ITfSource)_tfContext;
            tfSource.AdviseSink(typeof(ITfContextOwner).GUID, this, out _contextSinkCookie);
            tfSource.AdviseSink(typeof(ITfTextEditSink).GUID, this, out _editSinkCookie);

            _tfThreadMgr.SetFocus(_tfDocumentMgr);

            _tfThreadMgr.SuspendKeystrokeHandling();
            ((ITfContextOwnerCompositionServices)_tfContext).TerminateComposition(null);
        }

        internal void Focus()
        {
            if (_tfThreadMgr != null)
                _tfThreadMgr.SetFocus(_tfDocumentMgr!);
        }

        internal void Focus(Win32InputMethodContext context)
        {
            _inputContext = context;
            if (Thread.CurrentThread == context.WindowContext.WindowThread)
            {
                EnableCore();
            }
            else
            {
                _windowContext.ProcessInWindowThreadAsync(EnableCore);
            }
        }

        internal void Unfocus(Win32InputMethodContext context)
        {
            if (Thread.CurrentThread == context.WindowContext.WindowThread)
            {
                DisableCore();
            }
            else
            {
                _windowContext.ProcessInWindowThreadAsync(DisableCore);
            }
        }

        #endregion

        #region ITfContextOwnerCompositionSink

        unsafe void ITfContextOwnerCompositionSink.OnStartComposition(ITfCompositionView pComposition, BOOL* pfOk)
        {
            if (_inputContext != null)
            {
                _isStartComposition = true;
                _isCompletedComposition = false;
                Debug.WriteLine("开始输入法");
                *pfOk = true;
            }
        }

        void ITfContextOwnerCompositionSink.OnUpdateComposition(ITfCompositionView pComposition, ITfRange pRangeNew)
        {

        }

        void ITfContextOwnerCompositionSink.OnEndComposition(ITfCompositionView pComposition)
        {
            if (_inputContext != null)
            {
                _tfContext!.RequestEditSession(_clientId, this, TF_CONTEXT_EDIT_CONTEXT_FLAGS.TF_ES_SYNC | TF_CONTEXT_EDIT_CONTEXT_FLAGS.TF_ES_READ, out var result);
                _isEndComposition = true;
                _tfContext!.RequestEditSession(_clientId, this, TF_CONTEXT_EDIT_CONTEXT_FLAGS.TF_ES_ASYNC | TF_CONTEXT_EDIT_CONTEXT_FLAGS.TF_ES_READWRITE, out result);
            }
        }

        #endregion

        #region ITfContextOwner

        unsafe void ITfContextOwner.GetACPFromPoint(System.Drawing.Point* ptScreen, uint dwFlags, out int pacp)
        {
            pacp = 0;
        }

        unsafe void ITfContextOwner.GetTextExt(int acpStart, int acpEnd, RECT* prc, BOOL* pfClipped)
        {
            if (_inputContext != null)
            {
                var point = _inputContext.Source.CaretPosition;
                if (_windowContext.DpiX != 1f)
                    point.X *= _windowContext.DpiX;
                if (_windowContext.DpiY != 1f)
                    point.Y *= _windowContext.DpiY;
                point.X += _windowContext.ClientX;
                point.Y += _windowContext.ClientY;
                prc->left = (int)point.X;
                prc->top = (int)point.Y;
                prc->right = (int)point.X;
                prc->bottom = (int)point.Y;
                Debug.WriteLine($"输入法坐标：{point.X}, {point.Y}");
            }
        }

        unsafe void ITfContextOwner.GetScreenExt(RECT* prc)
        {
            prc->left = _windowContext.ClientX;
            prc->top = _windowContext.ClientY;
            prc->right = _windowContext.ClientX + _windowContext.ClientWidth;
            prc->bottom = _windowContext.ClientY + _windowContext.ClientHeight;
        }

        unsafe void ITfContextOwner.GetStatus(TS_STATUS* pdcs)
        {
            pdcs->dwDynamicFlags = 0;
            pdcs->dwStaticFlags = 0;
        }

        unsafe void ITfContextOwner.GetWnd(HWND* phwnd)
        {
            var hwnd = _windowContext.Hwnd;
            *phwnd = hwnd;
        }

        private static readonly Guid _GUID_PROP_INPUTSCOPE = new Guid(0x1713dd5a, 0x68e7, 0x4a5b, 0x9a, 0xf6, 0x59, 0x2a, 0x59, 0x5c, 0x77, 0x8d);

        unsafe void ITfContextOwner.GetAttribute(Guid* rguidAttribute, out object? pvarValue)
        {
            _isChangeAttribute = true;
            if (_inputContext != null && *rguidAttribute == _GUID_PROP_INPUTSCOPE)
            {
                pvarValue = new Win32InputScopeAttribute((Input.InputScope?)_inputContext.Source.UIScope.GetValue(InputScopeProperty));
            }
            else
                pvarValue = null;
        }

        #endregion

        #region ITfTextEditSink

        unsafe void ITfTextEditSink.OnEndEdit(ITfContext pic, uint ecReadOnly, ITfEditRecord pEditRecord)
        {
            try
            {
                if (_isChangeAttribute && !_isStartComposition)
                {
                    _isChangeAttribute = false;
                    return;
                }
                if (_inputContext != null && !_isEndComposition)
                {
                    pic.GetStart(ecReadOnly, out var start);
                    pic.GetEnd(ecReadOnly, out var end);
                    start.ShiftEndToRange(ecReadOnly, end, TfAnchor.TF_ANCHOR_END);

                    char[] chars = new char[65];
                    uint length;
                    fixed (char* charPtr = &chars[0])
                    {
                        PWSTR buffer = new PWSTR(charPtr);
                        start.GetText(ecReadOnly, 0, buffer, 64, out length);
                    }
                    int caretPosition;
                    if (length == 0)
                        caretPosition = 0;
                    else
                    {
                        var selection = new TF_SELECTION[1];
                        pic.GetSelection(ecReadOnly, uint.MaxValue, 1, selection, out var fetched);
                        var range = (ITfRangeACP)selection[0].range;
                        range.GetExtent(out caretPosition, out _);
                    }
                    var text = new string(chars, 0, (int)length);
                    TextCompositionStage stage;
                    if (_isStartComposition)
                    {
                        stage = TextCompositionStage.Started;
                        _isStartComposition = false;
                    }
                    else
                        stage = TextCompositionStage.Changing;
                    _inputContext.Source.UIScope.Dispatcher.InvokeAsync(() =>
                    {
                        OnTextComposition(text, caretPosition, stage);
                    });
                    Debug.WriteLine("键入输入法");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        #endregion

        #region ITfEditSession

        unsafe void ITfEditSession.DoEditSession(uint ec)
        {
            try
            {
                if (_inputContext != null)
                {
                    _tfContext!.GetStart(ec, out var start);
                    _tfContext.GetEnd(ec, out var end);
                    start.ShiftEndToRange(ec, end, TfAnchor.TF_ANCHOR_END);

                    if (_isEndComposition)
                    {
                        start.IsEmpty(ec, out var empty);
                        if (!empty)
                            start.SetText(ec, 0, null, 0);
                        _isEndComposition = false;
                        return;
                    }

                    if (_isCompletedComposition)
                        return;

                    char[] chars = new char[65];
                    uint length;
                    fixed (char* charPtr = &chars[0])
                    {
                        PWSTR buffer = new PWSTR(charPtr);
                        start.GetText(ec, 0, buffer, 64, out length);
                    }
                    int caretPosition;
                    if (length == 0)
                        caretPosition = 0;
                    else
                    {
                        var selection = new TF_SELECTION[1];
                        _tfContext.GetSelection(ec, uint.MaxValue, 1, selection, out var fetched);
                        var range = (ITfRangeACP)selection[0].range;
                        range.GetExtent(out caretPosition, out _);
                    }
                    var text = new string(chars, 0, (int)length);
                    _inputContext.Source.UIScope.Dispatcher.InvokeAsync(() =>
                    {
                        OnTextComposition(text, caretPosition, TextCompositionStage.Completed);
                    });
                    _isCompletedComposition = true;
                    Debug.WriteLine("结束输入法");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
        }

        #endregion
    }
}
