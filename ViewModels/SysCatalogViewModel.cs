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
using static Params.SysAppParam;

namespace ViewModels
{
    public class SysCatalogViewModel : BaseViewModel<SysCatalogViewModel>
    {
        private ObservableCollection<SysApp> _rootList;
        /// <summary>
        /// Root 清單
        /// </summary>
        public ObservableCollection<SysApp> RootList
        {
            get => _rootList;
            set => Set(ref _rootList, value);
        }

        private SysApp _selectedRoot;
        /// <summary>
        /// 選取 Root
        /// </summary>
        public SysApp SelectedRoot
        {
            get => _selectedRoot;
            set => Set(ref _selectedRoot, value);
        }

        private ObservableCollection<SysGroup> _treeList;
        /// <summary>
        /// 目錄階層清單
        /// </summary>
        public ObservableCollection<SysGroup> TreeList
        {
            get => _treeList;
            set => Set(ref _treeList, value);
        }

        private SysGroup _selectedTreeItem;
        /// <summary>
        /// 選取 TreeItem
        /// </summary>
        public SysGroup SelectedTreeItem
        {
            get => _selectedTreeItem;
            set => Set(ref _selectedTreeItem, value);
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
            ProgressShow = true;

            var result = await ApiUtil.HttpClientExAsync<ApiResult<List<SysApp>>>(
                UAACRoute.Service(), UAACRoute.SysApp.Controller,
                method: ApiParam.HttpVerbs.Get,
                queryParams: new SysApp { SysType = SysType.Root });

            if (!result.Succ) Global.PageSnackbar.MessageEnqueue(result.Msg);
            else
            {
                RootList = new ObservableCollection<SysApp>(result.Data
                    .OrderByDescending(s => s.Activate).ThenBy(s => s.SysId));
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

            var result = await ApiUtil.HttpClientExAsync<ApiResult<ObservableCollection<SysGroup>>>(
                UAACRoute.Service(), UAACRoute.Permission.GetEntireSysGroup,
                method: ApiParam.HttpVerbs.Get,
                queryParams: new { rootId = SelectedRoot.SysId });

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
            var vm = new SysAppEditViewModel();
            vm.EditMode = EditParam.EditMode.INSERT;
            vm.CloseDialog = CloseRootEditDialog;
            vm.SysTypeList = new ObservableCollection<Item>(EnumData<SysType>.GetCollection()
                .Where(t => t.Value.Equals(SysType.Root)));
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
            var vm = new SysAppEditViewModel();
            vm.EditMode = EditParam.EditMode.UPDATE;
            vm.SetEditedItem(new SysApp { SysId = SelectedRoot.SysId });
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

        private DelegateCommand<FrameworkElement> _rootAddSysCatalogCommand;
        public DelegateCommand<FrameworkElement> RootAddSysCatalogCommand =>
            _rootAddSysCatalogCommand ?? (_rootAddSysCatalogCommand = new DelegateCommand<FrameworkElement>
            (OnRootAddSysCatalog, (param) => (bool)Permission.AddAuth));
        /// <summary>
        /// Root 加入目錄階層
        /// </summary>
        private void OnRootAddSysCatalog(FrameworkElement dialogContent)
        {
            if (SelectedRoot == null)
            {
                Global.PageSnackbar.MessageEnqueue("未選取 Root。");
                return;
            }
            var vm = new SysCatalogAddViewModel();
            vm.EditMode = EditParam.EditMode.INSERT;
            vm.QueryParam.RootId = SelectedRoot.SysId;
            vm.QueryParam.CatalogId = SelectedRoot.SysId;
            vm.CloseDialog = CloseAddSysCatalogDialog;
            dialogContent.DataContext = vm;
            DialogContent = dialogContent;
            IsOpenDialog = true;
        }

        private DelegateCommand<FrameworkElement> _catalogAddSysCatalogCommand;
        public DelegateCommand<FrameworkElement> CatalogAddSysCatalogCommand =>
            _catalogAddSysCatalogCommand ?? (_catalogAddSysCatalogCommand = new DelegateCommand<FrameworkElement>
            (OnCatalogAddSysCatalog, (param) => (bool)Permission.AddAuth));
        /// <summary>
        /// Catalog 加入目錄階層
        /// </summary>
        private void OnCatalogAddSysCatalog(FrameworkElement dialogContent)
        {
            if (SelectedTreeItem == null || SelectedTreeItem.SysType != SysType.Catalog)
            {
                Global.PageSnackbar.MessageEnqueue("未選取 Catalog。");
                return;
            }
            var vm = new SysCatalogAddViewModel();
            vm.EditMode = EditParam.EditMode.INSERT;
            vm.QueryParam.RootId = SelectedTreeItem.RootId;
            vm.QueryParam.CatalogId = SelectedTreeItem.SysId;
            vm.CloseDialog = CloseAddSysCatalogDialog;
            dialogContent.DataContext = vm;
            DialogContent = dialogContent;
            IsOpenDialog = true;
        }

        private void CloseAddSysCatalogDialog(bool close, bool? result)
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
            var vm = new SysAppEditViewModel();
            vm.EditMode = EditParam.EditMode.UPDATE;
            vm.SetEditedItem(new SysApp { SysId = SelectedTreeItem.SysId });
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

        private DelegateCommand _treeItemRemoveSysCatalogCommand;
        public DelegateCommand TreeItemRemoveSysCatalogCommand =>
            _treeItemRemoveSysCatalogCommand ?? (_treeItemRemoveSysCatalogCommand = new DelegateCommand
            (OnTreeItemRemoveSysCatalog, () => (bool)Permission.DeleteAuth && !ProgressShow));
        /// <summary>
        /// TreeItem 移除目錄階層
        /// </summary>
        private void OnTreeItemRemoveSysCatalog()
        {
            if (SelectedTreeItem == null)
            {
                Global.PageSnackbar.MessageEnqueue("未選取 TreeItem。");
                return;
            }

            string msg = $"是否移除";
            if (SelectedTreeItem.SysType == SysType.Catalog)
                msg += $"目錄 {SelectedTreeItem.SysName}，及其下所有子項。";
            else
                msg += $"{SelectedTreeItem.SysName}。";

            Global.PageSnackbar.MessageEnqueue(msg,
                actionContent: $"移除",
                actionHandler: RemoveSysCatalog,
                actionArgument: new SysCatalogRemoveParam
                {
                    SysCatalogId = SelectedTreeItem.SysCatalogId,
                    RootId = SelectedTreeItem.RootId,
                    MUserId = LoginViewModel.LoginUser.EmpId
                }, durationOverride: TimeSpan.FromSeconds(5));
        }

        private async void RemoveSysCatalog(SysCatalogRemoveParam removeParam)
        {
            ProgressShow = true;
            var result = await ApiUtil.HttpClientExAsync<ApiResult<object>>(
                UAACRoute.Service(), UAACRoute.SysCatalog.BatchPatchDeactivate,
                method: HttpVerbs.Patch,
                body: removeParam);

            Global.PageSnackbar.MessageEnqueue(result.Msg);
            ProgressShow = false;

            if (result.Succ) OnSelectRoot();
        }

        private DelegateCommand<FrameworkElement> _treeItemSortSysCatalogCommand;
        public DelegateCommand<FrameworkElement> TreeItemSortSysCatalogCommand =>
            _treeItemSortSysCatalogCommand ?? (_treeItemSortSysCatalogCommand = new DelegateCommand<FrameworkElement>
            (OnTreeItemSortSysCatalog, (param) => (bool)Permission.ModifyAuth));
        /// <summary>
        /// TreeItem 排序目錄階層
        /// </summary>
        private void OnTreeItemSortSysCatalog(FrameworkElement dialogContent)
        {
            if (SelectedTreeItem == null)
            {
                Global.PageSnackbar.MessageEnqueue("未選取 TreeItem。");
                return;
            }
            var vm = new SysCatalogSortViewModel();
            vm.EditMode = EditParam.EditMode.UPDATE;
            vm.QueryParam.RootId = SelectedTreeItem.RootId;
            vm.QueryParam.CatalogId = SelectedTreeItem.CatalogId;
            vm.CloseDialog = CloseSortSysCatalogDialog;
            dialogContent.DataContext = vm;
            DialogContent = dialogContent;
            IsOpenDialog = true;
        }

        private void CloseSortSysCatalogDialog(bool close, bool? result)
        {
            if (close) IsOpenDialog = false;
            if (result.HasValue && result.Value) OnSelectRoot();
        }

    }
}
