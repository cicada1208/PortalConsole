using Params;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Lib.Attributes.Table(DBParam.DBType.SQLSERVER, DBParam.DBName.UAAC, "Role")]
    public class Role : BaseModel<Role>
    {
        private string _RoleId;
        [Key]
        [Display(Name = "角色群組代碼")]
        [MaxLength(36)]
        public string RoleId
        {
            get => _RoleId;
            set => Set(ref _RoleId, value);
        }

        private string _RoleName;
        [Display(Name = "角色群組名稱")]
        [MaxLength(50)]
        public string RoleName
        {
            get => _RoleName;
            set => Set(ref _RoleName, value);
        }

        private string _Description;
        [Display(Name = "描述")]
        [MaxLength(100)]
        public string Description
        {
            get => _Description;
            set => Set(ref _Description, value);
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
