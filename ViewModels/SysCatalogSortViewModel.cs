using Lib;
using Lib.Wpf;
using Lib.Wpf.Routes;
using Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using static Params.ApiParam;

namespace ViewModels
{
    public class SysCatalogSortViewModel : BaseViewModel<SysCatalogSortViewModel>
    {
        private SysCatalogQueryParam _queryParam;
        /// <summary>
        /// 查詢參數
        /// </summary>
        public SysCatalogQueryParam QueryParam
        {
            get => _queryParam ?? (_queryParam = new SysCatalogQueryParam());
            set => Set(ref _queryParam, value);
        }

        private ObservableCollection<SysGroup> _sysGroupList;
        /// <summary>
        /// 查詢清單
        /// </summary>
        public ObservableCollection<SysGroup> SysGroupList
        {
            get
            {
                if (_sysGroupList == null)
                {
                    var result = ApiUtil.HttpClientEx<ApiResult<ObservableCollection<SysGroup>>>(
                        UAACRoute.Service(), UAACRoute.SysApp.GetSysCatalogSortList,
                        method: HttpVerbs.Get,
                        queryParams: QueryParam);
                    _sysGroupList = result.Data;
                }
                return _sysGroupList;
            }
            set => Set(ref _sysGroupList, value);
        }


        private DelegateCommand _okCommand;
        public DelegateCommand OKCommand =>
            _okCommand ?? (_okCommand = new DelegateCommand
            (OnOK));
        private void OnOK()
        {
            List<SysCatalog> sysCatalogs = new List<SysCatalog>();
            short seq = 1;

            foreach (var sysGroup in SysGroupList)
            {
                sysCatalogs.Add(new SysCatalog
                {
                    SysCatalogId = sysGroup.SysCatalogId,
                    RootId = sysGroup.RootId,
                    CatalogId = sysGroup.CatalogId,
                    SysId = sysGroup.SysId,
                    Seq = seq++,
                    Activate = true,
                    MUserId = LoginViewModel.LoginUser.EmpId,
                    //MDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                }); ;
            }

            var result = ApiUtil.HttpClientEx<ApiResult<List<SysCatalog>>>(
                UAACRoute.Service(), UAACRoute.SysCatalog.BatchPatchSeq,
                method: HttpVerbs.Patch,
                body: sysCatalogs);

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
