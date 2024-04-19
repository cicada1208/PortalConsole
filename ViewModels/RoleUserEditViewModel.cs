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
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Data;
using static MoreLinq.Extensions.DistinctByExtension;
using static Params.ApiParam;
using static Params.EditParam;

namespace ViewModels
{
    public class RoleUserEditViewModel : BaseViewModel<RoleUserEditViewModel>
    {
        public RoleUserEditViewModel()
        {
            QueryDeptAndPositionListDebounce();
        }

        private RoleUser _editedItem;
        /// <summary>
        /// 編輯
        /// </summary>
        public RoleUser EditedItem
        {
            get
            {
                if (_editedItem == null)
                {
                    _editedItem = new RoleUser
                    {
                        Activate = true
                    };
                }
                return _editedItem;
            }
            set => Set(ref _editedItem, value);
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

        private ObservableCollection<Company> _cpnyIdList;
        /// <summary>
        /// 機構別代碼
        /// </summary>
        public ObservableCollection<Company> CpnyIdList
        {
            get
            {
                if (_cpnyIdList == null)
                {
                    var result = ApiUtil.HttpClientEx<List<Company>>(
                        HrRoute.Service(), HrRoute.Org.GetCompany,
                        method: ApiParam.HttpVerbs.Post);
                    _cpnyIdList = new ObservableCollection<Company>(result);
                }
                return _cpnyIdList;
            }
            set => Set(ref _cpnyIdList, value);
        }

        private ObservableCollection<DeptView> _deptIdList;
        /// <summary>
        /// 科室代碼
        /// </summary>
        public ObservableCollection<DeptView> DeptIdList
        {
            get => _deptIdList;
            set => Set(ref _deptIdList, value);
        }

        private ObservableCollection<Position> _positionIdAllList;
        /// <summary>
        /// 職稱代碼(全部作用中)
        /// </summary>
        public ObservableCollection<Position> PositionIdAllList
        {
            get
            {
                if (_positionIdAllList == null)
                {
                    var result = ApiUtil.HttpClientEx<List<Position>>(
                        HrRoute.Service(), HrRoute.BasicInfo.GetPositions,
                        method: ApiParam.HttpVerbs.Get);
                    _positionIdAllList = new ObservableCollection<Position>(result.Where(p => p.Disable == "N"));
                }
                return _positionIdAllList;
            }
            set => Set(ref _positionIdAllList, value);
        }

        private ObservableCollection<Position> _positionIdList;
        /// <summary>
        /// 職稱代碼
        /// </summary>
        public ObservableCollection<Position> PositionIdList
        {
            get => _positionIdList;
            set => Set(ref _positionIdList, value);
        }

        private ObservableCollection<AttributeSet> _attributeIdList;
        /// <summary>
        /// 屬性進階代碼
        /// </summary>
        public ObservableCollection<AttributeSet> AttributeIdList
        {
            get
            {
                if (_attributeIdList == null)
                {
                    var result = ApiUtil.HttpClientEx<List<AttributeSet>>(
                        HrRoute.Service(), HrRoute.BasicInfo.GetAttributeSets,
                        method: ApiParam.HttpVerbs.Get);
                    _attributeIdList = new ObservableCollection<AttributeSet>(result
                        .OrderBy(a => a.Attribute1).ThenBy(a => a.Attribute2).ThenBy(a => a.Attribute3));
                }
                return _attributeIdList;
            }
            set => Set(ref _attributeIdList, value);
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

        private Func<Task> _queryDeptAndPositionListDebounce;
        private Func<Task> QueryDeptAndPositionListDebounce => _queryDeptAndPositionListDebounce ??
            (_queryDeptAndPositionListDebounce = ((Func<CancellationToken, Task>)QueryDeptAndPositionList).Debounce());


        public void SetEditedItem(RoleUser roleUser)
        {
            var result = ApiUtil.HttpClientEx<ApiResult<List<RoleUser>>>(
            UAACRoute.Service(), UAACRoute.RoleUser.Controller,
            method: HttpVerbs.Get,
            queryParams: roleUser
            ).Data.FirstOrDefault();
            EditedItem = result;
        }

        private DelegateCommand _selectCpnyIdCommand;
        public DelegateCommand SelectCpnyIdCommand =>
            _selectCpnyIdCommand ?? (_selectCpnyIdCommand = new DelegateCommand
            (OnSelectCpnyId));
        /// <summary>
        /// 選取機構代碼，查詢科室代碼、職稱代碼
        /// </summary>
        private void OnSelectCpnyId()
        {
            DeptIdList?.Clear();
            PositionIdList?.Clear();
            QueryDeptAndPositionListDebounce();
        }

        private async Task QueryDeptAndPositionList(CancellationToken cancellationToken = default)
        {
            if (!EditedItem.CpnyId.IsNullOrWhiteSpace())
            {
                var deptResult = await ApiUtil.HttpClientExAsync<ObservableCollection<DeptView>>(
                 HrRoute.Service(), HrRoute.Org.GetDept + EditedItem.CpnyId,
                 method: ApiParam.HttpVerbs.Get,
                 cancellationToken: cancellationToken);

                if (deptResult != null)
                    DeptIdList = new ObservableCollection<DeptView>(deptResult.OrderBy(d => d.No));

                PositionIdList = new ObservableCollection<Position>(
                    PositionIdAllList.Where(p => p.CpnyId == EditedItem.CpnyId));
            }
        }

        private DelegateCommand _okCommand;
        public DelegateCommand OKCommand =>
            _okCommand ?? (_okCommand = new DelegateCommand
            (OnOK, () => Validate().IsValid));
        private void OnOK()
        {
            EditedItem.MUserId = LoginViewModel.LoginUser.EmpId;

            var result = ApiUtil.HttpClientEx<ApiResult<RoleUser>>(
                UAACRoute.Service(), UAACRoute.RoleUser.Controller,
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


        #region 使用者資訊

        private Auth _searchedUserInfo;
        /// <summary>
        /// 使用者資訊(含人事系統、iEMR、嘉基資訊系統)
        /// </summary>
        public Auth SearchedUserInfo
        {
            get
            {
                if (_searchedUserInfo == null)
                    _searchedUserInfo = new Auth();
                return _searchedUserInfo;
            }
            set => Set(ref _searchedUserInfo, value);
        }

        private AuthSelected _filteredHrUserInfo;
        /// <summary>
        /// 篩選使用者資訊(人事系統)
        /// </summary>
        public AuthSelected FilteredHrUserInfo
        {
            get
            {
                if (_filteredHrUserInfo == null)
                {
                    _filteredHrUserInfo = new AuthSelected();
                    _filteredHrUserInfo.Selected = null;
                    _filteredHrUserInfo.PropertyChanged += FilteredHrUserInfoPropertyChanged;
                }
                return _filteredHrUserInfo;
            }
            set
            {
                Set(ref _filteredHrUserInfo, value);
                if (_filteredHrUserInfo != null)
                {
                    _filteredHrUserInfo.PropertyChanged -= FilteredHrUserInfoPropertyChanged;
                    _filteredHrUserInfo.PropertyChanged += FilteredHrUserInfoPropertyChanged;
                }
            }
        }

        private ObservableCollection<AuthSelected> _hrUserInfoList;
        /// <summary>
        /// 使用者資訊(人事系統)
        /// </summary>
        public ObservableCollection<AuthSelected> HrUserInfoList
        {
            get
            {
                if (_hrUserInfoList == null)
                {
                    _hrUserInfoList = ApiUtil.HttpClientEx<ObservableCollection<AuthSelected>>(
                        HrRoute.Service(), HrRoute.BasicInfo.UserInfosV2,
                        method: HttpVerbs.Get);

                    CollectionView view = CollectionViewSource.GetDefaultView(_hrUserInfoList) as CollectionView;
                    view.Filter = HrUserInfoFilter;
                }
                return _hrUserInfoList;
            }
            set
            {
                Set(ref _hrUserInfoList, value);
                if (_hrUserInfoList != null)
                {
                    CollectionView view = CollectionViewSource.GetDefaultView(_hrUserInfoList) as CollectionView;
                    view.Filter = HrUserInfoFilter;
                }
            }
        }

        private ObservableCollection<Item> _hrUserInfoCpnyIdList;
        /// <summary>
        /// 機構別代碼
        /// </summary>
        public ObservableCollection<Item> HrUserInfoCpnyIdList
        {
            get
            {
                if (_hrUserInfoCpnyIdList == null)
                {
                    var items = HrUserInfoList.DistinctBy(u => u.CpnyID)
                        .Where(u => !u.CpnyID.IsNullOrWhiteSpace())
                        .Select(u => new Item { Value = u.CpnyID, Text = u.CoscName })
                        .OrderBy(u => u.Value);
                    _hrUserInfoCpnyIdList = new ObservableCollection<Item>(items);
                }
                return _hrUserInfoCpnyIdList;
            }
        }

        private ObservableCollection<Item> _hrUserInfoDeptIdList;
        /// <summary>
        /// 科室代碼
        /// </summary>
        public ObservableCollection<Item> HrUserInfoDeptIdList
        {
            get
            {
                if (_hrUserInfoDeptIdList == null)
                {
                    var items = HrUserInfoList.DistinctBy(u => u.DeptNo)
                        .Where(u => !u.DeptNo.IsNullOrWhiteSpace())
                        .Select(u => new Item { Value = u.DeptNo, Text = u.DeptName })
                        .OrderBy(u => u.Value);
                    _hrUserInfoDeptIdList = new ObservableCollection<Item>(items);
                }
                return _hrUserInfoDeptIdList;
            }
        }

        private ObservableCollection<Item> _hrUserInfoPositionIdList;
        /// <summary>
        /// 職稱代碼
        /// </summary>
        public ObservableCollection<Item> HrUserInfoPositionIdList
        {
            get
            {
                if (_hrUserInfoPositionIdList == null)
                {
                    var items = HrUserInfoList.DistinctBy(u => u.Possie)
                        .Where(u => !u.Possie.IsNullOrWhiteSpace())
                        .Select(u => new Item { Value = u.Possie, Text = u.PosName })
                        .OrderBy(u => u.Value);
                    _hrUserInfoPositionIdList = new ObservableCollection<Item>(items);
                }
                return _hrUserInfoPositionIdList;
            }
        }

        private ObservableCollection<Item> _hrUserInfoAttributeIdList;
        /// <summary>
        /// 屬性進階代碼
        /// </summary>
        public ObservableCollection<Item> HrUserInfoAttributeIdList
        {
            get
            {
                if (_hrUserInfoAttributeIdList == null)
                {
                    var items = HrUserInfoList.DistinctBy(u => u.AttributeID)
                        .Where(u => !u.AttributeID.IsNullOrWhiteSpace())
                        .Select(u => new Item { Value = u.AttributeID, Text = u.Attribute })
                        .OrderBy(u => u.Text).ThenBy(u => u.Value);
                    _hrUserInfoAttributeIdList = new ObservableCollection<Item>(items);
                }
                return _hrUserInfoAttributeIdList;
            }
        }

        private ObservableCollection<Item> _hrUserInfoEmpIdList;
        /// <summary>
        /// 使用者代碼
        /// </summary>
        public ObservableCollection<Item> HrUserInfoEmpIdList
        {
            get
            {
                if (_hrUserInfoEmpIdList == null)
                {
                    var items = HrUserInfoList.DistinctBy(u => u.EmpId)
                        .Select(u => new Item { Value = u.EmpId, Text = u.Name })
                        .OrderBy(u => u.Value);
                    _hrUserInfoEmpIdList = new ObservableCollection<Item>(items);
                }
                return _hrUserInfoEmpIdList;
            }
        }

        private ObservableCollection<Item> _hrUserInfoStateList;
        /// <summary>
        /// 在職狀態
        /// </summary>
        public ObservableCollection<Item> HrUserInfoStateList
        {
            get
            {
                if (_hrUserInfoStateList == null)
                {
                    _hrUserInfoStateList = new ObservableCollection<Item>
                    {
                        new Item { Value="A", Text="在職" },
                        new Item { Value="B", Text="留停" },
                        new Item { Value="C", Text="離職" },
                        new Item { Value="D", Text="資遣" },
                        new Item { Value="E", Text="退休" },
                    };
                }
                return _hrUserInfoStateList;
            }
        }


        private DelegateCommand _searchUserInfoCommand;
        public DelegateCommand SearchUserInfoCommand =>
            _searchUserInfoCommand ?? (_searchUserInfoCommand = new DelegateCommand
            (OnSearchUserInfo, () => !ProgressShow));
        /// <summary>
        /// 查詢使用者資訊
        /// </summary>
        private void OnSearchUserInfo()
        {
            ProgressShow = true;

            if (!SearchedUserInfo.EmpId.IsNullOrWhiteSpace())
            {
                var userInfoResult = ApiUtil.HttpClientEx<ApiResult<Auth>>(
                   UAACRoute.Service(), UAACRoute.Auth.GetUserInfo + SearchedUserInfo.EmpId,
                   method: ApiParam.HttpVerbs.Get);

                if (userInfoResult.Succ)
                    SearchedUserInfo = userInfoResult.Data;
                else
                    Global.PageSnackbar.MessageEnqueue(userInfoResult.Msg);
            }

            ProgressShow = false;
        }

        private DelegateCommand _importUserInfoCommand;
        public DelegateCommand ImportUserInfoCommand =>
            _importUserInfoCommand ?? (_importUserInfoCommand = new DelegateCommand
            (OnImportUserInfo));
        /// <summary>
        /// 匯入使用者資訊
        /// </summary>
        private void OnImportUserInfo()
        {
            EditedItem.CpnyId = SearchedUserInfo.CpnyID;
            EditedItem.DeptNo = SearchedUserInfo.DeptNo;
            EditedItem.Possie = SearchedUserInfo.Possie;
            EditedItem.Attribute = SearchedUserInfo.AttributeID;
            EditedItem.UserId = SearchedUserInfo.EmpId;
        }

        private void FilteredHrUserInfoPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName == nameof(AuthSelected.CpnyID) ||
                e.PropertyName == nameof(AuthSelected.DeptNo) ||
                e.PropertyName == nameof(AuthSelected.Possie) ||
                e.PropertyName == nameof(AuthSelected.AttributeID) ||
                e.PropertyName == nameof(AuthSelected.EmpId) ||
                e.PropertyName == nameof(AuthSelected.Selected))
                CollectionViewSource.GetDefaultView(HrUserInfoList)?.Refresh();
        }

        private bool HrUserInfoFilter(object item)
        {
            bool result = true;
            AuthSelected auth = item as AuthSelected;
            if (!FilteredHrUserInfo.CpnyID.IsNullOrWhiteSpace())
                result = result && auth.CpnyID == FilteredHrUserInfo.CpnyID;
            if (!FilteredHrUserInfo.DeptNo.IsNullOrWhiteSpace())
                result = result && auth.DeptNo == FilteredHrUserInfo.DeptNo;
            if (!FilteredHrUserInfo.Possie.IsNullOrWhiteSpace())
                result = result && auth.Possie == FilteredHrUserInfo.Possie;
            if (!FilteredHrUserInfo.AttributeID.IsNullOrWhiteSpace())
                result = result && auth.AttributeID == FilteredHrUserInfo.AttributeID;
            if (!FilteredHrUserInfo.EmpId.IsNullOrWhiteSpace())
                result = result && auth.EmpId == FilteredHrUserInfo.EmpId;
            if (FilteredHrUserInfo.Selected != null)
                result = result && auth.Selected.Equals(FilteredHrUserInfo.Selected);
            return result;
        }

        #endregion

    }
}
