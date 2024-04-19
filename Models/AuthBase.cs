using System;

namespace Models
{
    public abstract class AuthBase<T> : BaseModel<T> where T : class
    {
        private string _CpnyID;
        /// <summary>
        /// 機構別代碼
        /// </summary>
        public string CpnyID
        {
            get => _CpnyID;
            set => Set(ref _CpnyID, value);
        }

        private string _CoscName;
        /// <summary>
        /// 機構別名稱
        /// </summary>
        public string CoscName
        {
            get => _CoscName;
            set => Set(ref _CoscName, value);
        }

        private string _EmpId;
        /// <summary>
        /// 使用者帳號
        /// </summary>
        public string EmpId
        {
            get => _EmpId;
            set => Set(ref _EmpId, value);
        }

        private string _EmpIdHis;
        /// <summary>
        /// 使用者帳號 (醫療系統用：實習帳號截為5碼)
        /// </summary>
        public string EmpIdHis
        {
            get => _EmpIdHis;
            set => Set(ref _EmpIdHis, value);
        }

        private string _Name;
        /// <summary>
        /// 姓名
        /// </summary>
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }

        private string _EngName;
        /// <summary>
        /// 英文名字
        /// </summary>
        public string EngName
        {
            get => _EngName;
            set => Set(ref _EngName, value);
        }

        private string _DeptNo;
        /// <summary>
        /// 部門代碼
        /// </summary>
        public string DeptNo
        {
            get => _DeptNo;
            set => Set(ref _DeptNo, value);
        }

        private string _SubDeptNo;
        /// <summary>
        /// 組別代碼
        /// </summary>
        public string SubDeptNo
        {
            get => _SubDeptNo;
            set => Set(ref _SubDeptNo, value);
        }

        private string _DeptName;
        /// <summary>
        /// 部門名稱
        /// </summary>
        public string DeptName
        {
            get => _DeptName;
            set => Set(ref _DeptName, value);
        }

        private string _Possie;
        /// <summary>
        /// 職稱代碼
        /// </summary>
        public string Possie
        {
            get => _Possie;
            set => Set(ref _Possie, value);
        }

        private string _PosName;
        /// <summary>
        /// 職稱
        /// </summary>
        public string PosName
        {
            get => _PosName;
            set => Set(ref _PosName, value);
        }

        private string _AttributeID;
        /// <summary>
        /// 屬性代碼
        /// </summary>
        public string AttributeID
        {
            get => _AttributeID;
            set => Set(ref _AttributeID, value);
        }

        private string _Attribute;
        /// <summary>
        /// 屬性
        /// </summary>
        public string Attribute
        {
            get => _Attribute;
            set => Set(ref _Attribute, value);
        }

        private string _State;
        /// <summary>
        /// 在職狀態
        /// </summary>
        public string State
        {
            get => _State;
            set => Set(ref _State, value);
        }

        private string _Birthday;
        /// <summary>
        /// 生日
        /// </summary>
        public string Birthday
        {
            get => _Birthday;
            set => Set(ref _Birthday, value);
        }

        private string _Sex;
        /// <summary>
        /// 性別
        /// </summary>
        public string Sex
        {
            get => _Sex;
            set => Set(ref _Sex, value);
        }

        private string _Ext;
        /// <summary>
        /// 分機
        /// </summary>
        public string Ext
        {
            get => _Ext;
            set => Set(ref _Ext, value);
        }

        private string _Email;
        /// <summary>
        /// e-mail
        /// </summary>
        public string Email
        {
            get => _Email;
            set => Set(ref _Email, value);
        }

        private string _TelNo;
        /// <summary>
        /// 電話
        /// </summary>
        public string TelNo
        {
            get => _TelNo;
            set => Set(ref _TelNo, value);
        }

        private string _CellNo;
        /// <summary>
        /// 手機
        /// </summary>
        public string CellNo
        {
            get => _CellNo;
            set => Set(ref _CellNo, value);
        }

        private string _Grade;
        /// <summary>
        /// 職等職級
        /// </summary>
        public string Grade
        {
            get => _Grade;
            set => Set(ref _Grade, value);
        }

        private string _Rank;
        /// <summary>
        /// 進階
        /// </summary>
        public string Rank
        {
            get => _Rank;
            set => Set(ref _Rank, value);
        }

        private bool _IsVs;
        /// <summary>
        /// 是否為主治醫師(完訓醫師以上)
        /// </summary>
        public bool IsVs
        {
            get => _IsVs;
            set => Set(ref _IsVs, value);
        }

        private bool _IsPgyR;
        /// <summary>
        /// 是否為R或PGY
        /// </summary>
        public bool IsPgyR
        {
            get => _IsPgyR;
            set => Set(ref _IsPgyR, value);
        }

        private bool _IsDr;
        /// <summary>
        /// 是否為醫師
        /// </summary>
        public bool IsDr
        {
            get => _IsDr;
            set => Set(ref _IsDr, value);
        }

        private bool _NotCard;
        /// <summary>
        /// 是否免刷卡
        /// </summary>
        public bool NotCard
        {
            get => _NotCard;
            set => Set(ref _NotCard, value);
        }

        private string _QuitDateStr;
        public string QuitDateStr
        {
            get => _QuitDateStr;
            set => Set(ref _QuitDateStr, value);
        }

        private DateTime? _ArrivalDate;
        /// <summary>
        /// 到職日
        /// </summary>
        public DateTime? ArrivalDate
        {
            get => _ArrivalDate;
            set => Set(ref _ArrivalDate, value);
        }

        private DateTime? _VacBaseDate;
        /// <summary>
        /// 休假基準日
        /// </summary>
        public DateTime? VacBaseDate
        {
            get => _VacBaseDate;
            set => Set(ref _VacBaseDate, value);
        }

        private double _WorkHour;
        /// <summary>
        /// 工時
        /// </summary>
        public double WorkHour
        {
            get => _WorkHour;
            set => Set(ref _WorkHour, value);
        }

        private string _Token;
        public string Token
        {
            get => _Token;
            set => Set(ref _Token, value);
        }
    }
}
