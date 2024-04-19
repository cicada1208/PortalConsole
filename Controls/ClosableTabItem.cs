using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Controls
{
    public class ClosableTabItem : TabItem
    {
        public ClosableTabItem()
        {
            // Create an instance of the usercontrol
            ClosableTabHeader closableTabHeader = new ClosableTabHeader();
            // Assign the usercontrol to the tab header
            // This means that when the TabItem is displayed, it will show our new UserControl on the header.
            this.Header = closableTabHeader;

            closableTabHeader.CloseButton.MouseEnter +=
               new MouseEventHandler(CloseButton_MouseEnter);
            closableTabHeader.CloseButton.MouseLeave +=
               new MouseEventHandler(CloseButton_MouseLeave);
            closableTabHeader.CloseButton.Click +=
               new RoutedEventHandler(CloseButton_Click);
            //closableTabHeader.TitleLabel.SizeChanged +=
            //   new SizeChangedEventHandler(TitleLabel_SizeChanged);
        }

        private ClosableTabHeader ClosableTabHeader => this.Header as ClosableTabHeader;

        /// <summary>
        /// Set the title of the tab
        /// </summary>
        public string Title
        {
            set => ClosableTabHeader.TitleLabel.Content = value;
        }

        /// <summary>
        /// When the tab is selected, show the close button
        /// </summary>
        protected override void OnSelected(RoutedEventArgs e)
        {
            base.OnSelected(e);
            ClosableTabHeader.CloseButton.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// When the tab is deselected, hide the close button
        /// </summary>
        protected override void OnUnselected(RoutedEventArgs e)
        {
            base.OnUnselected(e);
            ClosableTabHeader.CloseButton.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// When the mouse is over a tab, show the close button
        /// </summary>
        protected override void OnMouseEnter(MouseEventArgs e)
        {
            base.OnMouseEnter(e);
            ClosableTabHeader.CloseButton.Visibility = Visibility.Visible;
        }

        /// <summary>
        /// When the mouse is not over a tab, hide the close button
        /// </summary>
        protected override void OnMouseLeave(MouseEventArgs e)
        {
            base.OnMouseLeave(e);
            if (!this.IsSelected)
                ClosableTabHeader.CloseButton.Visibility = Visibility.Hidden;
        }

        /// <summary>
        /// When the mouse is over the button, change color to Red
        /// </summary>
        void CloseButton_MouseEnter(object sender, MouseEventArgs e)
        {
            ClosableTabHeader.CloseButton.Foreground = Brushes.Red;
        }

        /// <summary>
        /// When the mouse is no longer over button, change color back to black
        /// </summary>
        void CloseButton_MouseLeave(object sender, MouseEventArgs e)
        {
            ClosableTabHeader.CloseButton.Foreground = Brushes.Black;
        }

        /// <summary>
        /// Button close click, remove the tab
        /// (or raise an event indicating a "CloseTab" event has occurred)
        /// </summary>
        void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            ((TabControl)this.Parent).Items.Remove(this);
        }

        ///// <summary>
        ///// Label SizeChanged, when the size of the label changes
        ///// (due to setting the Title) set position of button properly
        ///// </summary>
        //void TitleLabel_SizeChanged(object sender, SizeChangedEventArgs e)
        //{
        //    ClosableTabHeader.CloseButton.Margin = new Thickness(
        //    ClosableTabHeader.TitleLabel.ActualWidth + 5, 3, 4, 0);
        //}

    }
}
