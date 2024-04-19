using Lib;
using Lib.Wpf;
using Lib.Wpf.Routes;
using Models;
using Params;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using static Params.ApiParam;
using static Params.FuncParam;
using static Params.SysAppParam;

namespace ViewModels
{
    public class FuncCatalogViewModel : BaseViewModel<FuncCatalogViewModel>
    {
        private ObservableCollection<Func> _rootList;
        /// <summary>
        /// Root 清單
        /// </summary>
        public ObservableCollection<Func> RootList
        {
            get => _rootList;
            set => Set(ref _rootList, value);
        }

        private Func _selectedRoot;
        /// <summary>
        /// 選取 Root
        /// </summary>
        public Func SelectedRoot
        {
            get => _selectedRoot;
            set => Set(ref _selectedRoot, value);
        }

        private ObservableCollection<FuncGroup> _treeList;
        /// <summary>
        /// 目錄階層清單
        /// </summary>
        public ObservableCollection<FuncGroup> TreeList
        {
            get => _treeList;
            set => Set(ref _treeList, value);
        }

        private FuncGroup _selectedTreeItem;
        /// <summary>
        /// 選取 TreeItem
        /// </summary>
        public FuncGroup SelectedTreeItem
        {
            get => _selectedTreeItem;
            set => Set(ref _selectedTreeItem, value);
        }

