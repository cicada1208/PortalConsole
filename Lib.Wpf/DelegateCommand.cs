using System;
using System.Diagnostics;
using System.Windows.Input;

namespace Lib.Wpf
{
    /// <summary>
    /// 無帶入參數
    /// </summary>
    public class DelegateCommand : DelegateCommand<object>
    {
        public DelegateCommand(Action executeMethod)
            : base(param => executeMethod?.Invoke())
        {
        }

        public DelegateCommand(Action executeMethod, Func<bool> canExecuteMethod)
            : base(param => executeMethod?.Invoke(), param => canExecuteMethod())
        {
        }
    }

    /// <summary>
    /// 帶入參數
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class DelegateCommand<T> : ICommand where T : class
    {
        private readonly Action<T> _executeAction;
        private readonly Func<T, bool> _canExecuteAction;

        /// <summary>
        /// 建立委派物件與設定對應執行的方法
        /// </summary>
        /// <param name="executeAction"></param>
        /// <param name="canExecuteAction"></param>
        public DelegateCommand(Action<T> executeAction, Func<T, bool> canExecuteAction = null)
        {
            _executeAction = executeAction;
            _canExecuteAction = canExecuteAction;
        }

        /// <summary>
        /// 若回傳 true，表示可執行 command
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter) =>
            _canExecuteAction?.Invoke(parameter as T) ?? true;

        /// <summary>
        /// 執行 command，取決於 CanExecute 是否回傳 true
        /// </summary>
        /// <param name="parameter"></param>
        public void Execute(object parameter) =>
            _executeAction?.Invoke(parameter as T);

        /// <summary>
        /// 再次檢查是否可執行 command (自動呼叫，但會頻繁觸發)
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add
            {
                if (_canExecuteAction != null)
                    CommandManager.RequerySuggested += value;
            }

            remove
            {
                if (_canExecuteAction != null)
                    CommandManager.RequerySuggested -= value;
            }
        }

        /// <summary>
        /// 再次檢查是否可執行 command (手動呼叫)
        /// </summary>
        public void RaiseCanExecuteChanged() =>
            CommandManager.InvalidateRequerySuggested();

        //public event EventHandler CanExecuteChanged;
        ///// <summary>
        ///// 再次檢查是否可執行 command (手動呼叫)
        ///// </summary>
        //public void RaiseCanExecuteChanged() =>
        //    CanExecuteChanged?.Invoke(this, EventArgs.Empty);

    }

}
