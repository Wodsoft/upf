using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Input
{
    public class InputMethodStateChangedEventArgs : EventArgs
    {
        //------------------------------------------------------
        //
        //  Constructors
        //
        //------------------------------------------------------

        public InputMethodStateChangedEventArgs(InputMethodStateType statetype)
        {
            _statetype = statetype;
        }

        //------------------------------------------------------
        //
        //  Public Properties
        //
        //------------------------------------------------------

        /// <summary>
        /// IME (open/close) state is changed.
        /// </summary>
        public bool IsImeStateChanged
        {
            get
            {
                return (_statetype == InputMethodStateType.ImeState);
            }
        }

        /// <summary>
        /// Microphone state is changed.
        /// </summary>
        public bool IsMicrophoneStateChanged
        {
            get
            {
                return (_statetype == InputMethodStateType.MicrophoneState);
            }
        }

        /// <summary>
        /// Handwriting state is changed.
        /// </summary>
        public bool IsHandwritingStateChanged
        {
            get
            {
                return (_statetype == InputMethodStateType.HandwritingState);
            }
        }

        /// <summary>
        /// SpeechMode state is changed.
        /// </summary>
        public bool IsSpeechModeChanged
        {
            get
            {
                return (_statetype == InputMethodStateType.SpeechMode);
            }
        }

        /// <summary>
        /// ImeConversionMode state is changed.
        /// </summary>
        public bool IsImeConversionModeChanged
        {
            get
            {
                return (_statetype == InputMethodStateType.ImeConversionModeValues);
            }
        }

        /// <summary>
        /// ImeSentenceMode state is changed.
        /// </summary>
        public bool IsImeSentenceModeChanged
        {
            get
            {
                return (_statetype == InputMethodStateType.ImeSentenceModeValues);
            }
        }

        //------------------------------------------------------
        //
        //  Private Fields
        //
        //------------------------------------------------------

        #region Private Fields

        private InputMethodStateType _statetype;

        #endregion Private Fields
    }
}
