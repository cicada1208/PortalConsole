using System.Windows;
using System.Windows.Controls;

namespace Lib.Wpf.DependencyProperties
{
    public static class ScrollDprop
    {
        //public static readonly DependencyProperty ScrollToTopProperty =
        //    DependencyProperty.RegisterAttached(
        //        "ScrollToTop",
        //        typeof(bool),
        //        typeof(ScrollDprop),
        //        new PropertyMetadata(false, ScrollToTopPropertyChanged));

        public static readonly DependencyProperty ScrollToTopProperty =
            DependencyProperty.RegisterAttached(
                "ScrollToTop",
                typeof(bool),
                typeof(ScrollDprop),
                new FrameworkPropertyMetadata(false, ScrollToTopPropertyChanged)
                { BindsTwoWayByDefault = true });

        private static void ScrollToTopPropertyChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var scrollViewer = d as ScrollViewer;
            if (scrollViewer == null) return;
            scrollViewer.ScrollChanged -= ScrollViewer_ScrollChanged;
            scrollViewer.ScrollChanged += ScrollViewer_ScrollChanged;

            if ((bool)e.NewValue)
            {
                scrollViewer.ScrollToTop();
                //SetScrollToTop(d, false); // 在 ScrollToTopPropertyChanged 無作用
            }
        }

        public static bool GetScrollToTop(DependencyObject d)
        {
            return (bool)d.GetValue(ScrollToTopProperty);
        }

        public static void SetScrollToTop(DependencyObject d, bool value)
        {
            d.SetValue(ScrollToTopProperty, value);
        }

        private static void ScrollViewer_ScrollChanged(object sender, ScrollChangedEventArgs e)
        {
            var scrollViewer = sender as ScrollViewer;
            if (scrollViewer == null) return;
            if (GetScrollToTop(scrollViewer) && scrollViewer.VerticalOffset != 0)
                SetScrollToTop(scrollViewer, false);
        }

    }
}
