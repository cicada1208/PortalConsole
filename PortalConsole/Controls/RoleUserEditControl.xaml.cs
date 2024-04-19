using System.Windows;
using System.Windows.Controls;

namespace PortalConsole.Controls
{
    /// <summary>
    /// RoleUserEditControl.xaml 的互動邏輯
    /// </summary>
    public partial class RoleUserEditControl : UserControl
    {
        public RoleUserEditControl()
        {
            InitializeComponent();
        }

        private void FilterExpander_Expanded(object sender, RoutedEventArgs e)
        {
            HrUserInfoFilterRow.Height = new GridLength(170, GridUnitType.Auto);
            HrUserInfoListRow.Height = new GridLength(1, GridUnitType.Star);
        }

        private void FilterExpander_Collapsed(object sender, RoutedEventArgs e)
        {
            HrUserInfoFilterRow.Height = new GridLength(30, GridUnitType.Auto);
        }
    }
}
