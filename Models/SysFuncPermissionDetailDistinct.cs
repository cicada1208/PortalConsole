using static Params.FuncParam;

namespace Models
{
    public class SysFuncPermissionDetailDistinct : BaseModel<SysFuncPermissionDetailDistinct>
    {

        private string _SysId;
        /// <summary>
        /// 系統Id
        /// </summary>
        public string SysId
        {
            get => _SysId;
            set => Set(ref _SysId, value);
        }

        private string _FuncId;
        /// <summary>
        /// 功能Id
        /// </summary>
        public string FuncId
        {
            get => _FuncId;
            set => Set(ref _FuncId, value);
        }

        private bool? _QueryAuth;
        public bool? QueryAuth
        {
            get => _QueryAuth;
            set => Set(ref _QueryAuth, value);
        }

        private bool? _AddAuth;
        public bool? AddAuth
        {
            get => _AddAuth;
            set => Set(ref _AddAuth, value);
        }

        private bool? _ModifyAuth;
        public bool? ModifyAuth
        {
            get => _ModifyAuth;
            set => Set(ref _ModifyAuth, value);
        }

        private bool? _DeleteAuth;
        public bool? DeleteAuth
        {
            get => _DeleteAuth;
            set => Set(ref _DeleteAuth, value);
        }

        private bool? _ExportAuth;
        public bool? ExportAuth
        {
            get => _ExportAuth;
            set => Set(ref _ExportAuth, value);
        }

        private bool? _PrintAuth;
        public bool? PrintAuth
        {
            get => _PrintAuth;
            set => Set(ref _PrintAuth, value);
        }

        private string _FuncName;
        /// <summary>
        /// 功能名稱
        /// </summary>
        public string FuncName
        {
            get => _FuncName;
            set => Set(ref _FuncName, value);
        }

        private FuncType? _FuncType;
        /// <summary>
        /// 功能類別
        /// </summary>
        public FuncType? FuncType
        {
            get => _FuncType;
            set => Set(ref _FuncType, value);
        }

        private string _BasePath;
        /// <summary>
        /// 基礎路徑或完整路徑
        /// </summary>
        public string BasePath
        {
            get => _BasePath;
            set => Set(ref _BasePath, value);
        }

        private string _SubPath;
        /// <summary>
        /// 子路徑
        /// </summary>
        public string SubPath
        {
            get => _SubPath;
            set => Set(ref _SubPath, value);
        }

        private string _Assembly;
        /// <summary>
        /// 檔名(含附檔名)
        /// </summary>
        public string Assembly
        {
            get => _Assembly;
            set => Set(ref _Assembly, value);
        }

        private string _ViewName;
        /// <summary>
        /// 視圖名稱
        /// </summary>
        public string ViewName
        {
            get => _ViewName;
            set => Set(ref _ViewName, value);
        }

        private string _ViewComponent;
        /// <summary>
        /// 視圖頁面元件
        /// </summary>
        public string ViewComponent
        {
            get => _ViewComponent;
            set => Set(ref _ViewComponent, value);
        }

        private short? _IconType;
        /// <summary>
        /// 圖示類型
        /// </summary>
        public short? IconType
        {
            get => _IconType;
            set => Set(ref _IconType, value);
        }

        private string _Icon;
        /// <summary>
        /// 圖示內容或路徑
        /// </summary>
        public string Icon
        {
            get => _Icon;
            set => Set(ref _Icon, value);
        }

        private short? _Limit;
        /// <summary>
        /// 限制執行次數
        /// </summary>
        public short? Limit
        {
            get => _Limit;
            set => Set(ref _Limit, value);
        }

    }
}
