namespace Models
{
    public class Company : BaseModel<Company>
    {
        private string _Cpnyid;
        /// <summary>
        /// 公司別 (ID)
        /// </summary>
        public string Cpnyid
        {
            get => _Cpnyid;
            set => Set(ref _Cpnyid, value);
        }

        private string _Postal;
        /// <summary>
        /// 郵遞區號
        /// </summary>
        public string Postal
        {
            get => _Postal;
            set => Set(ref _Postal, value);
        }

        private string _Cocname;
        /// <summary>
        /// 公司中文名稱
        /// </summary>
        public string Cocname
        {
            get => _Cocname;
            set => Set(ref _Cocname, value);
        }

        private string _Coename;
        /// <summary>
        /// 公司英文名稱
        /// </summary>
        public string Coename
        {
            get => _Coename;
            set => Set(ref _Coename, value);
        }

        private string _Coscname;
        /// <summary>
        /// 公司中文簡稱
        /// </summary>
        public string Coscname
        {
            get => _Coscname;
            set => Set(ref _Coscname, value);
        }

        private string _Cosename;
        /// <summary>
        /// 公司英文簡稱
        /// </summary>
        public string Cosename
        {
            get => _Cosename;
            set => Set(ref _Cosename, value);
        }

        private string _Caddr;
        /// <summary>
        /// 中文地址
        /// </summary>
        public string Caddr
        {
            get => _Caddr;
            set => Set(ref _Caddr, value);
        }

        private string _Eaddr;
        /// <summary>
        /// 英文地址
        /// </summary>
        public string Eaddr
        {
            get => _Eaddr;
            set => Set(ref _Eaddr, value);
        }

        private string _Bossname;
        /// <summary>
        /// 負責人
        /// </summary>
        public string Bossname
        {
            get => _Bossname;
            set => Set(ref _Bossname, value);
        }

        private string _Telno1;
        /// <summary>
        /// 電話(1)
        /// </summary>
        public string Telno1
        {
            get => _Telno1;
            set => Set(ref _Telno1, value);
        }

        private string _Telno2;
        /// <summary>
        /// 電話(2)
        /// </summary>
        public string Telno2
        {
            get => _Telno2;
            set => Set(ref _Telno2, value);
        }

        private string _Faxno;
        /// <summary>
        /// 傳真號碼
        /// </summary>
        public string Faxno
        {
            get => _Faxno;
            set => Set(ref _Faxno, value);
        }

        private string _Colicnid;
        /// <summary>
        /// 統一編號
        /// </summary>
        public string Colicnid
        {
            get => _Colicnid;
            set => Set(ref _Colicnid, value);
        }

        private string _Taxtid;
        /// <summary>
        /// 稅籍編號
        /// </summary>
        public string Taxtid
        {
            get => _Taxtid;
            set => Set(ref _Taxtid, value);
        }

        private string _Muser;
        /// <summary>
        /// 異動者
        /// </summary>
        public string Muser
        {
            get => _Muser;
            set => Set(ref _Muser, value);
        }

        private string _Mdate;
        /// <summary>
        /// 異動日期
        /// </summary>
        public string Mdate
        {
            get => _Mdate;
            set => Set(ref _Mdate, value);
        }

        private string _Mtime;
        /// <summary>
        /// 異動時間
        /// </summary>
        public string Mtime
        {
            get => _Mtime;
            set => Set(ref _Mtime, value);
        }

        private string _Laborid;
        /// <summary>
        /// 勞保證號
        /// </summary>
        public string Laborid
        {
            get => _Laborid;
            set => Set(ref _Laborid, value);
        }

        private string _Heaid;
        /// <summary>
        /// 健保證號
        /// </summary>
        public string Heaid
        {
            get => _Heaid;
            set => Set(ref _Heaid, value);
        }

        private string _LaborReid;
        /// <summary>
        /// 勞退證號
        /// </summary>
        public string LaborReid
        {
            get => _LaborReid;
            set => Set(ref _LaborReid, value);
        }

        private int? _OrderNo;
        public int? OrderNo
        {
            get => _OrderNo;
            set => Set(ref _OrderNo, value);
        }
    }
}
