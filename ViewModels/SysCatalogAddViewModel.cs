using Lib;
using Lib.Wpf;
using Lib.Wpf.Routes;
using Models;
using Params;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows.Data;
using static MoreLinq.Extensions.DistinctByExtension;
using static Params.ApiParam;
using static Params.SysAppParam;

namespace ViewModels
{
    public class SysCatalogAddViewModel : BaseViewModel<SysCatalogAddViewModel>
    {
        private SysCatalogQueryParam _queryParam;
        /// <summary>
        /// 查詢參數
        /// </summary>
        public SysCatalogQueryParam QueryParam
        {
            get => _queryParam ?? (_queryParam = new SysCatalogQueryParam());
            set => Set(ref _queryParam, value);
        }

        private SysAppSelected _filteredItem;
        /// <summary>
        /// 篩選
        /// </summary>
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

        private ObservableCollection<SysAppSelected> _sysAppList;
        /// <summary>
        /// 查詢清單
        /// </summary>
        public ObservableCollection<SysAppSelected> SysAppList
        {
            get
            {
                if (_sysAppList == null)
                {
                    var result = ApiUtil.HttpClientEx<ApiResult<ObservableCollection<SysAppSelected>>>(
                        UAACRoute.Service(), UAACRoute.SysApp.GetSysCatalogAddList,
                        method: ApiParam.HttpVerbs.Get,
                        queryParams: QueryParam);
                    _sysAppList = result.Data;

                    CollectionView view = CollectionViewSource.GetDefaultView(_sysAppList) as CollectionView;
                    view.Filter = SysAppFilter;
                }
                return _sysAppList;
            }
            set => Set(ref _sysAppList, value);
        }

        private ObservableCollection<SysAppSelected> _sysIdList;
        /// <summary>
        /// 系統代碼(查詢清單除 Catalog)
        /// </summary>
        public ObservableCollection<SysAppSelected> SysIdList
        {
            get
            {
                if (_sysIdList == null)
                {
                    _sysIdList = new ObservableCollection<SysAppSelected>(SysAppList.Where(s =>
                    !(s.SysType == SysType.Catalog)));
                }
                return _sysIdList;
            }
            set => Set(ref _sysIdList, value);
        }

        private ObservableCollection<Item> _sysTypeList;
        /// <summary>
        /// 系統類別
        /// </summary>
        public ObservableCollection<Item> SysTypeList
        {
            get
            {
                if (_sysTypeList == null)
                {
                    var sysTypes = SysAppList.DistinctBy(s => s.SysType).Select(s => s.SysType);
                    _sysTypeList = new ObservableCollection<Item>(
                        EnumData<SysType>.GetFilterCollection().Where(t =>
                        t.Value == null || sysTypes.Contains((SysType?)t.Value)));
                }
                return _sysTypeList;
            }
            set => Set(ref _sysTypeList, value);
        }


        private void FilteredItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
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

        private DelegateCommand _okCommand;
        public DelegateCommand OKCommand =>
            _okCommand ?? (_okCommand = new DelegateCommand
            (OnOK));
        private void OnOK()
        {
            var selectedSysApps = SysAppList.Where(s => (bool)s.Selected).ToList();

            SysCatalogAddSysApp sysCatalogAddSysApp = new SysCatalogAddSysApp
            {
                RootId = QueryParam.RootId,
                CatalogId = QueryParam.CatalogId,
                MUserId = LoginViewModel.LoginUser.EmpId,
                SysApps = selectedSysApps
            };

            var result = ApiUtil.HttpClientEx<ApiResult<List<SysCatalog>>>(
                UAACRoute.Service(), UAACRoute.SysCatalog.BatchInsertSysApp,
                method: HttpVerbs.Post,
                body: sysCatalogAddSysApp);

            Global.PageSnackbar.MessageEnqueue(result.Msg);
            CloseDialog?.Invoke(result.Succ, result.Succ);
        }

        private DelegateCommand _cancelCommand;
        public DelegateCommand CancelCommand =>
            _cancelCommand ?? (_cancelCommand = new DelegateCommand
            (OnCancel));
        private void OnCancel() =>
            CloseDialog?.Invoke(true);
    }
}
