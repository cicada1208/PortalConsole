using Lib;
using Lib.Wpf;
using Lib.Wpf.Routes;
using Models;
using Params;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using static Params.ApiParam;
using static Params.EditParam;
using static Params.FuncParam;

namespace ViewModels
{
    public class FuncEditViewModel : BaseViewModel<FuncEditViewModel>
    {
        private Func _editedItem;
        /// <summary>
        /// 編輯
        /// </summary>
        public Func EditedItem
        {
            get
            {
                if (_editedItem == null)
                {
                    _editedItem = new Func
                    {
                        Activate = true
                    };
                }
                return _editedItem;
            }
            set => Set(ref _editedItem, value);
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

        private ObservableCollection<Item> _funcTypeList;
        /// <summary>
        /// 功能類別
        /// </summary>
        public ObservableCollection<Item> FuncTypeList
        {
            get
            {
                if (_funcTypeList == null)
                {
                    _funcTypeList = EnumData<FuncType>.GetCollection();
                    if (EditMode == EditMode.UPDATE &&
                        !(EditedItem.FuncType == FuncType.Root ||
                        EditedItem.FuncType == FuncType.Catalog))
                    {
                        _funcTypeList = new ObservableCollection<Item>(_funcTypeList.Where(t =>
                        !(t.Value.Equals(FuncType.Root) ||
                        t.Value.Equals(FuncType.Catalog))));
                    }
                }
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
                    _activateList = ActivateData.GetCollection();
                return _activateList;
            }
            set => Set(ref _activateList, value);
        }

        /// <summary>
        /// 是否編輯功能類別
        /// </summary>
        public bool IsFuncTypeEnabled
        {
            get
            {
                switch (EditMode)
                {
                    case EditMode.UPDATE:
                        return (EditedItem.FuncType == FuncType.Root ||
                            EditedItem.FuncType == FuncType.Catalog) ? false : true;
                    default:
                        return true;
                }
            }
        }


        public void SetEditedItem(Func func)
        {
            var result = ApiUtil.HttpClientEx<ApiResult<List<Func>>>(
            UAACRoute.Service(), UAACRoute.Func.Controller,
            method: HttpVerbs.Get,
            queryParams: func
            ).Data.FirstOrDefault();
            EditedItem = result;
        }

        private DelegateCommand _selectFuncTypeCommand;
        public DelegateCommand SelectFuncTypeCommand =>
            _selectFuncTypeCommand ?? (_selectFuncTypeCommand = new DelegateCommand
            (OnSelectFuncType));
        /// <summary>
        /// 選取功能類別
        /// </summary>
        private void OnSelectFuncType()
        {
            switch (EditedItem.FuncType)
            {
                case FuncType.Root:
                    break;
                default:
                    if (EditMode == EditMode.INSERT && EditedItem.FuncId.IsNullOrWhiteSpace())
                        EditedItem.FuncId = Guid.NewGuid().ToString();
                    break;
            }

            switch (EditedItem.FuncType)
            {
                case FuncType.Root:
                case FuncType.Catalog:
                    EditedItem.BasePath = string.Empty;
                    EditedItem.SubPath = string.Empty;
                    EditedItem.Assembly = string.Empty;
                    EditedItem.ViewName = string.Empty;
                    EditedItem.ViewComponent = string.Empty;
                    EditedItem.Limit = null;
                    break;
            }

            EditedItem.RaisePropertyChanged(nameof(EditedItem.BasePath));
            EditedItem.RaisePropertyChanged(nameof(EditedItem.SubPath));
            EditedItem.RaisePropertyChanged(nameof(EditedItem.Assembly));
            EditedItem.RaisePropertyChanged(nameof(EditedItem.ViewName));
            EditedItem.RaisePropertyChanged(nameof(EditedItem.ViewComponent));
        }

        private DelegateCommand _okCommand;
        public DelegateCommand OKCommand =>
            _okCommand ?? (_okCommand = new DelegateCommand
            (OnOK, () => Validate().IsValid));
        private void OnOK()
        {
            EditedItem.MUserId = LoginViewModel.LoginUser.EmpId;

            var result = ApiUtil.HttpClientEx<ApiResult<Func>>(
                UAACRoute.Service(), UAACRoute.Func.Controller,
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
