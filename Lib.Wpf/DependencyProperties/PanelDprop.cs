using System.Windows;
using System.Windows.Controls;

namespace Lib.Wpf.DependencyProperties
{
    public static class PanelDprop
    {
        #region Spacing
        // Apply outer thickness (margin) to all children.

        public static readonly DependencyProperty SpacingProperty =
            DependencyProperty.RegisterAttached(
                "Spacing",
                typeof(Thickness),
                typeof(PanelDprop),
                new FrameworkPropertyMetadata(default(Thickness), SpacingPropertyChanged));

        private static void SpacingPropertyChanged(
            DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var panel = sender as Panel;
            if (panel == null) return;
            panel.SizeChanged -= SpacingUpdated;
            panel.SizeChanged += SpacingUpdated;
            SpacingUpdated(panel, null);
        }

        public static void SetSpacing(Panel d, Thickness value)
        {
            d.SetValue(SpacingProperty, value);
        }

        public static Thickness GetSpacing(Panel d)
        {
            return (Thickness)d.GetValue(SpacingProperty);
        }

        private static void SpacingUpdated(object sender, SizeChangedEventArgs e)
        {
            var panel = sender as Panel;
            var s = GetSpacing(panel);
            var tf = GetTrimFirst(panel);
            var tl = GetTrimLast(panel);

            for (int i = 0, Count = panel.Children.Count; i < Count; i++)
            {
                var fe = panel.Children[i] as FrameworkElement;
                if (fe == null) continue;
                if (i == 0 && tf || i == Count - 1 && tl)
                {
                    fe.Margin = new Thickness(0);
                    continue;
                }
                fe.Margin = s;
            }
        }

        #endregion

        #region TrimFirst
        // If spacing is applied and TrimFirst is true, do not apply margin to first element.

        public static readonly DependencyProperty TrimFirstProperty =
            DependencyProperty.RegisterAttached(
                "TrimFirst",
                typeof(bool),
                typeof(PanelDprop),
                new FrameworkPropertyMetadata(false, SpacingPropertyChanged));

        public static void SetTrimFirst(Panel d, bool value)
        {
            d.SetValue(TrimFirstProperty, value);
        }

        public static bool GetTrimFirst(Panel d)
        {
            return (bool)d.GetValue(TrimFirstProperty);
        }

        #endregion

        #region TrimLast
        // If spacing is applied and TrimLast is true, do not apply margin to last element.

        public static readonly DependencyProperty TrimLastProperty =
            DependencyProperty.RegisterAttached(
                "TrimLast",
                typeof(bool),
                typeof(PanelDprop),
                new FrameworkPropertyMetadata(false, SpacingPropertyChanged));

        public static void SetTrimLast(Panel d, bool value)
        {
            d.SetValue(TrimLastProperty, value);
        }

        public static bool GetTrimLast(Panel d)
        {
            return (bool)d.GetValue(TrimLastProperty);
        }

        #endregion

        #region HorizontalContentAlignment
        // Align all children horizontally. (似乎無作用)

        public static readonly DependencyProperty HorizontalContentAlignmentProperty =
            DependencyProperty.RegisterAttached(
                "HorizontalContentAlignment",
                typeof(HorizontalAlignment),
                typeof(PanelDprop),
                new FrameworkPropertyMetadata(HorizontalAlignment.Left,
                HorizontalContentAlignmentPropertyChanged));

        private static void HorizontalContentAlignmentPropertyChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var panel = d as Panel;

            if (panel == null) return;
            panel.SizeChanged -= HorizontalContentAlignmentUpdated;
            panel.SizeChanged += HorizontalContentAlignmentUpdated;
            HorizontalContentAlignmentUpdated(panel, null);
        }

        public static void SetHorizontalContentAlignment(Panel d, HorizontalAlignment value)
        {
            d.SetValue(HorizontalContentAlignmentProperty, value);
        }

        public static HorizontalAlignment GetHorizontalContentAlignment(Panel d)
        {
            return (HorizontalAlignment)d.GetValue(HorizontalContentAlignmentProperty);
        }

        private static void HorizontalContentAlignmentUpdated(object sender, SizeChangedEventArgs e)
        {
            var panel = sender as Panel;
            var a = GetHorizontalContentAlignment(panel);

            foreach (var child in panel.Children)
            {
                var fe = child as FrameworkElement;
                if (fe == null) continue;
                fe.HorizontalAlignment = a;
            }
        }

        #endregion

        #region VerticalContentAlignment
        // Align all children vertically.

        public static readonly DependencyProperty VerticalContentAlignmentProperty =
               DependencyProperty.RegisterAttached(
                    "VerticalContentAlignment",
                    typeof(VerticalAlignment),
                    typeof(PanelDprop),
                    new FrameworkPropertyMetadata(VerticalAlignment.Top,
                    VerticalContentAlignmentPropertyChanged));

        private static void VerticalContentAlignmentPropertyChanged(
            DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var panel = sender as Panel;
            if (panel == null) return;
            panel.SizeChanged -= VerticalContentAlignmentUpdated;
            panel.SizeChanged += VerticalContentAlignmentUpdated;
            VerticalContentAlignmentUpdated(panel, null);
        }

        public static void SetVerticalContentAlignment(Panel d, VerticalAlignment value)
        {
            d.SetValue(VerticalContentAlignmentProperty, value);
        }

        public static VerticalAlignment GetVerticalContentAlignment(Panel d)
        {
            return (VerticalAlignment)d.GetValue(VerticalContentAlignmentProperty);
        }

        private static void VerticalContentAlignmentUpdated(object sender, SizeChangedEventArgs e)
        {
            var panel = sender as Panel;
            var a = GetVerticalContentAlignment(panel);

            foreach (var child in panel.Children)
            {
                var fe = child as FrameworkElement;
                if (fe == null) continue;
                fe.VerticalAlignment = a;
            }
        }

        #endregion

    }
}
