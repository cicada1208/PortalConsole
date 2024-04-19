using System;
using System.Windows;
using System.Windows.Interactivity;

namespace Lib.Wpf.Triggers
{
    /// <summary>
    /// Trigger which fires on attached event
    /// Ex: ComboBox TextBoxBase.TextChanged
    /// </summary>
    public class RoutedEventTrigger : EventTriggerBase<DependencyObject>
    {
        RoutedEvent _routedEvent;
        public RoutedEvent RoutedEvent
        {
            get { return _routedEvent; }
            set { _routedEvent = value; }
        }

        public RoutedEventTrigger()
        {
        }

        protected override void OnAttached()
        {
            Behavior behavior = base.AssociatedObject as Behavior;
            FrameworkElement associatedElement = base.AssociatedObject as FrameworkElement;

            if (behavior != null)
                associatedElement = ((IAttachedObject)behavior).AssociatedObject as FrameworkElement;

            if (associatedElement == null)
                throw new ArgumentException("Routed Event Trigger can only be associated to framework elements");

            if (RoutedEvent != null)
                associatedElement.AddHandler(RoutedEvent, new RoutedEventHandler(OnRoutedEvent));
        }

        void OnRoutedEvent(object sender, RoutedEventArgs args) =>
            base.OnEvent(args);

        protected override string GetEventName() =>
             RoutedEvent.Name;

    }
}