        private Func _filteredItem;
        /// <summary>
        /// 篩選
        /// </summary>
        public Func FilteredItem
        {
            get
            {
                if (_filteredItem == null)
                    _filteredItem = new Func() { FuncType = FuncType.Root };
                return _filteredItem;
            }
            set => Set(ref _filteredItem, value);
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
                        method: HttpVerbs.Get);
                    _sysIdList = new ObservableCollection<SysApp>(result.Data
                        .Where(s => !(s.SysType == SysType.Root || s.SysType == SysType.Catalog))
                        .OrderByDescending(s => s.Activate).ThenBy(s => s.SysId));
                }
                return _sysIdList;
            }
            set => Set(ref _sysIdList, value);
        }


        private DelegateCommand _queryRootCommand;
        public DelegateCommand QueryRootCommand =>
            _queryRootCommand ?? (_queryRootCommand = new DelegateCommand
            (OnQueryRoot, () => (bool)Permission.QueryAuth && !ProgressShow));
        /// <summary>
        /// 查詢 Root
        /// </summary>
        private async void OnQueryRoot()
        {
            if (FilteredItem.SysId.IsNullOrWhiteSpace())
            {
                Global.PageSnackbar.MessageEnqueue("未選取 System。");
                return;
            }

            ProgressShow = true;

            var result = await ApiUtil.HttpClientExAsync<ApiResult<List<Func>>>(
                UAACRoute.Service(), UAACRoute.Func.Controller,
                method: HttpVerbs.Get,
                queryParams: FilteredItem);

            if (!result.Succ) Global.PageSnackbar.MessageEnqueue(result.Msg);
            else
            {
                RootList = new ObservableCollection<Func>(result.Data
                    .OrderByDescending(s => s.Activate).ThenBy(s => s.FuncName));
                TreeList?.Clear();
            }

            ProgressShow = false;
        }

        private DelegateCommand _selectRootCommand;
        public DelegateCommand SelectRootCommand =>
            _selectRootCommand ?? (_selectRootCommand = new DelegateCommand
            (OnSelectRoot, () => !ProgressShow));
        /// <summary>
        /// 選取 Root，顯示目錄階層
        /// </summary>
        private async void OnSelectRoot()
        {
            if (SelectedRoot == null) return;

            ProgressShow = true;

            var result = await ApiUtil.HttpClientExAsync<ApiResult<ObservableCollection<FuncGroup>>>(
                UAACRoute.Service(), UAACRoute.Permission.GetEntireFuncGroup,
                method: HttpVerbs.Get,
                queryParams: new { rootId = SelectedRoot.FuncId });

            if (!result.Succ) Global.PageSnackbar.MessageEnqueue(result.Msg);
            else TreeList = result.Data;

            ProgressShow = false;
        }

        private DelegateCommand<FrameworkElement> _insertRootCommand;
        public DelegateCommand<FrameworkElement> InsertRootCommand =>
            _insertRootCommand ?? (_insertRootCommand = new DelegateCommand<FrameworkElement>
            (OnInsertRoot, (param) => (bool)Permission.AddAuth));
        /// <summary>
        /// 新增 Root
        /// </summary>
        private void OnInsertRoot(FrameworkElement dialogContent)
        {
            if (FilteredItem.SysId.IsNullOrWhiteSpace())
            {
                Global.PageSnackbar.MessageEnqueue("未選取 System。");
                return;
            }

            var vm = new FuncEditViewModel();
            vm.EditMode = EditParam.EditMode.INSERT;
            vm.CloseDialog = CloseRootEditDialog;
            vm.FuncTypeList = new ObservableCollection<Item>(EnumData<FuncType>.GetCollection()
                .Where(t => t.Value.Equals(FuncType.Root)));
            vm.EditedItem.SysId = FilteredItem.SysId;
            dialogContent.DataContext = vm;
            DialogContent = dialogContent;
            IsOpenDialog = true;
        }

        private DelegateCommand<FrameworkElement> _updateRootCommand;
        public DelegateCommand<FrameworkElement> UpdateRootCommand =>
            _updateRootCommand ?? (_updateRootCommand = new DelegateCommand<FrameworkElement>
            (OnUpdateRoot, (param) => (bool)Permission.ModifyAuth));
        /// <summary>
        /// 修改 Root
        /// </summary>
        private void OnUpdateRoot(FrameworkElement dialogContent)
        {
            if (SelectedRoot == null)
            {
                Global.PageSnackbar.MessageEnqueue("未選取 Root。");
                return;
            }
            var vm = new FuncEditViewModel();
            vm.EditMode = EditParam.EditMode.UPDATE;
            vm.SetEditedItem(new Func { FuncId = SelectedRoot.FuncId });
            vm.CloseDialog = CloseRootEditDialog;
            dialogContent.DataContext = vm;
            DialogContent = dialogContent;
            IsOpenDialog = true;
        }

        private void CloseRootEditDialog(bool close, bool? result)
        {
            if (close) IsOpenDialog = false;
            if (result.HasValue && result.Value) OnQueryRoot();
        }

        private DelegateCommand<FrameworkElement> _rootAddFuncCatalogCommand;
        public DelegateCommand<FrameworkElement> RootAddFuncCatalogCommand =>
            _rootAddFuncCatalogCommand ?? (_rootAddFuncCatalogCommand = new DelegateCommand<FrameworkElement>
            (OnRootAddFuncCatalog, (param) => (bool)Permission.AddAuth));
        /// <summary>
        /// Root 加入目錄階層
        /// </summary>
        private void OnRootAddFuncCatalog(FrameworkElement dialogContent)
        {
            if (SelectedRoot == null)
            {
                Global.PageSnackbar.MessageEnqueue("未選取 Root。");
                return;
            }
            var vm = new FuncCatalogAddViewModel();
            vm.EditMode = EditParam.EditMode.INSERT;
            vm.QueryParam.SysId = SelectedRoot.SysId;
            vm.QueryParam.RootId = SelectedRoot.FuncId;
            vm.QueryParam.CatalogId = SelectedRoot.FuncId;
            vm.CloseDialog = CloseAddFuncCatalogDialog;
            dialogContent.DataContext = vm;
            DialogContent = dialogContent;
            IsOpenDialog = true;
        }

        private DelegateCommand<FrameworkElement> _catalogAddFuncCatalogCommand;
        public DelegateCommand<FrameworkElement> CatalogAddFuncCatalogCommand =>
            _catalogAddFuncCatalogCommand ?? (_catalogAddFuncCatalogCommand = new DelegateCommand<FrameworkElement>
            (OnCatalogAddFuncCatalog, (param) => (bool)Permission.AddAuth));
        /// <summary>
        /// Catalog 加入目錄階層
        /// </summary>
        private void OnCatalogAddFuncCatalog(FrameworkElement dialogContent)
        {
            if (SelectedTreeItem == null || SelectedTreeItem.FuncType != FuncType.Catalog)
            {
                Global.PageSnackbar.MessageEnqueue("未選取 Catalog。");
                return;
            }
            var vm = new FuncCatalogAddViewModel();
            vm.EditMode = EditParam.EditMode.INSERT;
            vm.QueryParam.SysId = SelectedTreeItem.SysId;
            vm.QueryParam.RootId = SelectedTreeItem.RootId;
            vm.QueryParam.CatalogId = SelectedTreeItem.FuncId;
            vm.CloseDialog = CloseAddFuncCatalogDialog;
            dialogContent.DataContext = vm;
            DialogContent = dialogContent;
            IsOpenDialog = true;
        }

        private void CloseAddFuncCatalogDialog(bool close, bool? result)
        {
            if (close) IsOpenDialog = false;
            if (result.HasValue && result.Value) OnSelectRoot();
        }

        private DelegateCommand<FrameworkElement> _updateTreeItemCommand;
        public DelegateCommand<FrameworkElement> UpdateTreeItemCommand =>
            _updateTreeItemCommand ?? (_updateTreeItemCommand = new DelegateCommand<FrameworkElement>
            (OnUpdateTreeItem, (param) => (bool)Permission.ModifyAuth));
        /// <summary>
        /// 修改 TreeItem
        /// </summary>
        private void OnUpdateTreeItem(FrameworkElement dialogContent)
        {
            if (SelectedTreeItem == null)
            {
                Global.PageSnackbar.MessageEnqueue("未選取 TreeItem。");
                return;
            }
            var vm = new FuncEditViewModel();
            vm.EditMode = EditParam.EditMode.UPDATE;
            vm.SetEditedItem(new Func { FuncId = SelectedTreeItem.FuncId });
            vm.CloseDialog = CloseTreeItemEditDialog;
            dialogContent.DataContext = vm;
            DialogContent = dialogContent;
            IsOpenDialog = true;
        }

        private void CloseTreeItemEditDialog(bool close, bool? result)
        {
            if (close) IsOpenDialog = false;
            if (result.HasValue && result.Value) OnSelectRoot();
        }

        private DelegateCommand _treeItemRemoveFuncCatalogCommand;
        public DelegateCommand TreeItemRemoveFuncCatalogCommand =>
            _treeItemRemoveFuncCatalogCommand ?? (_treeItemRemoveFuncCatalogCommand = new DelegateCommand
            (OnTreeItemRemoveFuncCatalog, () => (bool)Permission.DeleteAuth && !ProgressShow));
        /// <summary>
        /// TreeItem 移除目錄階層
        /// </summary>
        private void OnTreeItemRemoveFuncCatalog()
        {
            if (SelectedTreeItem == null)
            {
                Global.PageSnackbar.MessageEnqueue("未選取 TreeItem。");
                return;
            }

            string msg = $"是否移除";
            if (SelectedTreeItem.FuncType == FuncType.Catalog)
                msg += $"目錄 {SelectedTreeItem.FuncName}，及其下所有子項。";
            else
                msg += $"{SelectedTreeItem.FuncName}。";

            Global.PageSnackbar.MessageEnqueue(msg,
                actionContent: $"移除",
                actionHandler: RemoveFuncCatalog,
                actionArgument: new FuncCatalogRemoveParam
                {
                    FuncCatalogId = SelectedTreeItem.FuncCatalogId,
                    RootId = SelectedTreeItem.RootId,
                    MUserId = LoginViewModel.LoginUser.EmpId
                }, durationOverride: TimeSpan.FromSeconds(5));
        }

        private async void RemoveFuncCatalog(FuncCatalogRemoveParam removeParam)
        {
            ProgressShow = true;
            var result = await ApiUtil.HttpClientExAsync<ApiResult<object>>(
                UAACRoute.Service(), UAACRoute.FuncCatalog.BatchPatchDeactivate,
                method: HttpVerbs.Patch,
                body: removeParam);

            Global.PageSnackbar.MessageEnqueue(result.Msg);
            ProgressShow = false;

            if (result.Succ) OnSelectRoot();
        }

        private DelegateCommand<FrameworkElement> _treeItemSortFuncCatalogCommand;
        public DelegateCommand<FrameworkElement> TreeItemSortFuncCatalogCommand =>
            _treeItemSortFuncCatalogCommand ?? (_treeItemSortFuncCatalogCommand = new DelegateCommand<FrameworkElement>
            (OnTreeItemSortFuncCatalog, (param) => (bool)Permission.ModifyAuth));
        /// <summary>
        /// TreeItem 排序目錄階層
        /// </summary>
        private void OnTreeItemSortFuncCatalog(FrameworkElement dialogContent)
        {
            if (SelectedTreeItem == null)
            {
                Global.PageSnackbar.MessageEnqueue("未選取 TreeItem。");
                return;
            }
            var vm = new FuncCatalogSortViewModel();
            vm.EditMode = EditParam.EditMode.UPDATE;
            vm.QueryParam.RootId = SelectedTreeItem.RootId;
            vm.QueryParam.CatalogId = SelectedTreeItem.CatalogId;
            vm.CloseDialog = CloseSortFuncCatalogDialog;
            dialogContent.DataContext = vm;
            DialogContent = dialogContent;
            IsOpenDialog = true;
        }

        private void CloseSortFuncCatalogDialog(bool close, bool? result)
        {
            if (close) IsOpenDialog = false;
            if (result.HasValue && result.Value) OnSelectRoot();
        }

    }
}
