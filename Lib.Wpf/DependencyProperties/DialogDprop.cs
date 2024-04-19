using System.Windows;

namespace Lib.Wpf.DependencyProperties
{
    public static class DialogDprop
    {
        public static readonly DependencyProperty DialogResultProperty =
            DependencyProperty.RegisterAttached(
                "DialogResult",
                typeof(bool?),
                typeof(DialogDprop),
                new PropertyMetadata(DialogResultPropertyChanged));

        private static void DialogResultPropertyChanged(
            DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var window = d as Window;
            if (window != null)
                window.DialogResult = e.NewValue as bool?;
        }

        public static void SetDialogResult(Window target, bool? value)
        {
            target.SetValue(DialogResultProperty, value);
        }

    }
}
