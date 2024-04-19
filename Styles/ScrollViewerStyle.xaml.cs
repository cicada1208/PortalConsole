using System.Windows.Controls;
using System.Windows.Input;

namespace Styles
{
    public partial class ScrollViewerStyle
    {
        private void ScrollViewerOnPreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scv = (ScrollViewer)sender;
            scv.ScrollToVerticalOffset(scv.VerticalOffset - e.Delta);
            //e.Handled = true;
        }
    }
}
