using Lib;
using Lib.Wpf;
using Lib.Wpf.Routes;
using Models;
using Params;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using static Params.SysAppParam;
using static Params.SysLogParam;

namespace ViewModels
{
    public class LoginViewModel : BaseViewModel<LoginViewModel>
    {
        /// <summary>
        /// Static Property 變更通知機制，通知 Binding 更新，此為委派事件
        /// </summary>
        public static event PropertyChangedEventHandler StaticPropertyChanged;
        /// <summary>
        ///  賦值並通知 Static Property 已經修改 Binding 更新，Static Property 同步方法，
        ///  需置於 Static Property 所在 class
        /// </summary>
        private static bool SetStatic<T>(ref T field, T newValue, [CallerMemberName] string propertyName = null)
        {
            if (!EqualityComparer<T>.Default.Equals(field, newValue))
            {
                field = newValue;
                StaticPropertyChanged?.Invoke(null, new PropertyChangedEventArgs(propertyName));
                return true;
            }
            return false;
        }

        private static Auth _loginUser;
        /// <summary>
        /// 使用者資訊
        /// </summary>
        public static Auth LoginUser
        {
            get => _loginUser;
            set => SetStatic(ref _loginUser, value);
        }

        private static List<SysFuncPermissionDetailDistinct> _loginUserPermission;
        /// <summary>
        /// 使用者權限
        /// </summary>
        public static List<SysFuncPermissionDetailDistinct> LoginUserPermission
        {
            get
            {
                if (_loginUserPermission == null)
                {
                    var permissionResult = ApiUtil.HttpClientEx<ApiResult<List<SysFuncPermissionDetailDistinct>>>(
                        UAACRoute.Service(), UAACRoute.Permission.GetSysFuncPermissionDetailDistinct,
                        method: ApiParam.HttpVerbs.Get,
                        queryParams: new { userId = LoginUser.EmpId, sysId = SysId.Portal });
                    _loginUserPermission = permissionResult.Data;
                }
                return _loginUserPermission;
            }
            set => SetStatic(ref _loginUserPermission, value);
        }

        private string _userId;
        [Display(Name = "使用者帳號")]
        public string UserId
        {
            get => _userId;
            set => Set(ref _userId, value);
        }

        /// <summary>
        /// 登入成功，顯示主畫面
        /// </summary>
        public Action ShowMainWindow;


        private DelegateCommand<PasswordBox> _loginCommand;
        public DelegateCommand<PasswordBox> LoginCommand =>
            _loginCommand ?? (_loginCommand = new DelegateCommand<PasswordBox>
            (OnLogin, param => Validate().IsValid));
        private void OnLogin(PasswordBox passwordBox)
        {
            if (!Validate().IsValid) return;

            string errorMsg = string.Empty;

            var authResult = ApiUtil.HttpClientEx<ApiResult<Auth>>(
                UAACRoute.Service(), UAACRoute.Auth.Controller,
                method: ApiParam.HttpVerbs.Post,
                basicAuth: new BasicAuth { UserId = UserId, Password = passwordBox.Password });

            if (!authResult.Succ)
                errorMsg = authResult.Msg;
            else
            {
                UAACRoute.Token = authResult.Data.Token;
                LoginUser = authResult.Data;
                LoginUser.Password = passwordBox.Password;
                Environment.SetEnvironmentVariable("CychUserId", LoginUser.EmpId, EnvironmentVariableTarget.Process);
                Environment.SetEnvironmentVariable("CychUserIdHis", LoginUser.EmpIdHis, EnvironmentVariableTarget.Process);
                ShowMainWindow?.Invoke();
            }

            ApiUtil.HttpClientExAsync<ApiResult<SysLog>>(
                UAACRoute.Service(), UAACRoute.SysLog.Controller,
                method: ApiParam.HttpVerbs.Post,
                body: new SysLog
                {
                    UserId = LoginUser?.EmpId ?? UserId,
                    SysId = SysId.Portal,
                    ProcId = Process.GetCurrentProcess().Id.ToString(),
                    ActionType = ActionType.Login,
                    ControllerClass = nameof(LoginViewModel),
                    ActionMethod = nameof(OnLogin),
                    State = errorMsg == string.Empty,
                    Msg = errorMsg
                }).FireAndForget();

            if (errorMsg != string.Empty)
                MessageBox.Show(errorMsg, MsgParam.TitlePrompt);
        }

        public void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key != Key.Enter) return;
            if (sender is PasswordBox)
                OnLogin(sender as PasswordBox);
            else
                CtrlUtil.KeyEnterMoveFocus(e);
        }

    }
}
