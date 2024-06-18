using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Wodsoft.UI.Input
{
    public class TextCompositionManager
    {
        public static readonly RoutedEvent PreviewTextInputStartEvent = EventManager.RegisterRoutedEvent("PreviewTextInputStart", RoutingStrategy.Tunnel, typeof(TextCompositionEventHandler), typeof(TextCompositionManager));
        public static void AddPreviewTextInputStartHandler(DependencyObject element, TextCompositionEventHandler handler)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            UIElement.AddHandler(element, PreviewTextInputStartEvent, handler);
        }
        public static void RemovePreviewTextInputStartHandler(DependencyObject element, TextCompositionEventHandler handler)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            UIElement.RemoveHandler(element, PreviewTextInputStartEvent, handler);
        }

        public static readonly RoutedEvent TextInputStartEvent = EventManager.RegisterRoutedEvent("TextInputStart", RoutingStrategy.Bubble, typeof(TextCompositionEventHandler), typeof(TextCompositionManager));
        public static void AddTextInputStartHandler(DependencyObject element, TextCompositionEventHandler handler)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            UIElement.AddHandler(element, TextInputStartEvent, handler);
        }
        public static void RemoveTextInputStartHandler(DependencyObject element, TextCompositionEventHandler handler)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            UIElement.RemoveHandler(element, TextInputStartEvent, handler);
        }

        public static readonly RoutedEvent PreviewTextInputUpdateEvent = EventManager.RegisterRoutedEvent("PreviewTextInputUpdate", RoutingStrategy.Tunnel, typeof(TextCompositionEventHandler), typeof(TextCompositionManager));

        public static void AddPreviewTextInputUpdateHandler(DependencyObject element, TextCompositionEventHandler handler)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            UIElement.AddHandler(element, PreviewTextInputUpdateEvent, handler);
        }
        public static void RemovePreviewTextInputUpdateHandler(DependencyObject element, TextCompositionEventHandler handler)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            UIElement.RemoveHandler(element, PreviewTextInputUpdateEvent, handler);
        }

        public static readonly RoutedEvent TextInputUpdateEvent = EventManager.RegisterRoutedEvent("TextInputUpdate", RoutingStrategy.Bubble, typeof(TextCompositionEventHandler), typeof(TextCompositionManager));
        public static void AddTextInputUpdateHandler(DependencyObject element, TextCompositionEventHandler handler)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            UIElement.AddHandler(element, TextInputUpdateEvent, handler);
        }
        public static void RemoveTextInputUpdateHandler(DependencyObject element, TextCompositionEventHandler handler)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            UIElement.RemoveHandler(element, TextInputUpdateEvent, handler);
        }

        public static readonly RoutedEvent PreviewTextInputEvent = EventManager.RegisterRoutedEvent("PreviewTextInput", RoutingStrategy.Tunnel, typeof(TextCompositionEventHandler), typeof(TextCompositionManager));
        public static void AddPreviewTextInputHandler(DependencyObject element, TextCompositionEventHandler handler)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            UIElement.AddHandler(element, PreviewTextInputEvent, handler);
        }
        public static void RemovePreviewTextInputHandler(DependencyObject element, TextCompositionEventHandler handler)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            UIElement.RemoveHandler(element, PreviewTextInputEvent, handler);
        }

        public static readonly RoutedEvent TextInputEvent = EventManager.RegisterRoutedEvent("TextInput", RoutingStrategy.Bubble, typeof(TextCompositionEventHandler), typeof(TextCompositionManager));
        public static void AddTextInputHandler(DependencyObject element, TextCompositionEventHandler handler)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            UIElement.AddHandler(element, TextInputEvent, handler);
        }
        public static void RemoveTextInputHandler(DependencyObject element, TextCompositionEventHandler handler)
        {
            if (element == null)
                throw new ArgumentNullException("element");
            UIElement.RemoveHandler(element, TextInputEvent, handler);
        }
    }
}
