using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Models
{
    public class SysProcess : BaseModel<SysProcess>
    {
        private string _SysId;
        public string SysId
        {
            get => _SysId;
            set => Set(ref _SysId, value);
        }

        private Process _Process;
        public Process Process
        {
            get => _Process;
            set => Set(ref _Process, value);
        }

        private List<IntPtr> _Hwnds;
        public List<IntPtr> Hwnds
        {
            get => _Hwnds;
            set => Set(ref _Hwnds, value);
        }

    }
}
