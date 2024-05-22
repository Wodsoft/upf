using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Wodsoft.UI.Threading;

namespace Wodsoft.UI.Input
{
    public static class FocusManager
    {
        #region Public Events

        /// <summary>
        ///     GotFocus event
        /// </summary>
        public static readonly RoutedEvent GotFocusEvent = EventManager.RegisterRoutedEvent("GotFocus", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FocusManager));

        /// <summary>
        ///     Adds a handler for the GotFocus attached event
        /// </summary>
        /// <param name="element">UIElement or ContentElement that listens to this event</param>
        /// <param name="handler">Event Handler to be added</param>
        public static void AddGotFocusHandler(DependencyObject element, RoutedEventHandler handler)
        {
            UIElement.AddHandler(element, GotFocusEvent, handler);
        }

        /// <summary>
        ///     Removes a handler for the GotFocus attached event
        /// </summary>
        /// <param name="element">UIElement or ContentElement that listens to this event</param>
        /// <param name="handler">Event Handler to be removed</param>
        public static void RemoveGotFocusHandler(DependencyObject element, RoutedEventHandler handler)
        {
            UIElement.RemoveHandler(element, GotFocusEvent, handler);
        }

        /// <summary>
        ///     LostFocus event
        /// </summary>
        public static readonly RoutedEvent LostFocusEvent = EventManager.RegisterRoutedEvent("LostFocus", RoutingStrategy.Bubble, typeof(RoutedEventHandler), typeof(FocusManager));

        /// <summary>
        ///     Adds a handler for the LostFocus attached event
        /// </summary>
        /// <param name="element">UIElement or ContentElement that listens to this event</param>
        /// <param name="handler">Event Handler to be added</param>
        public static void AddLostFocusHandler(DependencyObject element, RoutedEventHandler handler)
        {
            UIElement.AddHandler(element, LostFocusEvent, handler);
        }

        /// <summary>
        ///     Removes a handler for the LostFocus attached event
        /// </summary>
        /// <param name="element">UIElement or ContentElement that listens to this event</param>
        /// <param name="handler">Event Handler to be removed</param>
        public static void RemoveLostFocusHandler(DependencyObject element, RoutedEventHandler handler)
        {
            UIElement.RemoveHandler(element, LostFocusEvent, handler);
        }

        #endregion Public Events

        //#region Properties


        //public static readonly DependencyProperty FocusedElementProperty =
        //        DependencyProperty.RegisterAttached(
        //                "FocusedElement",
        //                typeof(IInputElement),
        //                typeof(FocusManager),
        //                new PropertyMetadata(new PropertyChangedCallback(OnFocusedElementChanged)));
        //private static void OnFocusedElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    IInputElement? newFocusedElement = (IInputElement?)e.NewValue;
        //    DependencyObject? oldVisual = (DependencyObject?)e.OldValue;
        //    DependencyObject? newVisual = (DependencyObject?)e.NewValue;

        //    if (oldVisual != null)
        //    {
        //        oldVisual.ClearValue(UIElement.IsFocusedPropertyKey);
        //    }

        //    if (newVisual != null)
        //    {
        //        // set IsFocused on the element.  The element may redirect Keyboard focus
        //        // in response to this (e.g. Editable ComboBox redirects to the
        //        // child TextBox), so detect whether this happens.
        //        DependencyObject oldFocus = Keyboard.FocusedElement as DependencyObject;
        //        newVisual.SetValue(UIElement.IsFocusedPropertyKey, BooleanBoxes.TrueBox);
        //        DependencyObject newFocus = Keyboard.FocusedElement as DependencyObject;

        //        // set the Keyboard focus to the new element, provided that
        //        //  a) the element didn't already set Keyboard focus
        //        //  b) Keyboard focus is not already on the new element
        //        //  c) the new element is within the same focus scope as the current
        //        //      holder (if any) of Keyboard focus
        //        if (oldFocus == newFocus && newVisual != newFocus &&
        //                (newFocus == null || GetRoot(newVisual) == GetRoot(newFocus)))
        //        {
        //            Keyboard.Focus(newFocusedElement);
        //        }
        //    }
        //}

        //#endregion
    }
}
