using System.Windows;

namespace Lib.Wpf.DependencyProperties
{
    public static class FocusDprop
    {
        // DependencyProperty: 屬性名稱 + Property
        // RegisterAttached: 屬性名稱, 屬性型別, 擁有屬性的類別, 相依屬性的其它設定
        // FrameworkPropertyMetadata: 屬性值改變時的Callback
        public static readonly DependencyProperty IsFocusedProperty =
            DependencyProperty.RegisterAttached(
                "IsFocused",
                typeof(bool?),
                typeof(FocusDprop),
                new FrameworkPropertyMetadata(IsFocusedPropertyChanged)
                { BindsTwoWayByDefault = true });

        private static void IsFocusedPropertyChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var fe = (FrameworkElement)d;

            if (e.OldValue == null)
            {
                fe.GotFocus += FrameworkElement_GotFocus;
                fe.LostFocus += FrameworkElement_LostFocus;
            }

            if (!fe.IsVisible)
                fe.IsVisibleChanged += new DependencyPropertyChangedEventHandler(FrameworkElement_IsVisibleChanged);

            if (e.NewValue != null && (bool)e.NewValue)
                fe.Focus();
        }

        public static bool? GetIsFocused(DependencyObject d)
        {
            return (bool?)d?.GetValue(IsFocusedProperty);
        }

        public static void SetIsFocused(DependencyObject d, bool? value)
        {
            d?.SetValue(IsFocusedProperty, value);
        }

        private static void FrameworkElement_GotFocus(object sender, RoutedEventArgs e) =>
            ((FrameworkElement)sender).SetValue(IsFocusedProperty, true);

        private static void FrameworkElement_LostFocus(object sender, RoutedEventArgs e) =>
            ((FrameworkElement)sender).SetValue(IsFocusedProperty, false);

        private static void FrameworkElement_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            var fe = (FrameworkElement)sender;
            if (fe.IsVisible && (bool)fe.GetValue(IsFocusedProperty))
            {
                fe.IsVisibleChanged -= FrameworkElement_IsVisibleChanged;
                fe.Focus();
            }
        }

    }

    //public static class FocusDprop
    //{
    //    // DependencyProperty: 屬性名稱 + Property
    //    // RegisterAttached: 屬性名稱, 屬性型別, 擁有屬性的類別, 相依屬性的其它設定
    //    // UIPropertyMetadata: 屬性預設值, 屬性值改變時的Callback
    //    public static readonly DependencyProperty IsFocusedProperty =
    //        DependencyProperty.RegisterAttached(
    //            "IsFocused", typeof(bool), typeof(FocusDprop),
    //            new UIPropertyMetadata(false, OnIsFocusedPropertyChanged));

    //    public static bool GetIsFocused(DependencyObject obj)
    //    {
    //        return (bool)obj.GetValue(IsFocusedProperty);
    //    }

    //    public static void SetIsFocused(DependencyObject obj, bool value)
    //    {
    //        obj.SetValue(IsFocusedProperty, value);
    //    }

    //    private static void OnIsFocusedPropertyChanged(
    //        DependencyObject d,
    //        DependencyPropertyChangedEventArgs e)
    //    {
    //        var uie = (UIElement)d;
    //        //if ((bool)e.NewValue)
    //        //    uie.Focus();
    //        if ((bool)e.NewValue && uie.Dispatcher != null)  // don't care about false values.
    //        {
    //            // invoke behaves nicer, if e.g. you have some additional handler attached to 'GotFocus' of uie.
    //            uie.Dispatcher.BeginInvoke(DispatcherPriority.Normal, (Action)(() => uie.Focus()));
    //            // reset bound value if possible, to allow setting again ...
    //            SetIsFocused(uie, false);
    //        }
    //    }
    //}

}
