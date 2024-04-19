using System.Collections.Generic;

namespace Models
{
    public class DeptView : BaseModel<DeptView>
    {
        private string _No;
        /// <summary>
        /// 部門代碼
        /// </summary>
        public string No
        {
            get => _No;
            set => Set(ref _No, value);
        }

        private string _Name;
        /// <summary>
        /// 部門名稱
        /// </summary>
        public string Name
        {
            get => _Name;
            set => Set(ref _Name, value);
        }

        private string _Chief;
        /// <summary>
        /// 部門主管
        /// </summary>
        public string Chief
        {
            get => _Chief;
            set => Set(ref _Chief, value);
        }

        private string _ViceChief;
        /// <summary>
        /// 部門副主管
        /// </summary>
        public string ViceChief
        {
            get => _ViceChief;
            set => Set(ref _ViceChief, value);
        }

        private IEnumerable<string> _Chiefs;
        /// <summary>
        /// 主管群
        /// </summary>
        public IEnumerable<string> Chiefs
        {
            get => _Chiefs;
            set => Set(ref _Chiefs, value);
        }

        private IEnumerable<string> _Secretaries;
        /// <summary>
        /// 秘書群
        /// </summary>
        public IEnumerable<string> Secretaries
        {
            get => _Secretaries;
            set => Set(ref _Secretaries, value);
        }

        private bool _IsVirtual;
        /// <summary>
        /// 是否為虛擬部門
        /// </summary>
        public bool IsVirtual
        {
            get => _IsVirtual;
            set => Set(ref _IsVirtual, value);
        }

        private string _MajorDept;
        /// <summary>
        /// 大部門名稱
        /// </summary>
        public string MajorDept
        {
            get => _MajorDept;
            set => Set(ref _MajorDept, value);
        }
    }
}
