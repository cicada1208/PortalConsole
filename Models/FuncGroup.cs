using System.Collections.ObjectModel;

namespace Models
{
    public class FuncGroup : FuncBase<FuncGroup>
    {
        private string _FuncCatalogId;
        /// <summary>
        /// 功能目錄階層Id
        /// </summary>
        public string FuncCatalogId
        {
            get => _FuncCatalogId;
            set => Set(ref _FuncCatalogId, value);
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

        private ObservableCollection<FuncGroup> _Funcs;
        /// <summary>
        /// 群組子功能
        /// </summary>
        public ObservableCollection<FuncGroup> Funcs
        {
            get => _Funcs;
            set => Set(ref _Funcs, value);
        }

        private string _FuncUserFavoriteId;
        /// <summary>
        /// 功能使用者最愛Id
        /// </summary>
        public string FuncUserFavoriteId
        {
            get => _FuncUserFavoriteId;
            set => Set(ref _FuncUserFavoriteId, value);
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


        private object _ViewInstance;
        /// <summary>
        /// 視圖實例
        /// </summary>
        public object ViewInstance
        {
            get => _ViewInstance;
            set => Set(ref _ViewInstance, value);
        }

        /// <summary>
        /// 釋放視圖實例
        /// </summary>
        public void DisposeViewInstance() =>
            _ViewInstance = null;

    }
}
