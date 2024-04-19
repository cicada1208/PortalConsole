using Lib;
using Lib.Wpf;
using Lib.Wpf.Routes;
using Models;
using Params;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using static Params.FuncParam;

namespace ViewModels
{
    public class FuncViewModel : BaseViewModel<FuncViewModel>
    {
        private Func _filteredItem;
        /// <summary>
        /// 篩選
        /// </summary>
        public Func FilteredItem
        {
            get
            {
                if (_filteredItem == null)
                {
                    _filteredItem = new Func()
                    {
                        Activate = true
                    };
                }
                return _filteredItem;
            }
            set => Set(ref _filteredItem, value);
        }

        private Func _selectedItem;
        /// <summary>
        /// 選取
        /// </summary>
        public Func SelectedItem
        {
            get => _selectedItem;
            set => Set(ref _selectedItem, value);
        }

        private ObservableCollection<Func> _dataList;
        /// <summary>
        /// 查詢清單
        /// </summary>
        public ObservableCollection<Func> DataList
        {
            get => _dataList;
            set => Set(ref _dataList, value);
        }

        private ObservableCollection<SysApp> _sysIdList;
        /// <summary>
        /// 系統代碼(除 Root & Catalog)
        /// </summary>
        public ObservableCollection<SysApp> SysIdList
        {
            get
            {
                if (_sysIdList == null)
                {
                    var result = ApiUtil.HttpClientEx<ApiResult<List<SysApp>>>(
                        UAACRoute.Service(), UAACRoute.SysApp.Controller,
                        method: ApiParam.HttpVerbs.Get);
                    _sysIdList = new ObservableCollection<SysApp>(result.Data
                        .Where(s => !(s.SysType == SysAppParam.SysType.Root || s.SysType == SysAppParam.SysType.Catalog))
                        .OrderByDescending(s => s.Activate).ThenBy(s => s.SysId));
                }
                return _sysIdList;
            }
            set => Set(ref _sysIdList, value);
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
                    _funcTypeList = EnumData<FuncType>.GetFilterCollection();
                return _funcTypeList;
            }
            set => Set(ref _funcTypeList, value);
        }

        private ObservableCollection<Item> _activateList;
        /// <summary>
        /// 啟用狀態
        /// </summary>
        public ObservableCollection<Item> ActivateList
        {
            get
            {
                if (_activateList == null)
                    _activateList = ActivateData.GetFilterCollection();
                return _activateList;
            }
            set => Set(ref _activateList, value);
        }

        private ObservableCollection<Func> _viewNameList;
        /// <summary>
        /// 視圖名稱(篩選系統代碼的子項)
        /// </summary>
        public ObservableCollection<Func> ViewNameList
        {
            get => _viewNameList;
            set => Set(ref _viewNameList, value);
        }

        private Func<Task> _queryViewNameListDebounce;
        private Func<Task> QueryViewNameListDebounce => _queryViewNameListDebounce ??
            (_queryViewNameListDebounce = ((Func<CancellationToken, Task>)QueryViewNameList).Debounce(2000));


        private DelegateCommand _filterSysIdCommand;
        public DelegateCommand FilterSysIdCommand =>
            _filterSysIdCommand ?? (_filterSysIdCommand = new DelegateCommand
            (OnFilterSysId));
        /// <summary>
        /// 選取系統代碼，查詢視圖名稱
        /// </summary>
        private void OnFilterSysId()
        {
            ViewNameList?.Clear();
            QueryViewNameListDebounce();
        }

        private async Task QueryViewNameList(CancellationToken cancellationToken = default)
        {
            if (!FilteredItem.SysId.IsNullOrWhiteSpace())
            {
                var result = await ApiUtil.HttpClientExAsync<ApiResult<ObservableCollection<Func>>>(
                 UAACRoute.Service(), UAACRoute.Func.Controller,
                 method: ApiParam.HttpVerbs.Get,
                 queryParams: new Func { SysId = FilteredItem.SysId },
                 cancellationToken: cancellationToken);

                if (result.Succ)
                    ViewNameList = new ObservableCollection<Func>(result.Data
                        .Where(f => !f.ViewName.IsNullOrWhiteSpace())
                        .OrderByDescending(f => f.Activate).ThenBy(f => f.ViewName));
            }
        }

        private DelegateCommand _queryCommand;
        public DelegateCommand QueryCommand =>
            _queryCommand ?? (_queryCommand = new DelegateCommand
            (OnQuery, () => (bool)Permission.QueryAuth && !ProgressShow));
        /// <summary>
        /// 查詢
        /// </summary>
        private async void OnQuery()
        {
            ProgressShow = true;

            var result = await ApiUtil.HttpClientExAsync<ApiResult<List<Func>>>(
                UAACRoute.Service(), UAACRoute.Func.Controller,
                method: ApiParam.HttpVerbs.Get,
                queryParams: FilteredItem);

            if (!result.Succ) Global.PageSnackbar.MessageEnqueue(result.Msg);
            else DataList = new ObservableCollection<Func>(result.Data);

            ProgressShow = false;
        }

        private DelegateCommand<FrameworkElement> _insertCommand;
        public DelegateCommand<FrameworkElement> InsertCommand =>
            _insertCommand ?? (_insertCommand = new DelegateCommand<FrameworkElement>
            (OnInsert, (param) => (bool)Permission.AddAuth));
        private void OnInsert(FrameworkElement dialogContent)
        {
            var vm = new FuncEditViewModel();
            vm.EditMode = EditParam.EditMode.INSERT;
            vm.CloseDialog = CloseEditDialog;
            dialogContent.DataContext = vm;
            DialogContent = dialogContent;
            IsOpenDialog = true;
        }

        private DelegateCommand<FrameworkElement> _editCommand;
        public DelegateCommand<FrameworkElement> EditCommand =>
            _editCommand ?? (_editCommand = new DelegateCommand<FrameworkElement>
            (OnEdit, (param) => (bool)Permission.ModifyAuth));
        /// <summary>
        /// 編輯
        /// </summary>
        private void OnEdit(FrameworkElement dialogContent)
        {
            var vm = new FuncEditViewModel();
            vm.EditMode = EditParam.EditMode.UPDATE;
            vm.SetEditedItem(new Func { FuncId = SelectedItem?.FuncId });
            vm.CloseDialog = CloseEditDialog;
            dialogContent.DataContext = vm;
            DialogContent = dialogContent;
            IsOpenDialog = true;
        }

        private void CloseEditDialog(bool close, bool? result)
        {
            if (close) IsOpenDialog = false;
            if (result.HasValue && result.Value) OnQuery();
        }

    }
}
