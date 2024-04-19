using System.Windows.Controls;
using System.Windows.Input;

namespace Styles
{
    public partial class TreeViewItemStyle
    {
        private void TreeViewItemSelectedOnPreviewMouseDown(object sender, MouseButtonEventArgs e) =>
            ((TreeViewItem)sender).IsSelected = true;
    }
}
