using Lib;
using Lib.Wpf;
using Lib.Wpf.Routes;
using Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static Params.ApiParam;
using static Params.EditParam;

namespace ViewModels
{
    public class RoleEditViewModel : BaseViewModel<RoleEditViewModel>
    {
        private Role _editedItem;
        /// <summary>
        /// 編輯
        /// </summary>
        public Role EditedItem
        {
            get
            {
                if (_editedItem == null)
                {
                    _editedItem = new Role
                    {
                        Activate = true
                    };
                }
                return _editedItem;
            }
            set => Set(ref _editedItem, value);
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


        public void SetEditedItem(Role role)
        {
            var result = ApiUtil.HttpClientEx<ApiResult<List<Role>>>(
            UAACRoute.Service(), UAACRoute.Role.Controller,
            method: HttpVerbs.Get,
            queryParams: role
            ).Data.FirstOrDefault();
            EditedItem = result;
        }

        private DelegateCommand _okCommand;
        public DelegateCommand OKCommand =>
            _okCommand ?? (_okCommand = new DelegateCommand
            (OnOK, () => Validate().IsValid));
        private void OnOK()
        {
            EditedItem.MUserId = LoginViewModel.LoginUser.EmpId;

            var result = ApiUtil.HttpClientEx<ApiResult<Role>>(
                UAACRoute.Service(), UAACRoute.Role.Controller,
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
