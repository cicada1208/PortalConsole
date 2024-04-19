﻿using Lib;
using Lib.Wpf;
using Lib.Wpf.Routes;
using Models;
using Params;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Params.ApiParam;
using static Params.EditParam;

namespace ViewModels
{
    public class RolePermissionEditViewModel : BaseViewModel<RolePermissionEditViewModel>
    {
        private RolePermission _editedItem;
        /// <summary>
        /// 編輯
        /// </summary>
        public RolePermission EditedItem
        {
            get
            {
                if (_editedItem == null)
                {
                    _editedItem = new RolePermission
                    {
                        QueryAuth = false,
                        AddAuth = false,
                        ModifyAuth = false,
                        DeleteAuth = false,
                        ExportAuth = false,
                        PrintAuth = false,
                        Activate = true
                    };
                    _editedItem.PropertyChanged += EditedItemPropertyChanged;
                }
                return _editedItem;
            }
            set
            {
                Set(ref _editedItem, value);
                if (_editedItem != null)
                {
                    _editedItem.PropertyChanged -= EditedItemPropertyChanged;
                    _editedItem.PropertyChanged += EditedItemPropertyChanged;
                }
            }
        }

        private ObservableCollection<Role> _roleIdList;
        /// <summary>
        /// 角色群組代碼
        /// </summary>
        public ObservableCollection<Role> RoleIdList
        {
            get
            {
                if (_roleIdList == null)
                {
                    var result = ApiUtil.HttpClientEx<ApiResult<List<Role>>>(
                        UAACRoute.Service(), UAACRoute.Role.Controller,
                        method: ApiParam.HttpVerbs.Get,
                        queryParams: EditMode == EditMode.INSERT ? new Role { Activate = true } : null);
                    _roleIdList = new ObservableCollection<Role>(result.Data.OrderBy(r => r.RoleName));
                }
                return _roleIdList;
            }
            set => Set(ref _roleIdList, value);
        }

        private ObservableCollection<SysApp> _sysIdList;
        /// <summary>
        /// 系統代碼(除 Root & Catalog 且作用中)
        /// </summary>
        public ObservableCollection<SysApp> SysIdList
        {
            get
            {
                if (_sysIdList == null)
                {
                    var result = ApiUtil.HttpClientEx<ApiResult<List<SysApp>>>(
                        UAACRoute.Service(), UAACRoute.SysApp.Controller,
                        method: ApiParam.HttpVerbs.Get,
                        queryParams: new SysApp { Activate = true });
                    _sysIdList = new ObservableCollection<SysApp>(result.Data.Where(s =>
                    !(s.SysType == SysAppParam.SysType.Root || s.SysType == SysAppParam.SysType.Catalog)));
                }
                return _sysIdList;
            }
            set => Set(ref _sysIdList, value);
        }

        private ObservableCollection<Func> _funcIdList;
        /// <summary>
        /// 功能代碼
        /// </summary>
        public ObservableCollection<Func> FuncIdList
        {
            get => _funcIdList;
            set => Set(ref _funcIdList, value);
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
                    _activateList = ActivateData.GetCollection();
                return _activateList;
            }
            set => Set(ref _activateList, value);
        }

        private Func<Task> _queryFuncListDebounce;
        private Func<Task> QueryFuncListDebounce => _queryFuncListDebounce ??
            (_queryFuncListDebounce = ((Func<CancellationToken, Task>)QueryFuncList).Debounce());


        private void EditedItemPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            (sender as RolePermission).PropertyChanged -= EditedItemPropertyChanged;
            if (e.PropertyName == nameof(RolePermission.QueryAuth) ||
                e.PropertyName == nameof(RolePermission.AddAuth) ||
                e.PropertyName == nameof(RolePermission.ModifyAuth) ||
                e.PropertyName == nameof(RolePermission.DeleteAuth) ||
                e.PropertyName == nameof(RolePermission.ExportAuth) ||
                e.PropertyName == nameof(RolePermission.PrintAuth))
            {
                if (e.PropertyName != nameof(RolePermission.QueryAuth))
                    (sender as RolePermission).RaisePropertyChanged(nameof(RolePermission.QueryAuth));
                if (e.PropertyName != nameof(RolePermission.AddAuth))
                    (sender as RolePermission).RaisePropertyChanged(nameof(RolePermission.AddAuth));
                if (e.PropertyName != nameof(RolePermission.ModifyAuth))
                    (sender as RolePermission).RaisePropertyChanged(nameof(RolePermission.ModifyAuth));
                if (e.PropertyName != nameof(RolePermission.DeleteAuth))
                    (sender as RolePermission).RaisePropertyChanged(nameof(RolePermission.DeleteAuth));
                if (e.PropertyName != nameof(RolePermission.ExportAuth))
                    (sender as RolePermission).RaisePropertyChanged(nameof(RolePermission.ExportAuth));
                if (e.PropertyName != nameof(RolePermission.PrintAuth))
                    (sender as RolePermission).RaisePropertyChanged(nameof(RolePermission.PrintAuth));
            }
            (sender as RolePermission).PropertyChanged += EditedItemPropertyChanged;
        }

        private DelegateCommand _selectSysIdCommand;
        public DelegateCommand SelectSysIdCommand =>
            _selectSysIdCommand ?? (_selectSysIdCommand = new DelegateCommand
            (OnSelectSysId));
        /// <summary>
        /// 選取系統代碼，查詢功能代碼
        /// </summary>
        private void OnSelectSysId()
        {
            FuncIdList?.Clear();
            QueryFuncListDebounce();
        }

        private async Task QueryFuncList(CancellationToken cancellationToken = default)
        {
            if (!EditedItem.SysId.IsNullOrWhiteSpace())
            {
                var result = await ApiUtil.HttpClientExAsync<ApiResult<ObservableCollection<Func>>>(
                 UAACRoute.Service(), UAACRoute.Func.Controller,
                 method: ApiParam.HttpVerbs.Get,
                 queryParams: new Func { SysId = EditedItem.SysId, Activate = true },
                 cancellationToken: cancellationToken);

                if (result.Succ)
                    FuncIdList = new ObservableCollection<Func>(result.Data
                        .Where(f => !(f.FuncType == FuncParam.FuncType.Root || f.FuncType == FuncParam.FuncType.Catalog))
                        .OrderBy(f => f.FuncName));
            }
        }

        private DelegateCommand _okCommand;
        public DelegateCommand OKCommand =>
            _okCommand ?? (_okCommand = new DelegateCommand
            (OnOK, () => Validate().IsValid));
        private void OnOK()
        {
            EditedItem.MUserId = LoginViewModel.LoginUser.EmpId;

            var result = ApiUtil.HttpClientEx<ApiResult<RolePermission>>(
                UAACRoute.Service(), UAACRoute.RolePermission.Controller,
                method: EditMode == EditMode.INSERT ? HttpVerbs.Post : HttpVerbs.Put,
                body: EditedItem);

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
