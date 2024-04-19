using Lib.Wpf;
using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using System.ComponentModel;
using System.Globalization;
using System.Speech.Synthesis;
using System.Windows;
using ViewModels;

namespace PortalConsole
{
    /// <summary>
    /// PageWindow.xaml 的互動邏輯
    /// </summary>
    public partial class PageWindow : MetroWindow
    {
        public PageWindow()
        {
            InitializeComponent();

            SizeChanged += PageWindow_SizeChanged;

            Global.PageSnackbar = PageSnackbar;
            PageSnackbar.IsActiveChanged += PageSnackbar_IsActiveChanged;
        }

        private PageViewModel PageViewModel =>
             this.DataContext as PageViewModel;


        #region Snackbar and Voice

        private SpeechSynthesizer _pageSpeech;
        private SpeechSynthesizer PageSpeech
        {
            get
            {
                if (_pageSpeech == null)
                {
                    _pageSpeech = new SpeechSynthesizer();
                    _pageSpeech.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult, 0,
                    CultureInfo.GetCultureInfo("zh-TW"));
                    _pageSpeech.Volume = 100;
                }
                return _pageSpeech;
            }
            set => _pageSpeech = value;
        }

        private void PageSnackbar_IsActiveChanged(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            Snackbar snackbar = sender as Snackbar;
            object msgSpeech = snackbar.Message.Content;

            if (snackbar.IsActive && msgSpeech.GetType() == typeof(string))
            {
                PageSpeech.SpeakAsyncCancelAll();
                PageSpeech.Volume = 100; //(int)VolumeSlider.Value;
                PageSpeech.SpeakAsync((string)msgSpeech);
            }
            else
                PageSpeech.SpeakAsyncCancelAll();
        }

        #endregion

        #region Zoom Content

        private void ZoomButton_Click(object sender, RoutedEventArgs e)
        {
            this.ZoomPopup.IsOpen = true;
            this.ZoomSlider.Focus();
        }

        private void ZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e) =>
            ZoomView();

        private void PageWindow_SizeChanged(object sender, SizeChangedEventArgs e) =>
            ZoomView();

        private void ZoomView()
        {
            double zoom = (this.ZoomSlider.Value - 100) * 7;
            double widthOffset, heightOffset;
            double width, height;

            if (this.WindowState == WindowState.Maximized)
            {
                widthOffset = 15;
                heightOffset = 45;
            }
            else
            {
                widthOffset = 0;
                heightOffset = 30; // 視窗title
            }

            width = this.ActualWidth - widthOffset - zoom;
            width = width < 0 ? 0 : width;
            height = (this.ActualHeight - heightOffset) * (this.ActualWidth - widthOffset - zoom) / (this.ActualWidth - widthOffset);
            height = height < 0 ? 0 : height;
            this.MainDockPanel.Width = width;
            this.MainDockPanel.Height = height;
        }

        #endregion

        protected override void OnClosing(CancelEventArgs e)
        {
            PageViewModel?.FuncListViewModel?.ContentFuncList.Clear();
            e.Cancel = true; // cancels  the window close
            this.Hide(); // hides the window
        }

    }
}
