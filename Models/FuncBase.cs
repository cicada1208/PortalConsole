using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Params.FuncParam;

namespace Models
{
    public abstract class FuncBase<T> : BaseModel<T> where T : class
    {
        private string _FuncId;
        /// <summary>
        /// 功能Id
        /// </summary>
        [Key]
        [Display(Name = "功能代碼")]
        [MaxLength(36)]
        public string FuncId
        {
            get => _FuncId;
            set => Set(ref _FuncId, value);
        }

        private string _SysId;
        /// <summary>
        /// 系統Id
        /// </summary>
        [Display(Name = "系統代碼")]
        [MaxLength(36)]
        public string SysId
        {
            get => _SysId;
            set => Set(ref _SysId, value);
        }

        private string _FuncName;
        /// <summary>
        /// 功能名稱
        /// </summary>
        [Display(Name = "功能名稱")]
        [MaxLength(60)]
        public string FuncName
        {
            get => _FuncName;
            set => Set(ref _FuncName, value);
        }

        private FuncType? _FuncType;
        /// <summary>
        /// 功能類別
        /// </summary>
        [Display(Name = "功能類別")]
        public FuncType? FuncType
        {
            get => _FuncType;
            set => Set(ref _FuncType, value);
        }

        private string _BasePath;
        /// <summary>
        /// 基礎路徑或完整路徑
        /// </summary>
        [Display(Name = "基礎路徑")]
        [MaxLength(100)]
        public string BasePath
        {
            get => _BasePath;
            set => Set(ref _BasePath, value);
        }

        private string _SubPath;
        /// <summary>
        /// 子路徑
        /// </summary>
        [Display(Name = "子路徑")]
        [MaxLength(100)]
        public string SubPath
        {
            get => _SubPath;
            set => Set(ref _SubPath, value);
        }

        private string _Path;
        /// <summary>
        /// <para>VersionExe：基礎路徑 + 子路徑</para>
        /// <para>Other：基礎路徑</para>
        /// </summary>
        [NotMapped]
        public string Path
        {
            get => _Path;
            set => Set(ref _Path, value);
        }

        private string _Assembly;
        /// <summary>
        /// 檔名(含附檔名)
        /// </summary>
        [Display(Name = "檔名")]
        [MaxLength(30)]
        public string Assembly
        {
            get => _Assembly;
            set => Set(ref _Assembly, value);
        }

        private string _ViewName;
        /// <summary>
        /// 視圖名稱
        /// </summary>
        [Display(Name = "視圖名稱")]
        [MaxLength(30)]
        public string ViewName
        {
            get => _ViewName;
            set => Set(ref _ViewName, value);
        }

        private string _ViewComponent;
        /// <summary>
        /// 視圖頁面元件
        /// </summary>
        [Display(Name = "視圖頁面元件")]
        [MaxLength(50)]
        public string ViewComponent
        {
            get => _ViewComponent;
            set => Set(ref _ViewComponent, value);
        }

        private short? _IconType;
        /// <summary>
        /// 圖示類型
        /// </summary>
        [Display(Name = "圖示類型")]
        public short? IconType
        {
            get => _IconType;
            set => Set(ref _IconType, value);
        }

        private string _Icon;
        /// <summary>
        /// 圖示內容或路徑
        /// </summary>
        [Display(Name = "圖示內容或路徑")]
        [MaxLength(50)]
        public string Icon
        {
            get => _Icon;
            set => Set(ref _Icon, value);
        }

        private short? _Limit;
        /// <summary>
        /// 限制執行次數
        /// </summary>
        [Display(Name = "限制執行次數")]
        public short? Limit
        {
            get => _Limit;
            set => Set(ref _Limit, value);
        }

        private bool? _Activate;
        /// <summary>
        /// 啟用
        /// </summary>
        public bool? Activate
        {
            get => _Activate;
            set => Set(ref _Activate, value);
        }

        private string _MUserId;
        public string MUserId
        {
            get => _MUserId;
            set => Set(ref _MUserId, value);
        }

        private string _MDateTime;
        public string MDateTime
        {
            get => _MDateTime;
            set => Set(ref _MDateTime, value);
        }

    }
}
