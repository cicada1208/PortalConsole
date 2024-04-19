using Params;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Lib.Attributes.Table(DBParam.DBType.SQLSERVER, DBParam.DBName.UAAC, "RoleUser")]
    public class RoleUser : BaseModel<RoleUser>
    {
        private string _RoleUserId;
        [Key]
        public string RoleUserId
        {
            get => _RoleUserId;
            set => Set(ref _RoleUserId, value);
        }

        private string _RoleId;
        [Display(Name = "角色群組代碼")]
        [MaxLength(36)]
        public string RoleId
        {
            get => _RoleId;
            set => Set(ref _RoleId, value);
        }

        private string _CpnyId;
        [Display(Name = "機構別代碼")]
        [MaxLength(10)]
        public string CpnyId
        {
            get => _CpnyId;
            set => Set(ref _CpnyId, value);
        }

        private string _DeptNo;
        [Display(Name = "科室代碼")]
        [MaxLength(20)]
        public string DeptNo
        {
            get => _DeptNo;
            set => Set(ref _DeptNo, value);
        }

        private string _Possie;
        [Display(Name = "職稱代碼")]
        [MaxLength(20)]
        public string Possie
        {
            get => _Possie;
            set => Set(ref _Possie, value);
        }

        private string _Attribute;
        [Display(Name = "屬性進階代碼")]
        [MaxLength(10)]
        public string Attribute
        {
            get => _Attribute;
            set => Set(ref _Attribute, value);
        }

        private string _UserId;
        [Display(Name = "使用者代碼")]
        [MaxLength(20)]
        public string UserId
        {
            get => _UserId;
            set => Set(ref _UserId, value);
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

    }
}
