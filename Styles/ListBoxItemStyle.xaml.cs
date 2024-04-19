using System.Windows.Controls;
using System.Windows.Input;

namespace Styles
{
    public partial class ListBoxItemStyle
    {
        private void ListBoxItemSelectedOnPreviewMouseDown(object sender, MouseButtonEventArgs e) =>
            ((ListBoxItem)sender).IsSelected = true;
    }
}
