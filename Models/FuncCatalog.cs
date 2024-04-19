using Params;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Lib.Attributes.Table(DBParam.DBType.SQLSERVER, DBParam.DBName.UAAC, "FuncCatalog")]
    public class FuncCatalog : BaseModel<FuncCatalog>
    {
        private string _FuncCatalogId;
        [Key]
        public string FuncCatalogId
        {
            get => _FuncCatalogId;
            set => Set(ref _FuncCatalogId, value);
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

        private string _FuncId;
        public string FuncId
        {
            get => _FuncId;
            set => Set(ref _FuncId, value);
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
