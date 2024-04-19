using Params;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Lib.Attributes.Table(DBParam.DBType.SQLSERVER, DBParam.DBName.MISSYS, "WORKERS")]
    public class WORKERS : BaseModel<WORKERS>
    {
        private string _EMPNO;
        [Key]
        public string EMPNO
        {
            get => _EMPNO;
            set => Set(ref _EMPNO, value);
        }

        private string _NAME;
        public string NAME
        {
            get => _NAME;
            set => Set(ref _NAME, value);
        }

        private string _SEX;
        public string SEX
        {
            get => _SEX;
            set => Set(ref _SEX, value);
        }

        private string _PASSWORD;
        public string PASSWORD
        {
            get => _PASSWORD;
            set => Set(ref _PASSWORD, value);
        }

        private string _WORKTYPE;
        public string WORKTYPE
        {
            get => _WORKTYPE;
            set => Set(ref _WORKTYPE, value);
        }
    }
}
