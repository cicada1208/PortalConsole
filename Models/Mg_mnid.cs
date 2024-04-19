using Params;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Lib.Attributes.Table(DBParam.DBType.SYBASE, DBParam.DBName.SYB2, "mg_mnid")]
    public class Mg_mnid : BaseModel<Mg_mnid>
    {
        private string _nid_id;
        [Key]
        public string nid_id
        {
            get => _nid_id;
            set => Set(ref _nid_id, value);
        }

        private string _nid_code;
        [Key]
        public string nid_code
        {
            get => _nid_code;
            set => Set(ref _nid_code, value);
        }

        private string _nid_trn;
        public string nid_trn
        {
            get => _nid_trn;
            set => Set(ref _nid_trn, value);
        }

        private string _nid_name;
        public string nid_name
        {
            get => _nid_name;
            set => Set(ref _nid_name, value);
        }

        private string _nid_rec;
        public string nid_rec
        {
            get => _nid_rec;
            set => Set(ref _nid_rec, value);
        }

    }
}

