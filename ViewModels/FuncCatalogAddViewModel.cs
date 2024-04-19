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
using static Params.FuncParam;

namespace ViewModels
{
    public class FuncCatalogAddViewModel : BaseViewModel<FuncCatalogAddViewModel>
    {
        private FuncCatalogQueryParam _queryParam;
        /// <summary>
        /// 查詢參數
        /// </summary>
        public FuncCatalogQueryParam QueryParam
        {
            get => _queryParam ?? (_queryParam = new FuncCatalogQueryParam());
            set => Set(ref _queryParam, value);
        }

        private FuncSelected _filteredItem;
        /// <summary>
        /// 篩選
        /// </summary>
        public FuncSelected FilteredItem
        {
            get
            {
                if (_filteredItem == null)
                {
                    _filteredItem = new FuncSelected();
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

        private ObservableCollection<FuncSelected> _funcList;
        /// <summary>
        /// 查詢清單
        /// </summary>
        public ObservableCollection<FuncSelected> FuncList
        {
            get
            {
                if (_funcList == null)
                {
                    var result = ApiUtil.HttpClientEx<ApiResult<ObservableCollection<FuncSelected>>>(
                        UAACRoute.Service(), UAACRoute.Func.GetFuncCatalogAddList,
                        method: ApiParam.HttpVerbs.Get,
                        queryParams: QueryParam);
                    _funcList = result.Data;

                    CollectionView view = CollectionViewSource.GetDefaultView(_funcList) as CollectionView;
                    view.Filter = FuncFilter;
                }
                return _funcList;
            }
            set => Set(ref _funcList, value);
        }

        private ObservableCollection<Item> _funcTypeList;
        /// <summary>
        /// 功能類別
        /// </summary>
        public ObservableCollection<Item> FuncTypeList
        {
            get
            {
                if (_funcTypeList == null)
                {
                    var funcTypes = FuncList.DistinctBy(s => s.FuncType).Select(s => s.FuncType);
                    _funcTypeList = new ObservableCollection<Item>(
                        EnumData<FuncType>.GetFilterCollection().Where(t =>
                        t.Value == null || funcTypes.Contains((FuncType?)t.Value)));
                }
                return _funcTypeList;
            }
            set => Set(ref _funcTypeList, value);
        }

        private ObservableCollection<FuncSelected> _funcNameList;
        /// <summary>
        /// 功能名稱
        /// </summary>
        public ObservableCollection<FuncSelected> FuncNameList
        {
            get
            {
                if (_funcNameList == null)
                    _funcNameList = new ObservableCollection<FuncSelected>(FuncList.OrderBy(f => f.FuncName));
                return _funcNameList;
            }
            set => Set(ref _funcNameList, value);
        }

        private ObservableCollection<FuncSelected> _viewNameList;
        /// <summary>
        /// 視圖名稱
        /// </summary>
        public ObservableCollection<FuncSelected> ViewNameList
        {
            get
            {
                if (_viewNameList == null)
                {
                    _viewNameList = new ObservableCollection<FuncSelected>(FuncList
                        .Where(f => !f.ViewName.IsNullOrWhiteSpace())
                        .OrderBy(f => f.ViewName));
                }
                return _viewNameList;
            }
            set => Set(ref _viewNameList, value);
        }


        private void FilteredItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(FuncSelected.FuncType) ||
                e.PropertyName == nameof(FuncSelected.FuncName) ||
                e.PropertyName == nameof(FuncSelected.ViewName) ||
                e.PropertyName == nameof(FuncSelected.Selected))
                CollectionViewSource.GetDefaultView(FuncList)?.Refresh();
        }

        private bool FuncFilter(object item)
        {
            bool result = true;
            FuncSelected func = item as FuncSelected;
            if (FilteredItem.FuncType != null)
                result = result && func.FuncType.Equals(FilteredItem.FuncType);
            if (!FilteredItem.FuncName.IsNullOrWhiteSpace())
                result = result && func.FuncName.Contains(FilteredItem.FuncName);
            if (!FilteredItem.ViewName.IsNullOrWhiteSpace())
                result = result && func.ViewName.Contains(FilteredItem.ViewName);
            if (FilteredItem.Selected != null)
                result = result && func.Selected.Equals(FilteredItem.Selected);
            return result;
        }

        private DelegateCommand _okCommand;
        public DelegateCommand OKCommand =>
            _okCommand ?? (_okCommand = new DelegateCommand
            (OnOK));
        private void OnOK()
        {
            var selectedFuncs = FuncList.Where(s => (bool)s.Selected).ToList();

            FuncCatalogAddFunc funcCatalogAddFunc = new FuncCatalogAddFunc
            {
                RootId = QueryParam.RootId,
                CatalogId = QueryParam.CatalogId,
                MUserId = LoginViewModel.LoginUser.EmpId,
                Funcs = selectedFuncs
            };

            var result = ApiUtil.HttpClientEx<ApiResult<List<FuncCatalog>>>(
                UAACRoute.Service(), UAACRoute.FuncCatalog.BatchInsertFunc,
                method: HttpVerbs.Post,
                body: funcCatalogAddFunc);

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
