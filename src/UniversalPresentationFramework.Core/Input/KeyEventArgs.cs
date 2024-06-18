using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Input
{
    /// <summary>
    ///     The KeyEventArgs class contains information about key states.
    /// </summary>
    /// <ExternalAPI/>     
    public class KeyEventArgs : KeyboardEventArgs
    {
        /// <summary>
        ///     Constructs an instance of the KeyEventArgs class.
        /// </summary>
        /// <param name="keyboard">
        ///     The logical keyboard device associated with this event.
        /// </param>
        /// <param name="timestamp">
        ///     The time when the input occurred.
        /// </param>
        /// <param name="key">
        ///     The key referenced by the event.
        /// </param>
        public KeyEventArgs(KeyboardDevice keyboard, int timestamp, Key key, KeyStates keyStates) : base(keyboard, timestamp)
        {
            if (!Keyboard.IsValidKey(key))
                throw new System.ComponentModel.InvalidEnumArgumentException("key", (int)key, typeof(Key));

            _realKey = key;
            _keyStates = keyStates;
            _isRepeat = false;

            // Start out assuming that this is just a normal key.
            MarkNormal();
        }

        /// <summary>
        ///     The Key referenced by the event, if the key is not being 
        ///     handled specially.
        /// </summary>
        public Key Key
        {
            get { return _key; }
        }

        /// <summary>
        ///     The original key, as opposed to <see cref="Key"/>, which might
        ///     have been changed (e.g. by MarkTextInput).
        /// </summary>
        /// <remarks>
        /// Note:  This should remain internal.  When a processor obfuscates the key,
        /// such as changing characters to Key.TextInput, we're deliberately trying to
        /// hide it and make it hard to find.  But internally we'd like an easy way to find
        /// it.  So we have this internal, but it must remain internal.
        /// </remarks>
        public Key RealKey
        {
            get { return _realKey; }
        }

        /// <summary>
        ///     The Key referenced by the event, if the key is going to be
        ///     processed by an IME.
        /// </summary>
        public Key ImeProcessedKey
        {
            get { return (_key == Key.ImeProcessed) ? _realKey : Key.None; }
        }

        /// <summary>
        ///     The Key referenced by the event, if the key is going to be
        ///     processed by an system.
        /// </summary>
        public Key SystemKey
        {
            get { return (_key == Key.System) ? _realKey : Key.None; }
        }

        /// <summary>
        ///     The Key referenced by the event, if the the key is going to 
        ///     be processed by Win32 Dead Char System.
        /// </summary>
        public Key DeadCharProcessedKey
        {
            get { return (_key == Key.DeadCharProcessed) ? _realKey : Key.None; }
        }

        /// <summary>
        ///     The state of the key referenced by the event.
        /// </summary>
        public KeyStates KeyStates => _keyStates;

        /// <summary>
        ///     Whether the key pressed is a repeated key or not.
        /// </summary>
        public bool IsRepeat
        {
            get { return _isRepeat; }
        }

        /// <summary>
        ///     Whether or not the key referenced by the event is down.
        /// </summary>
        /// <ExternalAPI Inherit="true"/>
        public bool IsDown => _keyStates == KeyStates.Down;

        /// <summary>
        ///     Whether or not the key referenced by the event is up.
        /// </summary>
        /// <ExternalAPI Inherit="true"/>
        public bool IsUp => _keyStates == KeyStates.None;

        /// <summary>
        ///     Whether or not the key referenced by the event is toggled.
        /// </summary>
        /// <ExternalAPI Inherit="true"/>
        public bool IsToggled => _keyStates == KeyStates.Toggled;

        /// <summary>
        ///     The mechanism used to call the type-specific handler on the
        ///     target.
        /// </summary>
        /// <param name="genericHandler">
        ///     The generic handler to call in a type-specific way.
        /// </param>
        /// <param name="genericTarget">
        ///     The target to call the handler on.
        /// </param>
        /// <ExternalAPI/> 
        protected override void InvokeEventHandler(Delegate genericHandler, object genericTarget)
        {
            KeyEventHandler handler = (KeyEventHandler)genericHandler;

            handler(genericTarget, this);
        }

        internal void SetRepeat(bool newRepeatState)
        {
            _isRepeat = newRepeatState;
        }

        internal void MarkNormal()
        {
            _key = _realKey;
        }

        internal void MarkSystem()
        {
            _key = Key.System;
        }

        internal void MarkImeProcessed()
        {
            _key = Key.ImeProcessed;
        }

        internal void MarkDeadCharProcessed()
        {
            _key = Key.DeadCharProcessed;
        }
        internal int ScanCode
        {
            get { return _scanCode; }
            set { _scanCode = value; }
        }

        internal bool IsExtendedKey
        {
            get { return _isExtendedKey; }
            set { _isExtendedKey = value; }
        }


        private Key _realKey;
        private readonly KeyStates _keyStates;
        private Key _key;

        private bool _isRepeat;
        private int _scanCode;
        private bool _isExtendedKey;
    }
}
