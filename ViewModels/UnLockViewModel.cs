using Lib.Wpf;
using Params;
using System.Windows.Controls;

namespace ViewModels
{
    public class UnLockViewModel : BaseViewModel<UnLockViewModel>
    {
        private string _authMsg;
        public string AuthMsg
        {
            get => _authMsg;
            set => Set(ref _authMsg, value);
        }

        private DelegateCommand<PasswordBox> _okCommand;
        public DelegateCommand<PasswordBox> OKCommand =>
            _okCommand ?? (_okCommand = new DelegateCommand<PasswordBox>(OnOK));
        private void OnOK(PasswordBox passwordBox)
        {
            //var result = ApiUtil.HttpClientEx<ApiResult<Auth>>(
            //    UAACRoute.Service(), UAACRoute.Auth.Controller,
            //    method: ApiParam.HttpVerbs.Post,
            //    basicAuth: new BasicAuth
            //    {
            //        UserId = LoginViewModel.LoginUser.EmpId,
            //        Password = passwordBox.Password
            //    });

            //CloseDialog?.Invoke(result.Succ, result.Succ);

            bool succ = LoginViewModel.LoginUser.Password == passwordBox.Password;
            AuthMsg = succ ? string.Empty : MsgParam.LoginErrorPw;
            if (succ) passwordBox.Password = string.Empty;
            CloseDialog?.Invoke(succ, succ);
        }

        private DelegateCommand<PasswordBox> _cancelCommand;
        public DelegateCommand<PasswordBox> CancelCommand =>
            _cancelCommand ?? (_cancelCommand = new DelegateCommand<PasswordBox>(OnCancel));
        private void OnCancel(PasswordBox passwordBox)
        {
            passwordBox.Password = string.Empty;
            CloseDialog?.Invoke(true);
        }

    }
}
