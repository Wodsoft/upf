using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using System.Xml.Linq;

namespace Wodsoft.UI.Input
{
    public class RoutedCommand : ICommand
    {
        private string _name;
        private Type? _ownerType;
        private InputGestureCollection? _inputGestureCollection;

        #region Constructors

        /// <summary>
        ///     Default Constructor - needed to allow markup creation
        /// </summary>
        public RoutedCommand()
        {
            _name = string.Empty;
            _ownerType = null;
            _inputGestureCollection = null;
        }

        /// <summary>
        /// RoutedCommand Constructor with Name and OwnerType
        /// </summary>
        /// <param name="name">Declared Name of the RoutedCommand for Serialization</param>
        /// <param name="ownerType">Type that is registering the property</param>
        public RoutedCommand(string name, Type ownerType) : this(name, ownerType, null)
        {
        }

        /// <summary>
        /// RoutedCommand Constructor with Name and OwnerType
        /// </summary>
        /// <param name="name">Declared Name of the RoutedCommand for Serialization</param>
        /// <param name="ownerType">Type that is registering the property</param>
        /// <param name="inputGestures">Default Input Gestures associated</param>
        public RoutedCommand(string name, Type ownerType, InputGestureCollection? inputGestures)
        {
            if (name == null)
            {
                throw new ArgumentNullException("name");
            }

            if (name.Length == 0)
            {
                throw new ArgumentException("Name can't be empty.", "name");
            }

            if (ownerType == null)
            {
                throw new ArgumentNullException("ownerType");
            }

            _name = name;
            _ownerType = ownerType;
            _inputGestureCollection = inputGestures;
        }

        #endregion

        #region Properties


        /// <summary>
        /// Name - Declared time Name of the property/field where it is
        ///              defined, for serialization/debug purposes only.
        ///     Ex: public static RoutedCommand New  { get { new RoutedCommand("New", .... ) } }
        ///          public static RoutedCommand New = new RoutedCommand("New", ... ) ;
        /// </summary>
        public string? Name => _name;

        /// <summary>
        /// Owning type of the property
        /// </summary>
        public Type? OwnerType => _ownerType;

        /// <summary>
        /// Input Gestures associated with RoutedCommand
        /// </summary>
        public InputGestureCollection? InputGestures
        {
            get
            {
                if (InputGesturesInternal == null)
                    _inputGestureCollection = new InputGestureCollection();
                return _inputGestureCollection;
            }
        }

        internal InputGestureCollection? InputGesturesInternal
        {
            get
            {
                if (_inputGestureCollection == null && AreInputGesturesDelayLoaded)
                {
                    _inputGestureCollection = GetInputGestures();
                    AreInputGesturesDelayLoaded = false;
                }
                return _inputGestureCollection;
            }
        }

        public bool AreInputGesturesDelayLoaded { get; set; }

        #endregion

        #region Methods

        /// <summary>
        ///     Executes the command with the given parameter on the given target.
        /// </summary>
        /// <param name="parameter">Parameter to be passed to any command handlers.</param>
        /// <param name="target">Element at which to begin looking for command handlers.</param>
        public void Execute(object? parameter, IInputElement? target)
        {
            // We only support UIElement, ContentElement and UIElement3D
            if (target != null && !InputElement.IsValid(target))
                throw new InvalidOperationException($"Invalid target \"{target.GetType()}\".");

            if (target == null)
                target = FilterInputElement(Keyboard.FocusedElement);

            ExecuteImpl(parameter, target, false);
        }

        /// <summary>
        ///     Whether the command can be executed with the given parameter on the given target.
        /// </summary>
        /// <param name="parameter">Parameter to be passed to any command handlers.</param>
        /// <param name="target">The target element on which to begin looking for command handlers.</param>
        /// <returns>true if the command can be executed, false otherwise.</returns>
        public bool CanExecute(object parameter, IInputElement target)
        {
            bool unused;
            return CriticalCanExecute(parameter, target, false, out unused);
        }

        internal bool CriticalCanExecute(object parameter, IInputElement? target, bool trusted, out bool continueRouting)
        {
            // We only support UIElement, ContentElement and UIElement3D
            if ((target != null) && !InputElement.IsValid(target))
            {
                throw new InvalidOperationException($"Invalid target \"{target.GetType()}\".");
            }

            if (target == null)
            {
                target = FilterInputElement(Keyboard.FocusedElement);
            }

            return CanExecuteImpl(parameter, target, trusted, out continueRouting);
        }

        private static IInputElement? FilterInputElement(IInputElement? elem)
        {
            // We only support UIElement, ContentElement, and UIElement3D
            if (elem != null && InputElement.IsValid(elem))
                return elem;
            return null;
        }

        private bool CanExecuteImpl(object? parameter, IInputElement? target, bool trusted, out bool continueRouting)
        {
            // If blocked by rights-management fall through and return false
            if (target != null)
            {
                // Raise the Preview Event, check the Handled value, and raise the regular event.
                CanExecuteRoutedEventArgs args = new CanExecuteRoutedEventArgs(this, parameter);
                args.RoutedEvent = CommandManager.PreviewCanExecuteEvent;
                CriticalCanExecuteWrapper(parameter, target, trusted, args);
                if (!args.Handled)
                {
                    args.RoutedEvent = CommandManager.CanExecuteEvent;
                    CriticalCanExecuteWrapper(parameter, target, trusted, args);
                }

                continueRouting = args.ContinueRouting;
                return args.CanExecute;
            }
            else
            {
                continueRouting = false;
                return false;
            }
        }

