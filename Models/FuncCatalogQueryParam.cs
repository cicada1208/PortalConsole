namespace Models
{
    public class FuncCatalogQueryParam : BaseModel<FuncCatalogQueryParam>
    {
        private string _SysId;
        public string SysId
        {
            get => _SysId;
            set => Set(ref _SysId, value);
        }

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
    }
}
