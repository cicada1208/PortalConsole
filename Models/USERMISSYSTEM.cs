namespace Models
{
    public class USERMISSYSTEM : BaseModel<USERMISSYSTEM>
    {
        private string _SYSID;
        public string SYSID
        {
            get => _SYSID;
            set => Set(ref _SYSID, value);
        }

        private string _SYSNAME;
        public string SYSNAME
        {
            get => _SYSNAME;
            set => Set(ref _SYSNAME, value);
        }

        private string _EXENAME;
        public string EXENAME
        {
            get => _EXENAME;
            set => Set(ref _EXENAME, value);
        }

        private string _FOLDERNAME;
        public string FOLDERNAME
        {
            get => _FOLDERNAME;
            set => Set(ref _FOLDERNAME, value);
        }

        private string _LOCALPATH;
        public string LOCALPATH
        {
            get => _LOCALPATH;
            set => Set(ref _LOCALPATH, value);
        }

        private string _SERVERPATH;
        public string SERVERPATH
        {
            get => _SERVERPATH;
            set => Set(ref _SERVERPATH, value);
        }

        private string _SUBSYS;
        public string SUBSYS
        {
            get => _SUBSYS;
            set => Set(ref _SUBSYS, value);
        }

        private string _ISSTOP;
        public string ISSTOP
        {
            get => _ISSTOP;
            set => Set(ref _ISSTOP, value);
        }

        private string _depnoAuth;
        public string depnoAuth
        {
            get => _depnoAuth;
            set => Set(ref _depnoAuth, value);
        }

        private string _ROLE;
        public string ROLE
        {
            get => _ROLE;
            set => Set(ref _ROLE, value);
        }

        private string _EMPNO;
        public string EMPNO
        {
            get => _EMPNO;
            set => Set(ref _EMPNO, value);
        }
    }
}