        private void CriticalCanExecuteWrapper(object? parameter, IInputElement target, bool trusted, CanExecuteRoutedEventArgs args)
        {
            // This cast is ok since we are already testing for UIElement, ContentElement, or UIElement3D
            // both of which derive from DO
            DependencyObject targetAsDO = (DependencyObject)target;

            if (targetAsDO is UIElement uie)
            {
                uie.RaiseEvent(args/*, trusted*/);
            }
            else if (targetAsDO is ContentElement ce)
            {
                ce.RaiseEvent(args/*, trusted*/);
            }
        }

        private InputGestureCollection GetInputGestures()
        {
            //if (OwnerType == typeof(ApplicationCommands))
            //{
            //    return ApplicationCommands.LoadDefaultGestureFromResource(_commandId);
            //}
            //else if (OwnerType == typeof(NavigationCommands))
            //{
            //    return NavigationCommands.LoadDefaultGestureFromResource(_commandId);
            //}
            //else if (OwnerType == typeof(MediaCommands))
            //{
            //    return MediaCommands.LoadDefaultGestureFromResource(_commandId);
            //}
            //else if (OwnerType == typeof(ComponentCommands))
            //{
            //    return ComponentCommands.LoadDefaultGestureFromResource(_commandId);
            //}
            return new InputGestureCollection();
        }

        private bool ExecuteImpl(object? parameter, IInputElement? target, bool userInitiated)
        {
            // If blocked by rights-management fall through and return false
            if ((target != null) /*&& !IsBlockedByRM*/)
            {
                UIElement? targetUIElement = target as UIElement;
                ContentElement? targetAsContentElement = null;

                // Raise the Preview Event and check for Handled value, and
                // Raise the regular ExecuteEvent.
                ExecutedRoutedEventArgs args = new ExecutedRoutedEventArgs(this, parameter);
                args.RoutedEvent = CommandManager.PreviewExecutedEvent;

                if (targetUIElement != null)
                {
                    targetUIElement.RaiseEvent(args/*, userInitiated*/);
                }
                else
                {
                    targetAsContentElement = target as ContentElement;
                    if (targetAsContentElement != null)
                    {
                        targetAsContentElement.RaiseEvent(args/*, userInitiated*/);
                    }
                }

                if (!args.Handled)
                {
                    args.RoutedEvent = CommandManager.ExecutedEvent;
                    if (targetUIElement != null)
                    {
                        targetUIElement.RaiseEvent(args/*, userInitiated*/);
                    }
                    else if (targetAsContentElement != null)
                    {
                        targetAsContentElement.RaiseEvent(args/*, userInitiated*/);
                    }
                }

                return args.Handled;
            }

            return false;
        }

        #endregion

        #region ICommand

        /// <summary>
        ///     Executes the command with the given parameter on the currently focused element.
        /// </summary>
        /// <param name="parameter">Parameter to pass to any command handlers.</param>
        void ICommand.Execute(object? parameter)
        {
            Execute(parameter, FilterInputElement(Keyboard.FocusedElement));
        }

        /// <summary>
        ///     Whether the command can be executed with the given parameter on the currently focused element.
        /// </summary>
        /// <param name="parameter">Parameter to pass to any command handlers.</param>
        /// <returns>true if the command can be executed, false otherwise.</returns>
        bool ICommand.CanExecute(object? parameter)
        {
            bool unused;
            return CanExecuteImpl(parameter, FilterInputElement(Keyboard.FocusedElement), false, out unused);
        }

        /// <summary>
        ///     Raised when CanExecute should be requeried on commands.
        ///     Since commands are often global, it will only hold onto the handler as a weak reference.
        ///     Users of this event should keep a strong reference to their event handler to avoid
        ///     it being garbage collected. This can be accomplished by having a private field
        ///     and assigning the handler as the value before or after attaching to this event.
        /// </summary>
        public event EventHandler? CanExecuteChanged;
        //{
        //    add { CommandManager.RequerySuggested += value; }
        //    remove { CommandManager.RequerySuggested -= value; }
        //}

        #endregion
    }

    public class A<T> where T : IA
    {
        public A(IB b) { }
        public A(IC c) { }
        public static void Init() { }
        public void Foo(IC c) { }
        public void Foo(ID d) { }
    }

    public interface IA { }
    public interface IB { }
    public interface IC { }
    public interface ID { }

public class B
{
    public void M(Type type, IC c)
    {
        I__GenericWrapper__1 wrapper = (I__GenericWrapper__1)Activator.CreateInstance(typeof(__GenericWrapper__1<>).MakeGenericType(type));
        wrapper.M1();
        wrapper.M2(c);
        wrapper.M3(c);
    }
    private interface I__GenericWrapper__1
    {
        void M1();
        void M2(IC c);
        void M3(IC c);
    }
    private class __GenericWrapper__1<T> : I__GenericWrapper__1
        where T : IA
    {
        private A<T> _a;

        public void M1()
        {
            A<T>.Init();
        }

        public void M2(IC c)
        {
            _a = new A<T>(c);
        }

        public void M3(IC c)
        {
            _a.Foo(c);
        }
    }
}
}
