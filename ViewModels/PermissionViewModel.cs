using Lib;
using Lib.Wpf;
using Lib.Wpf.Routes;
using Models;
using Params;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using static Params.ApiParam;

namespace ViewModels
{
    public class PermissionViewModel : BaseViewModel<PermissionViewModel>
    {
        public PermissionViewModel()
        {
            InitHrInfo();
        }

        private RoleUserPermissionParam _filteredItem;
        /// <summary>
        /// 篩選
        /// </summary>
        public RoleUserPermissionParam FilteredItem
        {
            get
            {
                if (_filteredItem == null)
                {
                    _filteredItem = new RoleUserPermissionParam()
                    {
                        Option = RoleUserPermissionParamOption.ByUserInfo,
                        Activate = true
                    };
                }
                return _filteredItem;
            }
            set => Set(ref _filteredItem, value);
        }

        public bool IsFilterUserIdEnabled
        {
            get
            {
                switch (FilteredItem.Option)
                {
                    case RoleUserPermissionParamOption.ByUserInfo:
                        return true;
                    default:
                        FilteredItem.UserId = string.Empty;
                        return false;
                }
            }
        }

        public bool IsFilterRoleIdEnabled
        {
            get
            {
                switch (FilteredItem.Option)
                {
                    case RoleUserPermissionParamOption.ByRoleId:
                        return true;
                    default:
                        FilteredItem.RoleId = string.Empty;
                        return false;
                }
            }
        }

        private RoleUser _roleUserSelectedItem;
        /// <summary>
        /// 選取角色群組使用者
        /// </summary>
        public RoleUser RoleUserSelectedItem
        {
            get => _roleUserSelectedItem;
            set => Set(ref _roleUserSelectedItem, value);
        }

        private ObservableCollection<RoleUser> _roleUserList;
        /// <summary>
        /// 查詢角色群組使用者清單
        /// </summary>
        public ObservableCollection<RoleUser> RoleUserList
        {
            get => _roleUserList;
            set => Set(ref _roleUserList, value);
        }

