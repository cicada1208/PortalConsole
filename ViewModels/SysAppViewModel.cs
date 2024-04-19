using Lib;
using Lib.Wpf;
using Lib.Wpf.Routes;
using Models;
using Params;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using static Params.SysAppParam;

namespace ViewModels
{
    public class SysAppViewModel : BaseViewModel<SysAppViewModel>
    {
        private SysApp _filteredItem;
        /// <summary>
        /// 篩選
        /// </summary>
        public SysApp FilteredItem
        {
            get
            {
                if (_filteredItem == null)
                {
                    _filteredItem = new SysApp()
                    {
                        Activate = true
                    };
                }
                return _filteredItem;
            }
            set => Set(ref _filteredItem, value);
        }

        private SysApp _selectedItem;
        /// <summary>
        /// 選取
        /// </summary>
        public SysApp SelectedItem
        {
            get => _selectedItem;
            set => Set(ref _selectedItem, value);
        }

        private ObservableCollection<SysApp> _dataList;
        /// <summary>
        /// 查詢清單
        /// </summary>
        public ObservableCollection<SysApp> DataList
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
                        .Where(s => !(s.SysType == SysType.Root || s.SysType == SysType.Catalog))
                        .OrderByDescending(s => s.Activate).ThenBy(s => s.SysId));
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
                    _sysTypeList = EnumData<SysType>.GetFilterCollection();
                return _sysTypeList;
            }
            set => Set(ref _sysTypeList, value);
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

            var result = await ApiUtil.HttpClientExAsync<ApiResult<List<SysApp>>>(
                UAACRoute.Service(), UAACRoute.SysApp.Controller,
                method: ApiParam.HttpVerbs.Get,
                queryParams: FilteredItem);

            if (!result.Succ) Global.PageSnackbar.MessageEnqueue(result.Msg);
            else DataList = new ObservableCollection<SysApp>(result.Data);

            ProgressShow = false;
        }

        private DelegateCommand<FrameworkElement> _insertCommand;
        public DelegateCommand<FrameworkElement> InsertCommand =>
            _insertCommand ?? (_insertCommand = new DelegateCommand<FrameworkElement>
            (OnInsert, (param) => (bool)Permission.AddAuth));
        private void OnInsert(FrameworkElement dialogContent)
        {
            var vm = new SysAppEditViewModel();
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
            var vm = new SysAppEditViewModel();
            vm.EditMode = EditParam.EditMode.UPDATE;
            vm.SetEditedItem(new SysApp { SysId = SelectedItem?.SysId });
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
