using Controls;
using Lib.Wpf;
using MahApps.Metro.Controls;
using MaterialDesignThemes.Wpf;
using System;
using System.Globalization;
using System.Speech.Synthesis;
using System.Windows;
using System.Windows.Input;
using ViewModels;

namespace PortalConsole
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : MetroWindow
    {
        public MainWindow()
        {
            InitializeComponent();

            MainViewModel.ShowLoginWindow = ShowLoginWindow;
            MainViewModel.MainPopupBox = MainPopupBox;
            MainViewModel.FuncListViewModel.ShowPageWindow = ShowPageWindow;
            MainViewModel.FuncListViewModel.HidePageWindow = HidePageWindow;
            MainViewModel.FuncListViewModel.MainPopupBox = MainPopupBox;

            this.Loaded += MainWindow_Loaded;
            this.PreviewMouseLeftButtonDown += MainWindow_PreviewMouseLeftButtonDown;
            this.PreviewMouseMove += MainWindow_PreviewMouseMove;
            this.Activated += MainWindow_Activated;
            this.Deactivated += MainWindow_Deactivated;
            this.Topmost = true;

            Global.MainSnackbar = MainSnackbar;
            MainSnackbar.IsActiveChanged += MainSnackbar_IsActiveChanged;
            Global.MainSnackbar.MessageEnqueue("登入成功，可於螢幕左下方開啟系統。",
                durationOverride: TimeSpan.FromSeconds(5));
        }

        private MainViewModel MainViewModel =>
             this.DataContext as MainViewModel;

        /// <summary>
        /// 功能頁籤畫面
        /// </summary>
        private static PageWindow pageWindow;


        private bool? ShowLoginWindow()
        {
            LoginWindow loginWindow = new LoginWindow();
            return loginWindow.ShowDialog();
        }

        private void ShowPageWindow()
        {
            if (pageWindow == null)
            {
                pageWindow = new PageWindow();
                pageWindow.DataContext = MainViewModel.PageViewModel;
                pageWindow.Show();
            }
            else
            {
                if (pageWindow.WindowState == WindowState.Minimized)
                    pageWindow.WindowState = WindowState.Normal;
                pageWindow.Show();
                pageWindow.Activate();
            }
        }

        private void HidePageWindow()
        {
            pageWindow?.Hide();
        }

        #region Snackbar and Voice

        private SpeechSynthesizer _mainSpeech;
        private SpeechSynthesizer MainSpeech
        {
            get
            {
                if (_mainSpeech == null)
                {
                    _mainSpeech = new SpeechSynthesizer();
                    _mainSpeech.SelectVoiceByHints(VoiceGender.Female, VoiceAge.Adult, 0,
                    CultureInfo.GetCultureInfo("zh-TW"));
                    _mainSpeech.Volume = 100;
                }
                return _mainSpeech;
            }
            set => _mainSpeech = value;
        }

        private void MainSnackbar_IsActiveChanged(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            Snackbar snackbar = sender as Snackbar;
            object msgSpeech = snackbar.Message.Content;

            if (snackbar.IsActive && msgSpeech.GetType() == typeof(string))
            {
                this.Activate();
                MainPopupBox.IsPopupOpen = false;
                MainSpeech.SpeakAsyncCancelAll();
                MainSpeech.Volume = 100; //(int)VolumeSlider.Value;
                MainSpeech.SpeakAsync((string)msgSpeech);
            }
            else
                MainSpeech.SpeakAsyncCancelAll();
        }

        #endregion

        #region Window's Position and DragMove

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // setting window's position
            this.WindowStartupLocation = WindowStartupLocation.Manual;
            this.Left = 60;
            this.Top = SystemParameters.WorkArea.Height - ActualHeight;
        }

        private Point startPoint;
        private bool drag;

        private void MainWindow_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            drag = e.OriginalSource == DragTarget;
            startPoint = e.GetPosition(this);
        }

        private void MainWindow_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            try
            {
                Point newPoint = e.GetPosition(this);
                // 以 drag 取代 (e.Source == MainPopupBox && e.OriginalSource.GetType() == typeof(ToggleButton))，因該情境 e.OriginalSource 的取得會造成延遲
                if (drag && e.LeftButton == MouseButtonState.Pressed &&
                    (Math.Abs(newPoint.X - startPoint.X) > SystemParameters.MinimumHorizontalDragDistance ||
                    Math.Abs(newPoint.Y - startPoint.Y) > SystemParameters.MinimumVerticalDragDistance))
                {
                    MainPopupBox.IsPopupOpen = false;
                    this.DragMove();
                }
            }
            catch (Exception) { }
        }

        //private void MainWindow_MouseDown(object sender, MouseButtonEventArgs e)
        //{
        //    if (e.ChangedButton == MouseButton.Left)
        //        this.DragMove();
        //    //if (Mouse.LeftButton == MouseButtonState.Pressed)
        //    //    this.DragMove();
        //}

        #endregion

        #region Window's Opacity

        private void MainWindow_Activated(object sender, EventArgs e)
        {
            this.Opacity = 1;
        }

        private void MainWindow_Deactivated(object sender, EventArgs e)
        {
            this.Opacity = 0.6;
        }

        #endregion

        private void MainPopupBox_PreviewGotKeyboardFocus(object sender, KeyboardFocusChangedEventArgs e) =>
            IMEPopupBox.OnPreviewGotKeyboardFocus(sender, e);

    }
}
