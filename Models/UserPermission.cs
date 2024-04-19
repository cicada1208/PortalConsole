using Params;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Lib.Attributes.Table(DBParam.DBType.SQLSERVER, DBParam.DBName.UAAC, "UserPermission")]
    public class UserPermission : BaseModel<UserPermission>
    {
        private string _UserPermissionId;
        [Key]
        public string UserPermissionId
        {
            get => _UserPermissionId;
            set => Set(ref _UserPermissionId, value);
        }

        private string _UserId;
        [Display(Name = "使用者代碼")]
        [MaxLength(20)]
        public string UserId
        {
            get => _UserId;
            set => Set(ref _UserId, value);
        }

        private string _SysId;
        [Display(Name = "系統代碼")]
        [MaxLength(36)]
        public string SysId
        {
            get => _SysId;
            set => Set(ref _SysId, value);
        }

        private string _FuncId;
        [Display(Name = "功能代碼")]
        [MaxLength(36)]
        public string FuncId
        {
            get => _FuncId;
            set => Set(ref _FuncId, value);
        }

        private bool? _QueryAuth;
        [Display(Name = "可否查詢")]
        public bool? QueryAuth
        {
            get => _QueryAuth;
            set => Set(ref _QueryAuth, value);
        }

        private bool? _AddAuth;
        [Display(Name = "可否新增")]
        public bool? AddAuth
        {
            get => _AddAuth;
            set => Set(ref _AddAuth, value);
        }

        private bool? _ModifyAuth;
        [Display(Name = "可否修改")]
        public bool? ModifyAuth
        {
            get => _ModifyAuth;
            set => Set(ref _ModifyAuth, value);
        }

        private bool? _DeleteAuth;
        [Display(Name = "可否刪除")]
        public bool? DeleteAuth
        {
            get => _DeleteAuth;
            set => Set(ref _DeleteAuth, value);
        }

        private bool? _ExportAuth;
        [Display(Name = "可否匯出")]
        public bool? ExportAuth
        {
            get => _ExportAuth;
            set => Set(ref _ExportAuth, value);
        }

        private bool? _PrintAuth;
        [Display(Name = "可否列印")]
        public bool? PrintAuth
        {
            get => _PrintAuth;
            set => Set(ref _PrintAuth, value);
        }

        private bool? _Activate;
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

        private bool _Selected = false;
        /// <summary>
        /// 選取
        /// </summary>
        [NotMapped]
        public bool Selected
        {
            get => _Selected;
            set => Set(ref _Selected, value);
        }

    }
}
