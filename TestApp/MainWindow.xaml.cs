using System;
using System.Windows;

namespace TestApp
{
    /// <summary>
    /// MainWindow.xaml 的互動邏輯
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            string cychUserId = Environment.GetEnvironmentVariable("CychUserId", EnvironmentVariableTarget.Process);
            EnvironmentTextBlock.Text = $"CychUserId：{cychUserId}";
        }

    }
}
