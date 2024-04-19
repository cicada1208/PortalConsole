using Lib;
using Lib.Wpf;
using Lib.Wpf.Routes;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static Params.ApiParam;
using static Params.EditParam;
using static Params.SysAppParam;

namespace ViewModels
{
    public class SysAppEditViewModel : BaseViewModel<SysAppEditViewModel>
    {
        private SysApp _editedItem;
        /// <summary>
        /// 編輯
        /// </summary>
        public SysApp EditedItem
        {
            get
            {
                if (_editedItem == null)
                {
                    _editedItem = new SysApp
                    {
                        Activate = true
                    };
                }
                return _editedItem;
            }
            set => Set(ref _editedItem, value);
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
                {
                    _sysTypeList = EnumData<SysType>.GetCollection();
                    if (EditMode == EditMode.UPDATE &&
                        !(EditedItem.SysType == SysType.Root ||
                        EditedItem.SysType == SysType.Catalog))
                    {
                        _sysTypeList = new ObservableCollection<Item>(_sysTypeList.Where(t =>
                        !(t.Value.Equals(SysType.Root) ||
                        t.Value.Equals(SysType.Catalog))));
                    }
                }
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
                    _activateList = ActivateData.GetCollection();
                return _activateList;
            }
            set => Set(ref _activateList, value);
        }

        /// <summary>
        /// 是否編輯系統類別
        /// </summary>
        public bool IsSysTypeEnabled
        {
            get
            {
                switch (EditMode)
                {
                    case EditMode.UPDATE:
                        return (EditedItem.SysType == SysType.Root ||
                            EditedItem.SysType == SysType.Catalog) ? false : true;
                    default:
                        return true;
                }
            }
        }


        public void SetEditedItem(SysApp sysApp)
        {
            var result = ApiUtil.HttpClientEx<ApiResult<List<SysApp>>>(
            UAACRoute.Service(), UAACRoute.SysApp.Controller,
            method: HttpVerbs.Get,
            queryParams: sysApp
            ).Data.FirstOrDefault();
            EditedItem = result;
        }

        private DelegateCommand _selectSysTypeCommand;
        public DelegateCommand SelectSysTypeCommand =>
            _selectSysTypeCommand ?? (_selectSysTypeCommand = new DelegateCommand
            (OnSelectSysType));
        /// <summary>
        /// 選取系統類別
        /// </summary>
        private void OnSelectSysType()
        {
            switch (EditedItem.SysType)
            {
                case SysType.Root:
                    EditedItem.BasePath = string.Empty;
                    EditedItem.SubPath = string.Empty;
                    EditedItem.Assembly = string.Empty;
                    EditedItem.Limit = null;
                    break;
                case SysType.Catalog:
                    if (EditMode == EditMode.INSERT && EditedItem.SysId.IsNullOrWhiteSpace())
                        EditedItem.SysId = Guid.NewGuid().ToString();
                    EditedItem.BasePath = string.Empty;
                    EditedItem.SubPath = string.Empty;
                    EditedItem.Assembly = string.Empty;
                    EditedItem.Limit = null;
                    break;
            }

            EditedItem.RaisePropertyChanged(nameof(EditedItem.BasePath));
            EditedItem.RaisePropertyChanged(nameof(EditedItem.SubPath));
            EditedItem.RaisePropertyChanged(nameof(EditedItem.Assembly));
        }

        private DelegateCommand _okCommand;
        public DelegateCommand OKCommand =>
            _okCommand ?? (_okCommand = new DelegateCommand
            (OnOK, () => Validate().IsValid));
        private void OnOK()
        {
            EditedItem.MUserId = LoginViewModel.LoginUser.EmpId;

            var result = ApiUtil.HttpClientEx<ApiResult<SysApp>>(
                UAACRoute.Service(), UAACRoute.SysApp.Controller,
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
