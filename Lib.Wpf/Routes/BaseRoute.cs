using System.Configuration;

namespace Lib.Wpf.Routes
{
    /// <summary>
    /// API Name (API Url 定義於 App.config appSettings)
    /// </summary>
    public class ApiName
    {
        /// <summary>
        /// Portal And UAAC
        /// </summary>
        public const string UAAC = "UAAC";
        /// <summary>
        /// 住院藥局調劑系統
        /// </summary>
        public const string Pds = "Pds";
        /// <summary>
        /// 手術系統
        /// </summary>
        public const string OPState = "OPState";
        /// <summary>
        /// 人資系統
        /// </summary>
        public const string Hr = "Hr";
        /// <summary>
        /// 排檢系統
        /// </summary>
        public const string PeriExam = "PeriExam";
    }

    public class BaseRoute
    {
        private static string _appRun;
        public static string AppRun
        {
            get
            {
                if (_appRun == null)
                    _appRun = ConfigUtil.GetEnvAppRun();
                return _appRun;
            }
        }

        /// <summary>
        /// API Url
        /// </summary>
        public static string Service(string name)
        {
            if (AppRun != string.Empty) name = $"{name}.{AppRun}";
            return ConfigurationManager.AppSettings[name].ToString();
        }
    }
}
