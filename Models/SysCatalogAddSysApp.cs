using System.Collections.Generic;

namespace Models
{
    public class SysCatalogAddSysApp : BaseModel<SysCatalogAddSysApp>
    {
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

        private string _MUserId;
        public string MUserId
        {
            get => _MUserId;
            set => Set(ref _MUserId, value);
        }

        private List<SysAppSelected> _SysApps;
        public List<SysAppSelected> SysApps
        {
            get => _SysApps;
            set => Set(ref _SysApps, value);
        }
    }
}
