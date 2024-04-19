using MaterialDesignThemes.Wpf;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;

namespace Lib.Wpf
{
    public class CtrlUtil
    {
        /// <summary>
        /// 鍵盤鍵入Enter將焦點移至另一控制項
        /// </summary>
        public static bool KeyEnterMoveFocus(KeyEventArgs e,
            FocusNavigationDirection focusDirection = FocusNavigationDirection.Next)
        {
            bool moveFocusSucc = false;
            if (e.Key != Key.Enter) return moveFocusSucc;
            TraversalRequest request = new TraversalRequest(focusDirection);
            UIElement focusElement = Keyboard.FocusedElement as UIElement;
            moveFocusSucc = focusElement?.MoveFocus(request) ?? false;
            return moveFocusSucc;
        }

        /// <summary>
        /// 設定 ToolTip 依然顯示，即使元件 IsEnabled = false
        /// </summary>
        public static void SetToolTipShowOnDisabled()
        {
            ToolTipService.ShowOnDisabledProperty.OverrideMetadata(
            typeof(FrameworkElement),
            new FrameworkPropertyMetadata(true));
        }

        /// <summary>
        /// 設定 ToolTip ShowDuration
        /// </summary>
        public static void SetToolTipShowDuration(int millisecond)
        {
            ToolTipService.ShowDurationProperty.OverrideMetadata(
            typeof(DependencyObject),
            new FrameworkPropertyMetadata(millisecond));
        }

        /// <summary>
        /// 取得 Content Text
        /// </summary>
        /// <typeparam name="T">ContentPresenter 中尋找的類型</typeparam>
        /// <param name="content">Ex: ContentPresenter</param>
        /// <returns></returns>
        public string GetContentText<T>(FrameworkElement content)
            where T : FrameworkElement
        {
            string text = string.Empty;

            try
            {
                //if (cellContent.GetType() == typeof(ContentPresenter)) 
                if (content is ContentPresenter)
                    text = (FindContentFirstChild<T>(content) as dynamic)?.Text ?? string.Empty;
                else
                    text = (content as dynamic)?.Text ?? string.Empty;
            }
            catch (Exception) { }

            return text;
        }

        /// <summary>
        /// 尋找 Content 中第一個某類型的 FrameworkElement
        /// </summary>
        /// <typeparam name="T">尋找類型</typeparam>
        /// <param name="content">Ex: ContentPresenter</param>
        /// <returns></returns>
        public T FindContentFirstChild<T>(FrameworkElement content)
            where T : FrameworkElement
        {
            int childrenCount = VisualTreeHelper.GetChildrenCount(content);
            var children = new FrameworkElement[childrenCount];

            for (int i = 0; i < childrenCount; i++)
            {
                var child = VisualTreeHelper.GetChild(content, i) as FrameworkElement;
                children[i] = child;
                if (child is T)
                    return (T)child;
            }

            for (int i = 0; i < childrenCount; i++)
                if (children[i] != null)
                {
                    var subChild = FindContentFirstChild<T>(children[i]);
                    if (subChild != null)
                        return subChild;
                }

            return null;
        }

        /// <summary>
        /// 定義 DataGrid columns
        /// </summary>
        /// <typeparam name="T">data source type</typeparam>
        /// <param name="dataGrid">DataGrid</param>
        /// <param name="cols">定義的欄位</param>
        public void DataGridDefineCols<T>(DataGrid dataGrid, HashSet<string> cols)
            where T : class
        {
            dataGrid.Columns.Clear();
            System.Windows.Controls.DataGridTextColumn txtCol;
            foreach (var col in cols)
            {
                txtCol = new System.Windows.Controls.DataGridTextColumn
                {
                    Header = ModelUtil.GetPropertyDisplayName<T>(col),
                    Binding = new Binding(col)
                };
                dataGrid.Columns.Add(txtCol);
            }
        }

        /// <summary>
        /// 取得 Window 實體
        /// </summary>
        /// <param name="typeName">e.g.
        /// <para>typeof(Controls.PdsRecdWindow).FullName</para>
        /// <para>typeof(PdsWpfApp.LoginWindow).FullName</para>
        /// </param>
        /// <param name="assemblyFile">e.g. Controls.dll</param>
        public Window GetWindow(string typeName, string assemblyFile = "", string winTitile = "")
        {
            Window win = null;
            CommonUtil commonUtil = new CommonUtil();

            try
            {
                Type targetType = commonUtil.GetTypeFromAssembly(typeName, assemblyFile);

                if (targetType != null)
                {
                    win = Activator.CreateInstance(targetType) as Window;
                    if ((!winTitile.IsNullOrWhiteSpace()) && win != null) win.Title = winTitile;
                }
            }
            catch (Exception) { }

            return win;
        }

