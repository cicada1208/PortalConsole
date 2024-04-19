using MaterialDesignThemes.Wpf;
using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Interop;

namespace Controls
{
    /// <summary>
    /// <para>
    /// Windows will only send WM_IME_SETCONTEXT message to the active window, 
    /// Popup by default is designed to be shown with WM_EX_NOACTIVE 
    /// which means that it doesn't in active state when displaying, 
    /// that's why IME could not work correctly in this regard, to workaround this issue, 
    /// you could try set the Popup as the active window using win32 SetActiveWindow() API.
    /// </para>
    /// <para>IMEPopupBox do not work well with editable ComboBox.</para>
    /// </summary>
    /// <remarks>
    /// https://social.msdn.microsoft.com/Forums/en-US/b2428b85-adc9-4a1e-a588-8dbb3b9aac06/ime-can39t-be-turned-on-for-textboxes-inside-a-popup?forum=wpf
    /// </remarks>
    public class IMEPopupBox : PopupBox
    {
        [DllImport("user32.dll")]
        private static extern IntPtr SetActiveWindow(IntPtr hWnd);

        public IMEPopupBox()
        {
            EventManager.RegisterClassHandler(
            typeof(IMEPopupBox),
            PreviewGotKeyboardFocusEvent,
            new KeyboardFocusChangedEventHandler(OnPreviewGotKeyboardFocus),
            true);
        }

        public static void OnPreviewGotKeyboardFocus(Object sender, KeyboardFocusChangedEventArgs e)
        {
            var textBox = e.NewFocus as TextBoxBase;
            if (textBox != null)
            {
                var hwndSource = PresentationSource.FromVisual(textBox) as HwndSource;
                if (hwndSource != null)
                    SetActiveWindow(hwndSource.Handle);
            }
        }

    }
}
