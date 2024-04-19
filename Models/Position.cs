namespace Models
{
    public class Position : BaseModel<Position>
    {
        private string _CpnyId;
        /// <summary>
        /// 機構別
        /// </summary>
        public string CpnyId
        {
            get => _CpnyId;
            set => Set(ref _CpnyId, value);
        }

        private string _Possie;
        /// <summary>
        /// 職稱代號
        /// </summary>
        public string Possie
        {
            get => _Possie;
            set => Set(ref _Possie, value);
        }

        private string _Poskind;
        /// <summary>
        /// 職稱種類
        /// </summary>
        public string Poskind
        {
            get => _Poskind;
            set => Set(ref _Poskind, value);
        }

        private string _PosName;
        /// <summary>
        /// 中文名稱
        /// </summary>
        public string PosName
        {
            get => _PosName;
            set => Set(ref _PosName, value);
        }

        private string _PosEname;
        /// <summary>
        /// 英文名稱
        /// </summary>
        public string PosEname
        {
            get => _PosEname;
            set => Set(ref _PosEname, value);
        }

        private string _PosBrief;
        /// <summary>
        /// 簡稱
        /// </summary>
        public string PosBrief
        {
            get => _PosBrief;
            set => Set(ref _PosBrief, value);
        }

        private string _Chief;
        /// <summary>
        /// 類型A主管B一般同仁
        /// </summary>
        public string Chief
        {
            get => _Chief;
            set => Set(ref _Chief, value);
        }

        private string _MUser;
        /// <summary>
        /// 異動者
        /// </summary>
        public string MUser
        {
            get => _MUser;
            set => Set(ref _MUser, value);
        }

        private string _MDate;
        /// <summary>
        /// 異動日期
        /// </summary>
        public string MDate
        {
            get => _MDate;
            set => Set(ref _MDate, value);
        }

        private string _MTime;
        /// <summary>
        /// 異動時間
        /// </summary>
        public string MTime
        {
            get => _MTime;
            set => Set(ref _MTime, value);
        }

        private string _Duty;
        /// <summary>
        /// 責任制
        /// </summary>
        public string Duty
        {
            get => _Duty;
            set => Set(ref _Duty, value);
        }

        private string _Notcard;
        /// <summary>
        /// 免刷卡
        /// </summary>
        public string Notcard
        {
            get => _Notcard;
            set => Set(ref _Notcard, value);
        }

        private string _Amt;
        /// <summary>
        /// 津貼
        /// </summary>
        public string Amt
        {
            get => _Amt;
            set => Set(ref _Amt, value);
        }

        private string _Disable;
        /// <summary>
        /// 是否停用(N非停用,其餘為停用)
        /// </summary>
        public string Disable
        {
            get => _Disable;
            set => Set(ref _Disable, value);
        }

        private string _DisableDate;
        /// <summary>
        /// 停用日期
        /// </summary>
        public string DisableDate
        {
            get => _DisableDate;
            set => Set(ref _DisableDate, value);
        }

        private string _Div;
        /// <summary>
        /// 層級
        /// </summary>
        public string Div
        {
            get => _Div;
            set => Set(ref _Div, value);
        }

        private int _SerialNumber;
        /// <summary>
        /// 流水號
        /// </summary>
        public int SerialNumber
        {
            get => _SerialNumber;
            set => Set(ref _SerialNumber, value);
        }
    }
}
