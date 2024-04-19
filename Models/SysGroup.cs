using System.Collections.ObjectModel;

namespace Models
{
    public class SysGroup : SysAppBase<SysGroup>
    {
        private string _SysCatalogId;
        /// <summary>
        /// 系統目錄階層Id
        /// </summary>
        public string SysCatalogId
        {
            get => _SysCatalogId;
            set => Set(ref _SysCatalogId, value);
        }

        private string _RootId;
        /// <summary>
        /// 根Id
        /// </summary>
        public string RootId
        {
            get => _RootId;
            set => Set(ref _RootId, value);
        }

        private string _CatalogId;
        /// <summary>
        /// 目錄Id
        /// </summary>
        public string CatalogId
        {
            get => _CatalogId;
            set => Set(ref _CatalogId, value);
        }

        private short? _Seq;
        /// <summary>
        /// 順序
        /// </summary>
        public short? Seq
        {
            get => _Seq;
            set => Set(ref _Seq, value);
        }

        private ObservableCollection<SysGroup> _SysApps;
        /// <summary>
        /// 群組子系統
        /// </summary>
        public ObservableCollection<SysGroup> SysApps
        {
            get => _SysApps;
            set => Set(ref _SysApps, value);
        }

        private string _SysUserFavoriteId;
        /// <summary>
        /// 系統使用者最愛Id
        /// </summary>
        public string SysUserFavoriteId
        {
            get => _SysUserFavoriteId;
            set => Set(ref _SysUserFavoriteId, value);
        }

        private bool _Favorite;
        /// <summary>
        /// 使用者最愛
        /// </summary>
        public bool Favorite
        {
            get => _Favorite;
            set => Set(ref _Favorite, value);
        }


        private bool _Loading = false;
        /// <summary>
        /// 系統加載狀態
        /// </summary>
        public bool Loading
        {
            get => _Loading;
            set => Set(ref _Loading, value);
        }

    }
}
