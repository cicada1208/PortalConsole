using Lib;
using Lib.Wpf;
using Lib.Wpf.Routes;
using Models;
using Params;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;


namespace ViewModels
{
    public class RoleViewModel : BaseViewModel<RoleViewModel>
    {
        private Role _filteredItem;
        /// <summary>
        /// 篩選
        /// </summary>
        public Role FilteredItem
        {
            get
            {
                if (_filteredItem == null)
                {
                    _filteredItem = new Role()
                    {
                        Activate = true
                    };
                }
                return _filteredItem;
            }
            set => Set(ref _filteredItem, value);
        }

        private Role _selectedItem;
        /// <summary>
        /// 選取
        /// </summary>
        public Role SelectedItem
        {
            get => _selectedItem;
            set => Set(ref _selectedItem, value);
        }

        private ObservableCollection<Role> _dataList;
        /// <summary>
        /// 查詢清單
        /// </summary>
        public ObservableCollection<Role> DataList
        {
            get => _dataList;
            set => Set(ref _dataList, value);
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

            var result = await ApiUtil.HttpClientExAsync<ApiResult<List<Role>>>(
                UAACRoute.Service(), UAACRoute.Role.Controller,
                method: ApiParam.HttpVerbs.Get,
                queryParams: FilteredItem);

            if (!result.Succ) Global.PageSnackbar.MessageEnqueue(result.Msg);
            else DataList = new ObservableCollection<Role>(result.Data);

            ProgressShow = false;
        }

        private DelegateCommand<FrameworkElement> _insertCommand;
        public DelegateCommand<FrameworkElement> InsertCommand =>
            _insertCommand ?? (_insertCommand = new DelegateCommand<FrameworkElement>
            (OnInsert, (param) => (bool)Permission.AddAuth));
        private void OnInsert(FrameworkElement dialogContent)
        {
            var vm = new RoleEditViewModel();
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
            var vm = new RoleEditViewModel();
            vm.EditMode = EditParam.EditMode.UPDATE;
            vm.SetEditedItem(new Role { RoleId = SelectedItem?.RoleId });
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
