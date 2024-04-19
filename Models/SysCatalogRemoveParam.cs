namespace Models
{
    public class SysCatalogRemoveParam : BaseModel<SysCatalogRemoveParam>
    {
        private string _SysCatalogId;
        /// <summary>
        /// 欲停用的系統目錄階層Id
        /// </summary>
        public string SysCatalogId
        {
            get => _SysCatalogId;
            set => Set(ref _SysCatalogId, value);
        }

        private string _RootId;
        /// <summary>
        /// SysCatalogId 所屬的 RootId
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
