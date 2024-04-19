namespace Models
{
    public class SysCatalogQueryParam : BaseModel<SysCatalogQueryParam>
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
    }
}
