using System.Collections.Generic;

namespace Models
{
    public class FuncCatalogAddFunc : BaseModel<FuncCatalogAddFunc>
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

        private List<FuncSelected> _Funcs;
        public List<FuncSelected> Funcs
        {
            get => _Funcs;
            set => Set(ref _Funcs, value);
        }
    }
}
