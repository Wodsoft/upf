using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Threading;

namespace Wodsoft.UI.Input
{
    public abstract class InputMethod
    {
        #region Properties

        /// <summary>
        ///     A dependency property that enables alternative text inputs.
        /// </summary>
        public static readonly DependencyProperty IsInputMethodEnabledProperty =
                DependencyProperty.RegisterAttached(
                        "IsInputMethodEnabled",
                        typeof(bool),
                        typeof(InputMethod),
                        new PropertyMetadata(
                                true,
                                new PropertyChangedCallback(IsInputMethodEnabled_Changed)));
        private static void IsInputMethodEnabled_Changed(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            //IInputElement inputElement = (IInputElement)d;
            //if (inputElement == Keyboard.FocusedElement && d.Dispatcher is UIDispatcher dispatcher)
            //{
            //    if ((bool)e.NewValue!)
            //        dispatcher.InputMethod.Enable();
            //    else
            //        dispatcher.InputMethod.Disable();
            //}
        }
        public static void SetIsInputMethodEnabled(DependencyObject target, bool value)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            target.SetValue(IsInputMethodEnabledProperty, value);
        }
        public static bool GetIsInputMethodEnabled(DependencyObject target)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            return (bool)target.GetValue(IsInputMethodEnabledProperty)!;
        }

        /// <summary>
        ///     A dependency property that suspends alternative text inputs.
        ///     If this property is true, the document focus remains in the previous focus element
        ///     and the key events won't be dispatched into Cicero/IMEs.
        /// </summary>
        public static readonly DependencyProperty IsInputMethodSuspendedProperty =
                DependencyProperty.RegisterAttached(
                        "IsInputMethodSuspended",
                        typeof(bool),
                        typeof(InputMethod),
                        new PropertyMetadata(false));
        public static void SetIsInputMethodSuspended(DependencyObject target, bool value)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            target.SetValue(IsInputMethodSuspendedProperty, value);
        }
        public static bool GetIsInputMethodSuspended(DependencyObject target)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            return (bool)target.GetValue(IsInputMethodSuspendedProperty)!;
        }

        ///// <summary>
        ///// This is a property for UIElements such as TextBox. 
        ///// When the element gets the focus, the IME status is changed to
        ///// the preferred state (open or close)
        ///// </summary>
        //public static readonly DependencyProperty PreferredImeStateProperty =
        //        DependencyProperty.RegisterAttached(
        //                "PreferredImeState",
        //                typeof(InputMethodState),
        //                typeof(InputMethod),
        //                new PropertyMetadata(InputMethodState.DoNotCare));
        //public static void SetPreferredImeState(DependencyObject target, InputMethodState value)
        //{
        //    if (target == null)
        //        throw new ArgumentNullException("target");
        //    target.SetValue(PreferredImeStateProperty, value);
        //}
        //public static InputMethodState GetPreferredImeState(DependencyObject target)
        //{
        //    if (target == null)
        //        throw new ArgumentNullException("target");
        //    return (InputMethodState)target.GetValue(PreferredImeStateProperty)!;
        //}

        ///// <summary>
        ///// This is a property for UIElements such as TextBox. 
        ///// When the element gets the focus, the IME conversion mode is changed to
        ///// the preferred mode
        ///// </summary>
        //public static readonly DependencyProperty PreferredImeConversionModeProperty =
        //        DependencyProperty.RegisterAttached(
        //                "PreferredImeConversionMode",
        //                typeof(ImeConversionModeValues),
        //                typeof(InputMethod),
        //                new PropertyMetadata(ImeConversionModeValues.DoNotCare));
        //public static void SetPreferredImeConversionMode(DependencyObject target, ImeConversionModeValues value)
        //{
        //    if (target == null)
        //        throw new ArgumentNullException("target");
        //    target.SetValue(PreferredImeConversionModeProperty, value);
        //}
        //public static ImeConversionModeValues GetPreferredImeConversionMode(DependencyObject target)
        //{
        //    if (target == null)
        //        throw new ArgumentNullException("target");
        //    return (ImeConversionModeValues)target.GetValue(PreferredImeConversionModeProperty)!;
        //}

        ///// <summary>
        ///// This is a property for UIElements such as TextBox. 
        ///// When the element gets the focus, the IME sentence mode is changed to
        ///// the preferred mode
        ///// </summary>
        //public static readonly DependencyProperty PreferredImeSentenceModeProperty =
        //        DependencyProperty.RegisterAttached(
        //                "PreferredImeSentenceMode",
        //                typeof(ImeSentenceModeValues),
        //                typeof(InputMethod),
        //                new PropertyMetadata(ImeSentenceModeValues.DoNotCare));
        //public static void SetPreferredImeSentenceMode(DependencyObject target, ImeSentenceModeValues value)
        //{
        //    if (target == null)
        //        throw new ArgumentNullException("target");
        //    target.SetValue(PreferredImeSentenceModeProperty, value);
        //}
        //public static ImeSentenceModeValues GetPreferredImeSentenceMode(DependencyObject target)
        //{
        //    if (target == null)
        //        throw new ArgumentNullException("target");
        //    return (ImeSentenceModeValues)target.GetValue(PreferredImeSentenceModeProperty)!;
        //}

        /// <summary>
        /// InputScope is the specified document context for UIElement.
        /// This is a property for UIElements such as TextBox. 
        /// </summary>
        public static readonly DependencyProperty InputScopeProperty =
                DependencyProperty.RegisterAttached(
                        "InputScope",
                        typeof(InputScope),
                        typeof(InputMethod),
                        new PropertyMetadata(null));
        public static void SetInputScope(DependencyObject target, InputScope value)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            target.SetValue(InputScopeProperty, value);
        }
        public static InputScope GetInputScope(DependencyObject target)
        {
            if (target == null)
                throw new ArgumentNullException("target");
            return (InputScope)target.GetValue(InputScopeProperty)!;
        }

        //public abstract InputMethodState ImeState { get; set; }

        //public abstract InputMethodState MicrophoneState { get; set; }

        //public abstract InputMethodState HandwritingState { get; set; }

        //public abstract SpeechMode SpeechMode { get; set; }

        //public abstract ImeConversionModeValues ImeConversionMode { get; set; }

        //public abstract ImeSentenceModeValues ImeSentenceMode { get; set; }

        //public virtual bool CanShowConfigurationUI => ShowConfigureUI(null, false);

        //public virtual bool CanShowRegisterWordUI => ShowRegisterWordUI(null, false, "");

        /// <summary>
        ///     Return the input language manager associated 
        ///     with the current context.
        /// </summary>
        public static InputMethod Current
        {
            get
            {
                // Do not auto-create the dispatcher.
                UIDispatcher? dispatcher = Dispatcher.FromThread(Thread.CurrentThread) as UIDispatcher;
                if (dispatcher == null)
                    throw new InvalidOperationException("Can't access InputMethod from a object that not belong to a UIDispatcher.");
                return dispatcher.InputMethod;
            }
        }

        #endregion

        #region Methods

        ///// <summary>
        /////    Show the configure UI of the current active keyboard text service.
        ///// </summary> 
        //public void ShowConfigureUI()
        //{
        //    ShowConfigureUI(null);
        //}

        ///// <summary>
        /////    Show the configure UI of the current active keyboard text service.
        ///// </summary> 
        ///// <param name="element">
        /////     Specify UIElement which frame window becomes the parent of the configure UI.
        /////     This param can be null.
        ///// </param>
        //public void ShowConfigureUI(UIElement? element)
        //{
        //    ShowConfigureUI(element, true);
        //}

        //protected abstract bool ShowConfigureUI(UIElement? element, bool show);

        ///// <summary>
        /////    Show the register word UI of the current active keyboard text service.
        ///// </summary> 
        //public void ShowRegisterWordUI()
        //{
        //    ShowRegisterWordUI("");
        //}

        ///// <summary>
        /////    Show the register word UI of the current active keyboard text service.
        ///// </summary> 
        ///// <param name="registeredText">
        /////     Specify default string to be registered. This is usually shown in the 
        /////     text field of the register word UI.
        ///// </param>
        //public void ShowRegisterWordUI(string registeredText)
        //{
        //    ShowRegisterWordUI(null, registeredText);
        //}

        ///// <summary>
        /////    Show the register word UI of the current active keyboard text service.
        ///// </summary> 
        ///// <param name="element">
        /////     Specify UIElement which frame window becomes the parent of the configure UI.
        /////     This param can be null.
        ///// </param>
        ///// <param name="registeredText">
        /////     Specify default string to be registered. This is usually shown in the 
        /////     text field of the register word UI.
        ///// </param>
        //public void ShowRegisterWordUI(UIElement? element, string registeredText)
        //{
        //    ShowRegisterWordUI(element, true, registeredText);
        //}
        //protected abstract bool ShowRegisterWordUI(UIElement? element, bool show, string registeredText);

        internal void GotKeyboardFocus(DependencyObject focus)
        {
            if (focus == null || !IsValidFocus(focus))
                return;
            //ITfThreadMgr2
            ////
            //// Check the InputLanguageProperty of the focus element.
            ////
            //var state = GetPreferredImeState(focus);
            //if (state != InputMethodState.DoNotCare)
            //{
            //    ImeState = state;
            //}

            //var conversionMode = GetPreferredImeConversionMode(focus);
            //if (!conversionMode.HasFlag(ImeConversionModeValues.DoNotCare))
            //{
            //    ImeConversionMode = conversionMode;
            //}

            //var sentenceMode = GetPreferredImeSentenceMode(focus);
            //if (!sentenceMode.HasFlag(ImeSentenceModeValues.DoNotCare))
            //{
            //    ImeSentenceMode = sentenceMode;
            //}
        }

        protected abstract bool IsValidFocus(DependencyObject focus);

        protected internal abstract void Enable(IInputElement inputElement);

        protected internal abstract void Disable(IInputElement inputElement);

        public abstract IInputMethodContext? CreateContext(IInputMethodSource source);

        private TextComposition _composition = new TextComposition();
        protected internal void OnTextComposition(string compositionText, int caretPosition, TextCompositionStage stage)
        {
            _composition.CompositionText = compositionText;
            _composition.CaretPosition = caretPosition;
            _composition.Stage = stage;
            var focusedElement = Keyboard.FocusedElement;
            if (focusedElement != null)
            {
                var e = new TextCompositionEventArgs(Keyboard.PrimaryDevice, _composition);
                e.RoutedEvent = TextCompositionManager.PreviewTextInputEvent;
                focusedElement.RaiseEvent(e);
                if (!e.Handled)
                {
                    e.RoutedEvent = TextCompositionManager.TextInputEvent;
                    focusedElement.RaiseEvent(e);
                }
            }
        }

        #endregion Methods

        #region Events

        public abstract event InputMethodStateChangedEventHandler StateChanged;

        #endregion
    }

    public delegate void InputMethodStateChangedEventHandler(object sender, InputMethodStateChangedEventArgs e);
}
