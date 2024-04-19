using Params;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Lib.Attributes.Table(DBParam.DBType.SQLSERVER, DBParam.DBName.UAAC, "SysCatalog")]
    public class SysCatalog : BaseModel<SysCatalog>
    {
        private string _SysCatalogId;
        [Key]
        public string SysCatalogId
        {
            get => _SysCatalogId;
            set => Set(ref _SysCatalogId, value);
        }

        private string _RootId;
        public string RootId
        {
            get => _RootId;
            set => Set(ref _RootId, value);
        }

        private string _CatalogId;
        public string CatalogId
        {
            get => _CatalogId;
            set => Set(ref _CatalogId, value);
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
