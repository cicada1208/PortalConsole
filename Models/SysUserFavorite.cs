using Params;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Lib.Attributes.Table(DBParam.DBType.SQLSERVER, DBParam.DBName.UAAC, "SysUserFavorite")]
    public class SysUserFavorite : BaseModel<SysUserFavorite>
    {
        private string _SysUserFavoriteId;
        [Key]
        public string SysUserFavoriteId
        {
            get => _SysUserFavoriteId;
            set => Set(ref _SysUserFavoriteId, value);
        }

        private string _UserId;
        public string UserId
        {
            get => _UserId;
            set => Set(ref _UserId, value);
        }

        private string _SysId;
        public string SysId
        {
            get => _SysId;
            set => Set(ref _SysId, value);
        }

        private short? _Seq;
        public short? Seq
        {
            get => _Seq;
            set => Set(ref _Seq, value);
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
