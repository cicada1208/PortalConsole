namespace Models
{
    public class FuncCatalogRemoveParam : BaseModel<FuncCatalogRemoveParam>
    {
        private string _FuncCatalogId;
        /// <summary>
        /// 欲停用的功能目錄階層Id
        /// </summary>
        public string FuncCatalogId
        {
            get => _FuncCatalogId;
            set => Set(ref _FuncCatalogId, value);
        }

        private string _RootId;
        /// <summary>
        /// FuncCatalogId 所屬的 RootId
        /// </summary>
        public string RootId
        {
            get => _RootId;
            set => Set(ref _RootId, value);
        }

        private string _MUserId;
        public string MUserId
        {
            get => _MUserId;
            set => Set(ref _MUserId, value);
        }
    }
}