        private ObservableCollection<RolePermission> _rolePermissionList;
        /// <summary>
        /// 查詢角色群組權限清單
        /// </summary>
        public ObservableCollection<RolePermission> RolePermissionList
        {
            get => _rolePermissionList;
            set
            {
                Set(ref _rolePermissionList, value);
                if (_rolePermissionList != null)
                {
                    foreach (var p in _rolePermissionList)
                    {
                        p.PropertyChanged -= RolePermissionAllSelectedPropertyChanged;
                        p.PropertyChanged += RolePermissionAllSelectedPropertyChanged;
                        p.PropertyChanged -= RolePermissionValidationPropertyChanged;
                        p.PropertyChanged += RolePermissionValidationPropertyChanged;
                    }
                }
            }
        }
        public bool? IsRolePermissionQueryAuthAllSelected
        {
            get
            {
                var selected = RolePermissionList?.Select(p => p.QueryAuth).Distinct().ToList();
                if (selected == null) return false;
                return selected.Count == 1 ? selected.Single() : null;
            }
            set
            {
                if (value.HasValue && RolePermissionList != null)
                {
                    foreach (var p in RolePermissionList)
                        p.QueryAuth = value.Value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool? IsRolePermissionAddAuthAllSelected
        {
            get
            {
                var selected = RolePermissionList?.Select(p => p.AddAuth).Distinct().ToList();
                if (selected == null) return false;
                return selected.Count == 1 ? selected.Single() : null;
            }
            set
            {
                if (value.HasValue && RolePermissionList != null)
                {
                    foreach (var p in RolePermissionList)
                        p.AddAuth = value.Value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool? IsRolePermissionModifyAuthAllSelected
        {
            get
            {
                var selected = RolePermissionList?.Select(p => p.ModifyAuth).Distinct().ToList();
                if (selected == null) return false;
                return selected.Count == 1 ? selected.Single() : null;
            }
            set
            {
                if (value.HasValue && RolePermissionList != null)
                {
                    foreach (var p in RolePermissionList)
                        p.ModifyAuth = value.Value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool? IsRolePermissionDeleteAuthAllSelected
        {
            get
            {
                var selected = RolePermissionList?.Select(p => p.DeleteAuth).Distinct().ToList();
                if (selected == null) return false;
                return selected.Count == 1 ? selected.Single() : null;
            }
            set
            {
                if (value.HasValue && RolePermissionList != null)
                {
                    foreach (var p in RolePermissionList)
                        p.DeleteAuth = value.Value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool? IsRolePermissionExportAuthAllSelected
        {
            get
            {
                var selected = RolePermissionList?.Select(p => p.ExportAuth).Distinct().ToList();
                if (selected == null) return false;
                return selected.Count == 1 ? selected.Single() : null;
            }
            set
            {
                if (value.HasValue && RolePermissionList != null)
                {
                    foreach (var p in RolePermissionList)
                        p.ExportAuth = value.Value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool? IsRolePermissionPrintAuthAllSelected
        {
            get
            {
                var selected = RolePermissionList?.Select(p => p.PrintAuth).Distinct().ToList();
                if (selected == null) return false;
                return selected.Count == 1 ? selected.Single() : null;
            }
            set
            {
                if (value.HasValue && RolePermissionList != null)
                {
                    foreach (var p in RolePermissionList)
                        p.PrintAuth = value.Value;
                    RaisePropertyChanged();
                }
            }
        }

        private ObservableCollection<UserPermission> _userPermissionList;
        /// <summary>
        /// 查詢使用者權限清單
        /// </summary>
        public ObservableCollection<UserPermission> UserPermissionList
        {
            get => _userPermissionList;
            set
            {
                Set(ref _userPermissionList, value);
                if (_userPermissionList != null)
                {
                    foreach (var p in _userPermissionList)
                    {
                        p.PropertyChanged -= UserPermissionAllSelectedPropertyChanged;
                        p.PropertyChanged += UserPermissionAllSelectedPropertyChanged;
                        p.PropertyChanged -= UserPermissionValidationPropertyChanged;
                        p.PropertyChanged += UserPermissionValidationPropertyChanged;
                    }
                }
            }
        }

        public bool? IsUserPermissionQueryAuthAllSelected
        {
            get
            {
                var selected = UserPermissionList?.Select(p => p.QueryAuth).Distinct().ToList();
                if (selected == null) return false;
                return selected.Count == 1 ? selected.Single() : null;
            }
            set
            {
                if (value.HasValue && UserPermissionList != null)
                {
                    foreach (var p in UserPermissionList)
                        p.QueryAuth = value.Value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool? IsUserPermissionAddAuthAllSelected
        {
            get
            {
                var selected = UserPermissionList?.Select(p => p.AddAuth).Distinct().ToList();
                if (selected == null) return false;
                return selected.Count == 1 ? selected.Single() : null;
            }
            set
            {
                if (value.HasValue && UserPermissionList != null)
                {
                    foreach (var p in UserPermissionList)
                        p.AddAuth = value.Value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool? IsUserPermissionModifyAuthAllSelected
        {
            get
            {
                var selected = UserPermissionList?.Select(p => p.ModifyAuth).Distinct().ToList();
                if (selected == null) return false;
                return selected.Count == 1 ? selected.Single() : null;
            }
            set
            {
                if (value.HasValue && UserPermissionList != null)
                {
                    foreach (var p in UserPermissionList)
                        p.ModifyAuth = value.Value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool? IsUserPermissionDeleteAuthAllSelected
        {
            get
            {
                var selected = UserPermissionList?.Select(p => p.DeleteAuth).Distinct().ToList();
                if (selected == null) return false;
                return selected.Count == 1 ? selected.Single() : null;
            }
            set
            {
                if (value.HasValue && UserPermissionList != null)
                {
                    foreach (var p in UserPermissionList)
                        p.DeleteAuth = value.Value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool? IsUserPermissionExportAuthAllSelected
        {
            get
            {
                var selected = UserPermissionList?.Select(p => p.ExportAuth).Distinct().ToList();
                if (selected == null) return false;
                return selected.Count == 1 ? selected.Single() : null;
            }
            set
            {
                if (value.HasValue && UserPermissionList != null)
                {
                    foreach (var p in UserPermissionList)
                        p.ExportAuth = value.Value;
                    RaisePropertyChanged();
                }
            }
        }

        public bool? IsUserPermissionPrintAuthAllSelected
        {
            get
            {
                var selected = UserPermissionList?.Select(p => p.PrintAuth).Distinct().ToList();
                if (selected == null) return false;
                return selected.Count == 1 ? selected.Single() : null;
            }
            set
            {
                if (value.HasValue && UserPermissionList != null)
                {
                    foreach (var p in UserPermissionList)
                        p.PrintAuth = value.Value;
                    RaisePropertyChanged();
                }
            }
        }

        private ObservableCollection<SysFuncPermissionDetailDistinct> _sysFuncPermissionDetailDistinctList;
        /// <summary>
        /// 查詢使用者系統功能權限清單(匯總)
        /// </summary>
        public ObservableCollection<SysFuncPermissionDetailDistinct> SysFuncPermissionDetailDistinctList
        {
            get => _sysFuncPermissionDetailDistinctList;
            set => Set(ref _sysFuncPermissionDetailDistinctList, value);
        }

        private ObservableCollection<Item> _optionList;
        /// <summary>
        /// 查詢模式
        /// </summary>
        public ObservableCollection<Item> OptionList
        {
            get
            {
                if (_optionList == null)
                    _optionList = EnumData<RoleUserPermissionParamOption>.GetCollection();
                return _optionList;
            }
            set => Set(ref _optionList, value);
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

        private ObservableCollection<Func> _funcIdList;
        /// <summary>
        /// 功能代碼(除 Root & Catalog)
        /// </summary>
        public ObservableCollection<Func> FuncIdList
        {
            get
            {
                if (_funcIdList == null)
                {
                    var result = ApiUtil.HttpClientEx<ApiResult<List<Func>>>(
                        UAACRoute.Service(), UAACRoute.Func.Controller,
                        method: ApiParam.HttpVerbs.Get);
                    _funcIdList = new ObservableCollection<Func>(result.Data.Where(s =>
                    !(s.FuncType == FuncParam.FuncType.Root || s.FuncType == FuncParam.FuncType.Catalog)));
                }
                return _funcIdList;
            }
            set => Set(ref _funcIdList, value);
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
                        method: ApiParam.HttpVerbs.Get);
                    _roleIdList = new ObservableCollection<Role>(result.Data
                        .OrderByDescending(r => r.Activate).ThenBy(r => r.RoleName));
                }
                return _roleIdList;
            }
            set => Set(ref _roleIdList, value);
        }

        private ObservableCollection<Item> _filterActivateList;
        /// <summary>
        /// 啟用狀態(Filter)
        /// </summary>
        public ObservableCollection<Item> FilterActivateList
        {
            get
            {
                if (_filterActivateList == null)
                    _filterActivateList = ActivateData.GetFilterCollection();
                return _filterActivateList;
            }
            set => Set(ref _filterActivateList, value);
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

        private int userIdMaxLen = ModelUtil.GetPropertyMaxLength<UserPermission>(m => m.UserId);

        private bool _isHrInfoLoading = false;
        /// <summary>
        /// 人事資訊是否載入中
        /// </summary>
        public bool IsHrInfoLoading
        {
            get => _isHrInfoLoading;
            set => Set(ref _isHrInfoLoading, value);
        }

        private ObservableCollection<AuthSelected> _hrUserInfoList;
        /// <summary>
        /// 使用者資訊(人事系統)
        /// </summary>
        public ObservableCollection<AuthSelected> HrUserInfoList
        {
            get => _hrUserInfoList;
            set => Set(ref _hrUserInfoList, value);
        }

        private ObservableCollection<Company> _cpnyIdList;
        /// <summary>
        /// 機構別代碼
        /// </summary>
        public ObservableCollection<Company> CpnyIdList
        {
            get => _cpnyIdList;
            set => Set(ref _cpnyIdList, value);
        }

        private ObservableCollection<Position> _positionIdAllList;
        /// <summary>
        /// 職稱代碼(全部作用中)
        /// </summary>
        public ObservableCollection<Position> PositionIdAllList
        {
            get => _positionIdAllList;
            set => Set(ref _positionIdAllList, value);
        }

        private ObservableCollection<AttributeSet> _attributeIdList;
        /// <summary>
        /// 屬性進階代碼
        /// </summary>
        public ObservableCollection<AttributeSet> AttributeIdList
        {
            get => _attributeIdList;
            set => Set(ref _attributeIdList, value);
        }


        /// <summary>
        /// 初始人事資訊
        /// </summary>
        private async void InitHrInfo()
        {
            try
            {
                IsHrInfoLoading = true;

                Task<ObservableCollection<AuthSelected>> hrUserInfoTask =
                    ApiUtil.HttpClientExAsync<ObservableCollection<AuthSelected>>(
                    HrRoute.Service(), HrRoute.BasicInfo.UserInfosV2,
                    method: HttpVerbs.Get);

                Task<ObservableCollection<Company>> companyTask =
                    ApiUtil.HttpClientExAsync<ObservableCollection<Company>>(
                    HrRoute.Service(), HrRoute.Org.GetCompany,
                    method: HttpVerbs.Post);

                Task<ObservableCollection<Position>> positionTask =
                    ApiUtil.HttpClientExAsync<ObservableCollection<Position>>(
                    HrRoute.Service(), HrRoute.BasicInfo.GetPositions,
                    method: HttpVerbs.Get);

                Task<ObservableCollection<AttributeSet>> attributeSetTask =
                    ApiUtil.HttpClientExAsync<ObservableCollection<AttributeSet>>(
                    HrRoute.Service(), HrRoute.BasicInfo.GetAttributeSets,
                    method: HttpVerbs.Get);

                HrUserInfoList = await hrUserInfoTask;
                CpnyIdList = await companyTask;
                PositionIdAllList = await positionTask;
                PositionIdAllList = new ObservableCollection<Position>(PositionIdAllList.Where(p => p.Disable == "N"));
                AttributeIdList = await attributeSetTask;
                AttributeIdList = new ObservableCollection<AttributeSet>(AttributeIdList
                    .OrderBy(a => a.Attribute1).ThenBy(a => a.Attribute2).ThenBy(a => a.Attribute3));
            }
            catch (Exception) { }
            finally
            {
                IsHrInfoLoading = false;
            }
        }


        private DelegateCommand _filterOptionCommand;
        public DelegateCommand FilterOptionCommand =>
            _filterOptionCommand ?? (_filterOptionCommand = new DelegateCommand
            (OnFilterOption));
        /// <summary>
        /// 選取查詢模式
        /// </summary>
        private void OnFilterOption()
        {
            RaisePropertyChanged(nameof(IsFilterUserIdEnabled));
            RaisePropertyChanged(nameof(IsFilterRoleIdEnabled));
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

            string msg = string.Empty;
            ApiResult<RoleUserPermission> roleUserPermissionResult = await QueryRoleUserPermission();
            ApiResult<List<UserPermission>> userPermissionResult = await QueryUserPermission();
            ApiResult<List<SysFuncPermissionDetailDistinct>> sysFuncPermissionDetailDistinctResult = await QuerySysFuncPermissionDetailDistinct();
            if (!roleUserPermissionResult.Succ)
                msg += $"群組權限查詢：{roleUserPermissionResult.Msg}";
            if (userPermissionResult != null && !userPermissionResult.Succ)
            {
                if (!msg.IsNullOrWhiteSpace()) msg += Environment.NewLine;
                msg += $"使用者權限查詢：{userPermissionResult.Msg}";
            }

            if (!msg.IsNullOrWhiteSpace())
                Global.PageSnackbar.MessageEnqueue(msg);

            ProgressShow = false;
        }

        private async Task<ApiResult<RoleUserPermission>> QueryRoleUserPermission()
        {
            var result = await ApiUtil.HttpClientExAsync<ApiResult<RoleUserPermission>>(
                UAACRoute.Service(), UAACRoute.Permission.GetRoleUserPermission,
                method: ApiParam.HttpVerbs.Get,
                queryParams: FilteredItem);

            if (result.Succ)
            {
                RoleUserList = new ObservableCollection<RoleUser>(result.Data.RoleUsers);
                RolePermissionList = new ObservableCollection<RolePermission>(result.Data.RolePermissions);
            }
            else
            {
                RoleUserList?.Clear();
                RolePermissionList?.Clear();
            }

            return result;
        }

        private async Task<ApiResult<List<UserPermission>>> QueryUserPermission()
        {
            ApiResult<List<UserPermission>> result = null;

            switch (FilteredItem.Option)
            {
                case RoleUserPermissionParamOption.ByUserInfo:
                    if (FilteredItem.UserId.IsNullOrWhiteSpace())
                        result = new ApiResult<List<UserPermission>>(false, msg: MsgParam.ParamNone);
                    break;
                case RoleUserPermissionParamOption.BySysId:
                    if (FilteredItem.SysId.IsNullOrWhiteSpace())
                        result = new ApiResult<List<UserPermission>>(false, msg: MsgParam.ParamNone);
                    break;
            }

            switch (FilteredItem.Option)
            {
                case RoleUserPermissionParamOption.ByUserInfo:
                case RoleUserPermissionParamOption.BySysId:
                    if (result == null)
                    {
                        result = await ApiUtil.HttpClientExAsync<ApiResult<List<UserPermission>>>(
                        UAACRoute.Service(), UAACRoute.UserPermission.Controller,
                        method: ApiParam.HttpVerbs.Get,
                        queryParams: FilteredItem);
                    }

                    if (result.Succ)
                        UserPermissionList = new ObservableCollection<UserPermission>(result.Data);
                    else
                        UserPermissionList?.Clear();
                    break;
                default:
                    UserPermissionList?.Clear();
                    break;
            }

            return result;
        }

        private async Task<ApiResult<List<SysFuncPermissionDetailDistinct>>> QuerySysFuncPermissionDetailDistinct()
        {
            ApiResult<List<SysFuncPermissionDetailDistinct>> result = null;
            SysFuncPermissionDetailDistinctList?.Clear();

            switch (FilteredItem.Option)
            {
                case RoleUserPermissionParamOption.ByUserInfo:
                    if (!FilteredItem.UserId.IsNullOrWhiteSpace())
                    {
                        result = await ApiUtil.HttpClientExAsync<ApiResult<List<SysFuncPermissionDetailDistinct>>>(
                        UAACRoute.Service(), UAACRoute.Permission.GetSysFuncPermissionDetailDistinct,
                        method: ApiParam.HttpVerbs.Get,
                        queryParams: new { userId = FilteredItem.UserId, sysId = FilteredItem.SysId });

                        if (result.Succ)
                            SysFuncPermissionDetailDistinctList = new ObservableCollection<SysFuncPermissionDetailDistinct>(result.Data);
                    }
                    break;
            }

            return result;
        }


        private DelegateCommand<FrameworkElement> _roleUserInsertCommand;
        public DelegateCommand<FrameworkElement> RoleUserInsertCommand =>
            _roleUserInsertCommand ?? (_roleUserInsertCommand = new DelegateCommand<FrameworkElement>
            (OnRoleUserInsert, (param) => (bool)Permission.AddAuth && !IsHrInfoLoading));
        /// <summary>
        /// 角色群組使用者：新增
        /// </summary>
        private void OnRoleUserInsert(FrameworkElement dialogContent)
        {
            var vm = new RoleUserEditViewModel();
            vm.EditMode = EditParam.EditMode.INSERT;
            vm.HrUserInfoList = HrUserInfoList;
            vm.CpnyIdList = CpnyIdList;
            vm.PositionIdAllList = PositionIdAllList;
            vm.AttributeIdList = AttributeIdList;
            vm.CloseDialog = CloseRoleUserInsertDialog;
            dialogContent.DataContext = vm;
            DialogContent = dialogContent;
            IsOpenDialog = true;
        }

        private DelegateCommand<FrameworkElement> _roleUserEditCommand;
        public DelegateCommand<FrameworkElement> RoleUserEditCommand =>
            _roleUserEditCommand ?? (_roleUserEditCommand = new DelegateCommand<FrameworkElement>
            (OnRoleUserEdit, (param) => (bool)Permission.ModifyAuth && !IsHrInfoLoading));
        /// <summary>
        /// 角色群組使用者：編輯
        /// </summary>
        private void OnRoleUserEdit(FrameworkElement dialogContent)
        {
            var vm = new RoleUserEditViewModel();
            vm.EditMode = EditParam.EditMode.UPDATE;
            vm.SetEditedItem(new RoleUser { RoleUserId = RoleUserSelectedItem?.RoleUserId });
            vm.HrUserInfoList = HrUserInfoList;
            vm.CpnyIdList = CpnyIdList;
            vm.PositionIdAllList = PositionIdAllList;
            vm.AttributeIdList = AttributeIdList;
            vm.CloseDialog = CloseRoleUserEditDialog;
            dialogContent.DataContext = vm;
            DialogContent = dialogContent;
            IsOpenDialog = true;
        }

        private void CloseRoleUserInsertDialog(bool close, bool? result)
        {
            if (close) IsOpenDialog = false;
        }

        private async void CloseRoleUserEditDialog(bool close, bool? result)
        {
            if (close) IsOpenDialog = false;
            if (result.HasValue && result.Value)
            {
                ProgressShow = true;
                await QueryRoleUserPermission();
                ProgressShow = false;
            }
        }


        private void RolePermissionAllSelectedPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(RolePermission.Selected))
                (sender as RolePermission).Selected = true; // 修改過即為可批次儲存

            if (e.PropertyName == nameof(RolePermission.QueryAuth))
                RaisePropertyChanged(nameof(IsRolePermissionQueryAuthAllSelected));
            else if (e.PropertyName == nameof(RolePermission.AddAuth))
                RaisePropertyChanged(nameof(IsRolePermissionAddAuthAllSelected));
            else if (e.PropertyName == nameof(RolePermission.ModifyAuth))
                RaisePropertyChanged(nameof(IsRolePermissionModifyAuthAllSelected));
            else if (e.PropertyName == nameof(RolePermission.DeleteAuth))
                RaisePropertyChanged(nameof(IsRolePermissionDeleteAuthAllSelected));
            else if (e.PropertyName == nameof(RolePermission.ExportAuth))
                RaisePropertyChanged(nameof(IsRolePermissionExportAuthAllSelected));
            else if (e.PropertyName == nameof(RolePermission.PrintAuth))
                RaisePropertyChanged(nameof(IsRolePermissionPrintAuthAllSelected));
        }

        private void RolePermissionValidationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            (sender as RolePermission).PropertyChanged -= RolePermissionValidationPropertyChanged;
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
            (sender as RolePermission).PropertyChanged += RolePermissionValidationPropertyChanged;
        }

        private DelegateCommand<FrameworkElement> _rolePermissionInsertCommand;
        public DelegateCommand<FrameworkElement> RolePermissionInsertCommand =>
            _rolePermissionInsertCommand ?? (_rolePermissionInsertCommand = new DelegateCommand<FrameworkElement>
            (OnRolePermissionInsert, (param) => (bool)Permission.AddAuth));
        /// <summary>
        /// 角色群組權限：新增
        /// </summary>
        private void OnRolePermissionInsert(FrameworkElement dialogContent)
        {
            var vm = new RolePermissionEditViewModel();
            vm.EditMode = EditParam.EditMode.INSERT;
            vm.CloseDialog = CloseRolePermissionInsertDialog;
            dialogContent.DataContext = vm;
            DialogContent = dialogContent;
            IsOpenDialog = true;
        }

        private void CloseRolePermissionInsertDialog(bool close, bool? result)
        {
            if (close) IsOpenDialog = false;
        }

        private DelegateCommand _rolePermissionBatchPatchCommand;
        public DelegateCommand RolePermissionBatchPatchCommand =>
            _rolePermissionBatchPatchCommand ?? (_rolePermissionBatchPatchCommand = new DelegateCommand
            (OnRolePermissionBatchPatch, RolePermissionBatchPatchCanExecute));
        /// <summary>
        /// 角色群組權限：批次儲存
        /// </summary>
        private async void OnRolePermissionBatchPatch()
        {
            ProgressShow = true;

            var batchPatchList = RolePermissionList.Where(rp => rp.Selected).ToList();

            foreach (var rp in batchPatchList)
                rp.MUserId = LoginViewModel.LoginUser.EmpId;

            var result = await ApiUtil.HttpClientExAsync<ApiResult<List<RolePermission>>>(
                UAACRoute.Service(), UAACRoute.RolePermission.BatchPatchAuth,
                method: HttpVerbs.Patch,
                body: batchPatchList);

            Global.PageSnackbar.MessageEnqueue(result.Msg);

            if (result.Succ)
                await QueryRoleUserPermission();

            ProgressShow = false;
        }

        private bool RolePermissionBatchPatchCanExecute()
        {
            bool canExecute = (bool)Permission.ModifyAuth && !ProgressShow;
            if (canExecute)
            {
                bool selected = RolePermissionList?.Any(rp => rp.Selected) ?? false;
                canExecute = canExecute && selected;
                if (canExecute)
                {
                    bool hasNoAuthChecked = RolePermissionList?
                        .Any(rp => rp.Selected &&
                        !(bool)rp.QueryAuth && !(bool)rp.AddAuth && !(bool)rp.ModifyAuth &&
                        !(bool)rp.DeleteAuth && !(bool)rp.ExportAuth && !(bool)rp.PrintAuth) ?? true;
                    canExecute = canExecute && !hasNoAuthChecked;
                }
            }
            return canExecute;
        }


        private void UserPermissionAllSelectedPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName != nameof(UserPermission.Selected))
                (sender as UserPermission).Selected = true; // 修改過即為可批次儲存

            if (e.PropertyName == nameof(UserPermission.QueryAuth))
                RaisePropertyChanged(nameof(IsUserPermissionQueryAuthAllSelected));
            else if (e.PropertyName == nameof(UserPermission.AddAuth))
                RaisePropertyChanged(nameof(IsUserPermissionAddAuthAllSelected));
            else if (e.PropertyName == nameof(UserPermission.ModifyAuth))
                RaisePropertyChanged(nameof(IsUserPermissionModifyAuthAllSelected));
            else if (e.PropertyName == nameof(UserPermission.DeleteAuth))
                RaisePropertyChanged(nameof(IsUserPermissionDeleteAuthAllSelected));
            else if (e.PropertyName == nameof(UserPermission.ExportAuth))
                RaisePropertyChanged(nameof(IsUserPermissionExportAuthAllSelected));
            else if (e.PropertyName == nameof(UserPermission.PrintAuth))
                RaisePropertyChanged(nameof(IsUserPermissionPrintAuthAllSelected));
        }

        private void UserPermissionValidationPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            (sender as UserPermission).PropertyChanged -= UserPermissionValidationPropertyChanged;
            if (e.PropertyName == nameof(UserPermission.QueryAuth) ||
                e.PropertyName == nameof(UserPermission.AddAuth) ||
                e.PropertyName == nameof(UserPermission.ModifyAuth) ||
                e.PropertyName == nameof(UserPermission.DeleteAuth) ||
                e.PropertyName == nameof(UserPermission.ExportAuth) ||
                e.PropertyName == nameof(UserPermission.PrintAuth))
            {
                if (e.PropertyName != nameof(UserPermission.QueryAuth))
                    (sender as UserPermission).RaisePropertyChanged(nameof(UserPermission.QueryAuth));
                if (e.PropertyName != nameof(UserPermission.AddAuth))
                    (sender as UserPermission).RaisePropertyChanged(nameof(UserPermission.AddAuth));
                if (e.PropertyName != nameof(UserPermission.ModifyAuth))
                    (sender as UserPermission).RaisePropertyChanged(nameof(UserPermission.ModifyAuth));
                if (e.PropertyName != nameof(UserPermission.DeleteAuth))
                    (sender as UserPermission).RaisePropertyChanged(nameof(UserPermission.DeleteAuth));
                if (e.PropertyName != nameof(UserPermission.ExportAuth))
                    (sender as UserPermission).RaisePropertyChanged(nameof(UserPermission.ExportAuth));
                if (e.PropertyName != nameof(UserPermission.PrintAuth))
                    (sender as UserPermission).RaisePropertyChanged(nameof(UserPermission.PrintAuth));
            }
            (sender as UserPermission).PropertyChanged += UserPermissionValidationPropertyChanged;
        }

        private DelegateCommand<FrameworkElement> _userPermissionInsertCommand;
        public DelegateCommand<FrameworkElement> UserPermissionInsertCommand =>
            _userPermissionInsertCommand ?? (_userPermissionInsertCommand = new DelegateCommand<FrameworkElement>
            (OnUserPermissionInsert, (param) => (bool)Permission.AddAuth));
        /// <summary>
        /// 使用者權限：新增
        /// </summary>
        private void OnUserPermissionInsert(FrameworkElement dialogContent)
        {
            var vm = new UserPermissionEditViewModel();
            vm.EditMode = EditParam.EditMode.INSERT;
            vm.CloseDialog = CloseUserPermissionInsertDialog;
            dialogContent.DataContext = vm;
            DialogContent = dialogContent;
            IsOpenDialog = true;
        }

        private void CloseUserPermissionInsertDialog(bool close, bool? result)
        {
            if (close) IsOpenDialog = false;
        }

        private DelegateCommand _userPermissionBatchPatchCommand;
        public DelegateCommand UserPermissionBatchPatchCommand =>
            _userPermissionBatchPatchCommand ?? (_userPermissionBatchPatchCommand = new DelegateCommand
            (OnUserPermissionBatchPatch, UserPermissionBatchPatchCanExecute));
        /// <summary>
        /// 使用者權限：批次儲存
        /// </summary>
        private async void OnUserPermissionBatchPatch()
        {
            ProgressShow = true;

            var batchPatchList = UserPermissionList.Where(rp => rp.Selected).ToList();

            foreach (var rp in batchPatchList)
                rp.MUserId = LoginViewModel.LoginUser.EmpId;

            var result = await ApiUtil.HttpClientExAsync<ApiResult<List<UserPermission>>>(
                UAACRoute.Service(), UAACRoute.UserPermission.BatchPatchAuth,
                method: HttpVerbs.Patch,
                body: batchPatchList);

            Global.PageSnackbar.MessageEnqueue(result.Msg);

            if (result.Succ)
                await QueryUserPermission();

            ProgressShow = false;
        }

        private bool UserPermissionBatchPatchCanExecute()
        {
            bool canExecute = (bool)Permission.ModifyAuth && !ProgressShow;
            if (canExecute)
            {
                bool selected = UserPermissionList?.Any(rp => rp.Selected) ?? false;
                canExecute = canExecute && selected;
                if (canExecute)
                {
                    bool hasNoAuthChecked = UserPermissionList?
                         .Any(rp => rp.Selected &&
                         (rp.UserId.StrLen() > userIdMaxLen ||
                         (!(bool)rp.QueryAuth && !(bool)rp.AddAuth && !(bool)rp.ModifyAuth &&
                         !(bool)rp.DeleteAuth && !(bool)rp.ExportAuth && !(bool)rp.PrintAuth)
                         )) ?? true;
                    canExecute = canExecute && !hasNoAuthChecked;
                }
            }
            return canExecute;
        }

    }
}
