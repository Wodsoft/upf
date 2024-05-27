using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Wodsoft.UI.Input;
using Wodsoft.UI.Media;

namespace Wodsoft.UI.Threading
{
    public abstract class UIDispatcher : Dispatcher
    {
        #region Render

        private bool _updateRender;
        private int _continueFrames;

        public void UpdateRender()
        {
            _updateRender = true;
        }

        protected void RunRender()
        {
            if (_updateLayout)
            {
                _updateLayout = false;
                _updateArrangeElements.Clear();
                _updateMeasureElements.Clear();
                InvalidateLayout(RootElement);
                UpdateLayoutCore();
            }
            while (_updateArrangeElements.Count != 0)
            {
                var element = _updateArrangeElements[_updateArrangeElements.Count - 1];
                if (element.IsArrangeValid)
                    _updateArrangeElements.RemoveAt(_updateArrangeElements.Count - 1);
                element.Arrange(element.PreviousArrangeRect);
            }
            while (_updateMeasureElements.Count != 0)
            {
                var element = _updateMeasureElements[_updateMeasureElements.Count - 1];
                if (element.IsMeasureValid)
                    _updateMeasureElements.RemoveAt(_updateMeasureElements.Count - 1);
                element.Measure(element.PreviousAvailableSize);
            }
            if (_updateRender || _continueFrames < 2)
            {
                if (_updateRender)
                {
                    _updateRender = false;
                    _continueFrames = 0;
                }
                else
                    _continueFrames++;
                RunRenderCore();
            }
        }

        protected abstract void RunRenderCore();

        #endregion

        #region Input

        private List<MouseInput> _mouseInputs = new List<MouseInput>();
        private List<MouseInput> _backMouseInputs = new List<MouseInput>();
        public void PushMouseInput(PresentationSource presentationSource, int messageTime, MouseActions actions, MouseButton? button, int x, int y, int wheel)
        {
            var inputs = _mouseInputs;
            lock (inputs)
            {
                if (inputs.Count != 0 && actions == MouseActions.Move)
                {
                    ref var lastInput = ref CollectionsMarshal.AsSpan(inputs)[inputs.Count - 1];
                    if (lastInput.Actions == MouseActions.Move)
                    {
                        lastInput.Point = new Int32Point(x, y);
                        return;
                    }
                }
                inputs.Add(new MouseInput
                {
                    Source = presentationSource,
                    MessageTime = messageTime,
                    Actions = actions,
                    Button = button,
                    Point = new Int32Point(x, y),
                    Wheel = wheel
                });
            }
        }

        private List<KeyboardInput> _keyboardInputs = new List<KeyboardInput>();
        private List<KeyboardInput> _backKeyboardInputs = new List<KeyboardInput>();
        public void PushKeyboardInput(int messageTime, Key key, KeyStates keyStates)
        {
            var inputs = _keyboardInputs;
            lock (inputs)
            {
                inputs.Add(new KeyboardInput
                {
                    MessageTime = messageTime,
                    Key = key,
                    KeyStates = keyStates
                });
            }
        }

        protected void RunInput()
        {
            _backMouseInputs = Interlocked.Exchange(ref _mouseInputs, _backMouseInputs);
            bool hasMouseInput = _backMouseInputs.Count != 0;
            if (hasMouseInput)
            {
                var count = _backMouseInputs.Count;
                var inputs = CollectionsMarshal.AsSpan(_backMouseInputs);
                for (int i = 0; i < count; i++)
                {
                    ref var input = ref inputs[i];
                    MouseDevice.HandleInput(input);
                }
                _backMouseInputs.Clear();
            }
            _backKeyboardInputs = Interlocked.Exchange(ref _keyboardInputs, _backKeyboardInputs);
            bool hasKeyboardInput = _backKeyboardInputs.Count != 0;
            if (hasKeyboardInput)
            {
                var count = _backKeyboardInputs.Count;
                var inputs = CollectionsMarshal.AsSpan(_backKeyboardInputs);
                for (int i = 0; i < count; i++)
                {
                    ref var input = ref inputs[i];
                    KeyboardDevice.HandleInput(input);
                }
                _backKeyboardInputs.Clear();
            }
            RunInputCore(hasMouseInput, hasKeyboardInput);
        }

        protected virtual void RunInputCore(bool hasMouseInput, bool hasKeyboardInput)
        {

        }

        #endregion

        #region Layout

        private bool _updateLayout;
        private List<UIElement> _updateArrangeElements = new List<UIElement>(), _updateMeasureElements = new List<UIElement>();

        protected abstract UIElement RootElement { get; }

        internal void UpdateLayout()
        {
            _updateLayout = true;
        }

        protected abstract void UpdateLayoutCore();

        private void InvalidateLayout(Visual visual)
        {
            if (visual is UIElement element)
            {
                element.InvalidateMeasure();
                element.InvalidateArrange();
            }
            var childrenCount = visual.VisualChildrenCount;
            for (int i = 0; i < childrenCount; i++)
            {
                var child = visual.GetVisualChild(i);
                if (child != null)
                    InvalidateLayout(child);
            }
        }

        internal void UpdateArrange(UIElement element)
        {
            Visual visual = element;
            while (true)
            {
                if (visual is UIElement ue && _updateArrangeElements.Contains(ue))
                    return;
                if (visual.VisualParent == null)
                    break;
                visual = visual.VisualParent;
            }
            _updateArrangeElements.Add(element);
        }

        internal void RemoveArrange(UIElement element)
        {
            _updateArrangeElements.Remove(element);
        }

        internal void UpdateMeasure(UIElement element)
        {
            Visual visual = element;
            while (true)
            {
                if (visual is UIElement ue && _updateMeasureElements.Contains(ue))
                    return;
                if (visual.VisualParent == null)
                    break;
                visual = visual.VisualParent;
            }
            _updateMeasureElements.Add(element);
        }

        internal void RemoveMeasure(UIElement element)
        {
            _updateMeasureElements.Remove(element);
        }

        #endregion

        #region Device

        protected internal abstract MouseDevice MouseDevice { get; }

        protected internal abstract KeyboardDevice KeyboardDevice { get; }

        #endregion

        #region Focus

        private IInputElement? _focusElement;
        internal void SetFocus(IInputElement? element)
        {
            var oldElement = _focusElement;
            _focusElement = element;
            if (oldElement != null && oldElement is DependencyObject)
                ((DependencyObject)oldElement).SetValue(UIElement.IsFocusedPropertyKey, false);
            if (element != null && element is DependencyObject)
                ((DependencyObject)element).SetValue(UIElement.IsFocusedPropertyKey, true);
        }

        #endregion
    }
}
