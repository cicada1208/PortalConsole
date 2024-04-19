using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using static Params.SysAppParam;

namespace Models
{
    public abstract class SysAppBase<T> : BaseModel<T> where T : class
    {
        private string _SysId;
        /// <summary>
        /// 系統Id
        /// </summary>
        [Key]
        [Display(Name = "系統代碼")]
        [MaxLength(36)]
        public string SysId
        {
            get => _SysId;
            set => Set(ref _SysId, value);
        }

        private string _SysName;
        /// <summary>
        /// 系統名稱
        /// </summary>
        [Display(Name = "系統名稱")]
        [MaxLength(60)]
        public string SysName
        {
            get => _SysName;
            set => Set(ref _SysName, value);
        }

        private SysType? _SysType;
        /// <summary>
        /// 系統類別
        /// </summary>
        [Display(Name = "系統類別")]
        public SysType? SysType
        {
            get => _SysType;
            set => Set(ref _SysType, value);
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
