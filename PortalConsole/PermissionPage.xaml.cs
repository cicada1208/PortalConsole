using System.Windows;
using System.Windows.Controls;

namespace PortalConsole
{
    /// <summary>
    /// PermissionPage.xaml 的互動邏輯
    /// </summary>
    public partial class PermissionPage : Page
    {
        public PermissionPage()
        {
            InitializeComponent();
        }

        private void FilterExpander_Expanded(object sender, RoutedEventArgs e)
        {
            FilterRow.Height = new GridLength(170, GridUnitType.Auto);
        }

        private void FilterExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            FilterRow.Height = new GridLength(30, GridUnitType.Auto);
        }
    }
}
