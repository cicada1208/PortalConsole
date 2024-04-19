using System.Windows;
using System.Windows.Controls;

namespace PortalConsole.Controls
{
    /// <summary>
    /// SysCatalogAddControl.xaml 的互動邏輯
    /// </summary>
    public partial class SysCatalogAddControl : UserControl
    {
        public SysCatalogAddControl()
        {
            InitializeComponent();
        }

        private void FilterExpander_Expanded(object sender, RoutedEventArgs e)
        {
            FilterRow.Height = new GridLength(170, GridUnitType.Auto);
            ListRow.Height = new GridLength(1, GridUnitType.Star);
        }

        private void FilterExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            FilterRow.Height = new GridLength(30, GridUnitType.Auto);
        }
    }
}
