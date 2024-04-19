using Lib;
using Lib.Wpf.Routes;
using Models;
using Params;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using static MoreLinq.Extensions.DistinctByExtension;
using static Params.SysAppParam;

namespace ViewModels
{
    public class TestListViewViewModel : BaseViewModel<TestListViewViewModel>
    {
        public TestListViewViewModel()
        {
            Init().FireAndForget();
        }

        private async Task Init()
        {
            var result = await ApiUtil.HttpClientExAsync<ApiResult<ObservableCollection<SysAppSelected>>>(
                UAACRoute.Service(), UAACRoute.SysApp.GetSysCatalogAddList,
                method: ApiParam.HttpVerbs.Get,
                queryParams: new SysCatalogQueryParam { RootId = SysId.BasicRoot, CatalogId = SysId.BasicRoot });

            SysAppList = result.Data;

            // 設定 Collection 在 View 中篩選
            CollectionView filterView = CollectionViewSource.GetDefaultView(SysAppList) as CollectionView;
            filterView.Filter = SysAppFilter;

            SysIdList = new ObservableCollection<SysAppSelected>(SysAppList.Where(s =>
            !(s.SysType == SysType.Catalog)));

            var sysTypes = SysAppList.DistinctBy(s => s.SysType).Select(s => s.SysType);
            SysTypeList = new ObservableCollection<Item>(
                EnumData<SysType>.GetFilterCollection().Where(t =>
                t.Value == null || sysTypes.Contains((SysType?)t.Value)));
        }

        private ObservableCollection<SysAppSelected> _sysAppList;
        public ObservableCollection<SysAppSelected> SysAppList
        {
            get => _sysAppList;
            set => Set(ref _sysAppList, value);
        }

        private ObservableCollection<SysAppSelected> _sysIdList;
        /// <summary>
        /// 系統代碼(查詢清單除 Catalog)
        /// </summary>
        public ObservableCollection<SysAppSelected> SysIdList
        {
            get => _sysIdList;
            set => Set(ref _sysIdList, value);
        }

        private ObservableCollection<Item> _sysTypeList;
        /// <summary>
        /// 系統類別
        /// </summary>
        public ObservableCollection<Item> SysTypeList
        {
            get => _sysTypeList;
            set => Set(ref _sysTypeList, value);
        }

        private SysAppSelected _filteredItem;
        public SysAppSelected FilteredItem
        {
            get
            {
                if (_filteredItem == null)
                {
                    _filteredItem = new SysAppSelected();
                    _filteredItem.Selected = null;
                    _filteredItem.PropertyChanged += FilteredItemPropertyChanged;
                }
                return _filteredItem;
            }
            set
            {
                Set(ref _filteredItem, value);
                if (_filteredItem != null)
                {
                    _filteredItem.PropertyChanged -= FilteredItemPropertyChanged;
                    _filteredItem.PropertyChanged += FilteredItemPropertyChanged;
                }
            }
        }

        private void FilteredItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            // PropertyChanged Refresh View
            if (e.PropertyName == nameof(SysAppSelected.SysId) ||
                e.PropertyName == nameof(SysAppSelected.SysType) ||
                e.PropertyName == nameof(SysAppSelected.Selected))
                CollectionViewSource.GetDefaultView(SysAppList)?.Refresh();
        }

        private bool SysAppFilter(object item)
        {
            bool result = true;
            SysAppSelected sysApp = item as SysAppSelected;
            if (!FilteredItem.SysId.IsNullOrWhiteSpace())
                result = result && sysApp.SysId.Contains(FilteredItem.SysId);
            if (FilteredItem.SysType != null)
                result = result && sysApp.SysType.Equals(FilteredItem.SysType);
            if (FilteredItem.Selected != null)
                result = result && sysApp.Selected.Equals(FilteredItem.Selected);
            return result;
        }

    }
}