        /// <summary>
        /// 將文字內容加入至插入符號
        /// </summary>
        public void InsertTextAtCaret(TextBox textBox, string text)
        {
            text = text.NullableToStr();
            int insertStrLen = text.Length;
            int newCaretIndex = textBox.CaretIndex + insertStrLen;

            //  to insert text at the caret position
            textBox.Text = textBox.Text.Insert(textBox.CaretIndex, text);
            textBox.CaretIndex = newCaretIndex;

            // to scroll the textbox to the caret position
            int lineIndex = textBox.GetLineIndexFromCharacterIndex(textBox.CaretIndex);
            textBox.ScrollToLine(lineIndex);

            textBox.Focus();

            //// to replace the selected text with new text:
            //textBox.SelectedText = "<new text>";
        }

    }

    public static class CtrlExUtil
    {
        /// <summary>
        /// Queues a notification message for display in a snackbar.
        /// </summary>
        /// <param name="content">Message.</param>
        /// <param name="actionContent">Content for the action button.</param>
        /// <param name="actionHandler"> Call back to be executed if user clicks the action button.</param>
        /// <param name="actionArgument">Argument to pass to actionHandler.</param>
        /// <param name="promote"> The message will promoted to the front of the queue.</param>
        /// <param name="neverConsiderToBeDuplicate">The message will never be considered a duplicate.</param>
        /// <param name="durationOverride">Message show duration override.</param>
        /// <param name="clear">Clear the message queue and close the active snackbar.</param>
        /// <param name="activateWindow">Activate window when WindowState is Minimized</param>
        public static void MessageEnqueue<TArgument>(this Snackbar snackbar, object content,
            object actionContent = null, Action<TArgument> actionHandler = null, TArgument actionArgument = default,
            bool promote = false, bool neverConsiderToBeDuplicate = true, TimeSpan? durationOverride = null,
            bool clear = true, bool activateWindow = true)
        {
            if (actionContent == null) actionContent = "Close";
            if (actionHandler == null) actionHandler = (param) => { };

            // The Dispatcher is just a way to execute code on the UI thread. 
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (activateWindow && snackbar != null)
                {
                    var window = Window.GetWindow(snackbar);
                    if (window != null && window.WindowState == WindowState.Minimized)
                    {
                        window.WindowState = WindowState.Normal;
                        window.Activate();
                    }
                }

                if (clear) snackbar?.MessageQueue?.Clear();
                snackbar?.MessageQueue?.Enqueue(content,
                actionContent, actionHandler, actionArgument,
                promote, neverConsiderToBeDuplicate, durationOverride);
            });
        }

        /// <summary>
        /// Queues a notification message for display in a snackbar.
        /// </summary>
        /// <param name="content">Message.</param>
        /// <param name="actionContent">Content for the action button.</param>
        /// <param name="actionHandler"> Call back to be executed if user clicks the action button.</param>
        /// <param name="actionArgument">Argument to pass to actionHandler.</param>
        /// <param name="promote"> The message will promoted to the front of the queue.</param>
        /// <param name="neverConsiderToBeDuplicate">The message will never be considered a duplicate.</param>
        /// <param name="durationOverride">Message show duration override.</param>
        /// <param name="clear">Clear the message queue and close the active snackbar.</param>
        /// <param name="activateWindow">Activate window when WindowState is Minimized</param>
        public static void MessageEnqueue(this Snackbar snackbar, object content,
            object actionContent = null, Action<object> actionHandler = null, object actionArgument = default,
            bool promote = false, bool neverConsiderToBeDuplicate = true, TimeSpan? durationOverride = null,
            bool clear = true, bool activateWindow = true) =>
            snackbar.MessageEnqueue<object>(content, actionContent, actionHandler, actionArgument,
                promote, neverConsiderToBeDuplicate, durationOverride, clear, activateWindow);

        /// <summary>
        /// Queues a notification message for display in a snackbar.
        /// </summary>
        /// <param name="content">Message.</param>
        /// <param name="actionContent">Content for the action button.</param>
        /// <param name="actionHandler"> Call back to be executed if user clicks the action button.</param>
        /// <param name="promote"> The message will promoted to the front of the queue.</param>
        /// <param name="neverConsiderToBeDuplicate">The message will never be considered a duplicate.</param>
        /// <param name="durationOverride">Message show duration override.</param>
        /// <param name="clear">Clear the message queue and close the active snackbar.</param>
        /// <param name="activateWindow">Activate window when WindowState is Minimized</param>
        public static void MessageEnqueue(this Snackbar snackbar, object content,
            object actionContent, Action actionHandler,
            bool promote = false, bool neverConsiderToBeDuplicate = true, TimeSpan? durationOverride = null,
            bool clear = true, bool activateWindow = true) =>
            snackbar.MessageEnqueue<object>(content, actionContent, (param) => { actionHandler?.Invoke(); }, default,
                promote, neverConsiderToBeDuplicate, durationOverride, clear, activateWindow);
    }
}
