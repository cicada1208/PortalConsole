using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace PortalConsole
{
    /// <summary>
    /// TestEventPage.xaml 的互動邏輯
    /// </summary>
    public partial class TestEventPage : Page
    {
        public TestEventPage()
        {
            InitializeComponent();

            // the 1st way: add event handler 
            GroupBox1.PreviewMouseDown += MouseEventHandler;
            GroupBox1.MouseDown += MouseEventHandler;
            StackPanel1.PreviewMouseDown += MouseEventHandler;
            StackPanel1.MouseDown += MouseEventHandler;
            Label1.PreviewMouseDown += MouseEventHandler;
            Label1.MouseDown += MouseEventHandler;
            Button1.PreviewMouseDown += MouseEventHandler;
            Button1.MouseDown += MouseEventHandler;
            Button1.Click += Button_ClickEventHandler;

            // the 2nd way: add event handler 
            //GroupBox1.AddHandler(PreviewMouseDownEvent, new MouseButtonEventHandler(MouseEventHandler));
            //GroupBox1.AddHandler(MouseDownEvent, new MouseButtonEventHandler(MouseEventHandler)); // true
            //StackPanel1.AddHandler(PreviewMouseDownEvent, new MouseButtonEventHandler(MouseEventHandler));
            //StackPanel1.AddHandler(MouseDownEvent, new MouseButtonEventHandler(MouseEventHandler)); // true
            //Label1.AddHandler(PreviewMouseDownEvent, new MouseButtonEventHandler(MouseEventHandler));
            //Label1.AddHandler(MouseDownEvent, new MouseButtonEventHandler(MouseEventHandler));
            //Button1.AddHandler(PreviewMouseDownEvent, new MouseButtonEventHandler(MouseEventHandler));
            //Button1.AddHandler(MouseDownEvent, new MouseButtonEventHandler(MouseEventHandler)); // true
            //Button1.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(Button_ClickEventHandler));

            // more mouse event handler
            //GroupBox1.AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(MouseEventHandler));
            //GroupBox1.AddHandler(PreviewMouseDownEvent, new MouseButtonEventHandler(MouseEventHandler));
            //GroupBox1.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(MouseEventHandler));
            //GroupBox1.AddHandler(MouseDownEvent, new MouseButtonEventHandler(MouseEventHandler));
            //GroupBox1.AddHandler(PreviewMouseLeftButtonUpEvent, new MouseButtonEventHandler(MouseEventHandler));
            //GroupBox1.AddHandler(PreviewMouseUpEvent, new MouseButtonEventHandler(MouseEventHandler));
            //GroupBox1.AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(MouseEventHandler));
            //GroupBox1.AddHandler(MouseUpEvent, new MouseButtonEventHandler(MouseEventHandler));
            //StackPanel1.AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(MouseEventHandler));
            //StackPanel1.AddHandler(PreviewMouseDownEvent, new MouseButtonEventHandler(MouseEventHandler));
            //StackPanel1.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(MouseEventHandler));
            //StackPanel1.AddHandler(MouseDownEvent, new MouseButtonEventHandler(MouseEventHandler));
            //StackPanel1.AddHandler(PreviewMouseLeftButtonUpEvent, new MouseButtonEventHandler(MouseEventHandler));
            //StackPanel1.AddHandler(PreviewMouseUpEvent, new MouseButtonEventHandler(MouseEventHandler));
            //StackPanel1.AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(MouseEventHandler));
            //StackPanel1.AddHandler(MouseUpEvent, new MouseButtonEventHandler(MouseEventHandler));
            //Label1.AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(MouseEventHandler));
            //Label1.AddHandler(PreviewMouseDownEvent, new MouseButtonEventHandler(MouseEventHandler));
            //Label1.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(MouseEventHandler));
            //Label1.AddHandler(MouseDownEvent, new MouseButtonEventHandler(MouseEventHandler));
            //Label1.AddHandler(PreviewMouseLeftButtonUpEvent, new MouseButtonEventHandler(MouseEventHandler));
            //Label1.AddHandler(PreviewMouseUpEvent, new MouseButtonEventHandler(MouseEventHandler));
            //Label1.AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(MouseEventHandler));
            //Label1.AddHandler(MouseUpEvent, new MouseButtonEventHandler(MouseEventHandler));
            //Button1.AddHandler(PreviewMouseLeftButtonDownEvent, new MouseButtonEventHandler(MouseEventHandler));
            //Button1.AddHandler(PreviewMouseDownEvent, new MouseButtonEventHandler(MouseEventHandler));
            //Button1.AddHandler(MouseLeftButtonDownEvent, new MouseButtonEventHandler(MouseEventHandler));
            //Button1.AddHandler(MouseDownEvent, new MouseButtonEventHandler(MouseEventHandler));
            //Button1.AddHandler(PreviewMouseLeftButtonUpEvent, new MouseButtonEventHandler(MouseEventHandler));
            //Button1.AddHandler(PreviewMouseUpEvent, new MouseButtonEventHandler(MouseEventHandler));
            //Button1.AddHandler(ButtonBase.ClickEvent, new RoutedEventHandler(Button_ClickEventHandler));
            //Button1.AddHandler(MouseLeftButtonUpEvent, new MouseButtonEventHandler(MouseEventHandler));
            //Button1.AddHandler(MouseUpEvent, new MouseButtonEventHandler(MouseEventHandler));
        }

        private void Log(string eventName, string senderControlName, string sourceControlName)
        {
            Console.WriteLine($"eventName: {eventName}, senderControlName: {senderControlName}, sourceControlName: {sourceControlName}");
        }

        private void MouseEventHandler(object sender, MouseButtonEventArgs e)
        {
            Log(e.RoutedEvent.ToString(), (sender as FrameworkElement).Name, (e.Source as FrameworkElement).Name);
        }

        private void Button_ClickEventHandler(object sender, RoutedEventArgs e)
        {
            // 按鈕路由事件的特殊性(點擊 Button)：The ButtonBase marks the MouseLeftButtonDown event as handled in the OnMouseLeftButtonDown method and raises the Click event.
            Log(e.RoutedEvent.ToString(), (sender as FrameworkElement).Name, (e.Source as FrameworkElement).Name);
        }

    }
}
