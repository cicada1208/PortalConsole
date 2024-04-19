using Lib;
using Lib.Wpf;
using Lib.Wpf.Routes;
using Models;
using Params;
using System.Diagnostics;
using System.Reflection;
using System.Windows;
using System.Windows.Threading;
using ViewModels;
using static Params.SysAppParam;
using static Params.SysLogParam;

namespace PortalConsole
{
    /// <summary>
    /// App.xaml 的互動邏輯
    /// </summary>
    public partial class App : Application
    {
        private static string _dbFormal;
        public static string DBFormal
        {
            get
            {
                if (_dbFormal == null)
                {
                    switch (ConfigUtil.GetEnvAppRun())
                    {
                        case "dev":
                            _dbFormal = "開發";
                            break;
                        case "test":
                            _dbFormal = "測試";
                            break;
                        case "prod":
                            _dbFormal = "正式";
                            break;
                        default:
                            _dbFormal = string.Empty;
                            break;
                    }
                }
                return _dbFormal;
            }
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            DateTimeUtil.SetDefaultCulture();
            CtrlUtil.SetToolTipShowOnDisabled();
            CtrlUtil.SetToolTipShowDuration(30000);
        }

        private void App_DispatcherUnhandledException(object sender,
            DispatcherUnhandledExceptionEventArgs e)
        {
            ApiUtil.HttpClientExAsync<ApiResult<SysLog>>(
                UAACRoute.Service(), UAACRoute.SysLog.Controller,
                method: ApiParam.HttpVerbs.Post,
                body: new SysLog
                {
                    UserId = LoginViewModel.LoginUser?.EmpId,
                    SysId = SysId.Portal,
                    ProcId = Process.GetCurrentProcess().Id.ToString(),
                    ActionType = ActionType.Exception,
                    ControllerClass = MethodBase.GetCurrentMethod().DeclaringType.Name,
                    ActionMethod = MethodBase.GetCurrentMethod().Name,
                    State = false,
                    Msg = e.Exception.ToString()
                }).FireAndForget();

            MessageBox.Show(e.Exception.ToString());
            e.Handled = true;
        }
    }
}
