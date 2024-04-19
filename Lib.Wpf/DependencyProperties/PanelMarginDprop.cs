using System.Windows;
using System.Windows.Controls;

namespace Lib.Wpf.DependencyProperties
{
    public static class PanelMarginDprop
    {
        public static readonly DependencyProperty MarginProperty =
            DependencyProperty.RegisterAttached(
                "Margin",
                typeof(Thickness),
                typeof(PanelMarginDprop),
                new UIPropertyMetadata(new Thickness(), MarginPropertyChanged));

        private static void MarginPropertyChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = d as Panel;
            if (panel == null) return;
            //panel.Loaded -= new RoutedEventHandler(Panel_Loaded);
            //panel.Loaded += new RoutedEventHandler(Panel_Loaded);
            panel.SizeChanged -= Panel_SizeChanged;
            panel.SizeChanged += Panel_SizeChanged;
            Panel_SizeChanged(panel, null);
        }

        public static Thickness GetMargin(DependencyObject d)
        {
            return (Thickness)d.GetValue(MarginProperty);
        }

        public static void SetMargin(DependencyObject d, Thickness value)
        {
            d.SetValue(MarginProperty, value);
        }

        //private static void Panel_Loaded(object sender, RoutedEventArgs e)
        //{
        //    var panel = sender as Panel;

        //    foreach (var child in panel.Children)
        //    {
        //        var fe = child as FrameworkElement;
        //        if (fe == null) continue;
        //        fe.Margin = GetMargin(panel);
        //    }
        //}

        private static void Panel_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            var panel = sender as Panel;

            foreach (var child in panel.Children)
            {
                var fe = child as FrameworkElement;
                if (fe == null) continue;
                fe.Margin = GetMargin(panel);
            }
        }

    }
}

