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
    public class FuncCatalogSortViewModel : BaseViewModel<FuncCatalogSortViewModel>
    {
        private FuncCatalogQueryParam _queryParam;
        /// <summary>
        /// 查詢參數
        /// </summary>
        public FuncCatalogQueryParam QueryParam
        {
            get => _queryParam ?? (_queryParam = new FuncCatalogQueryParam());
            set => Set(ref _queryParam, value);
        }

        private ObservableCollection<FuncGroup> _funcGroupList;
        /// <summary>
        /// 查詢清單
        /// </summary>
        public ObservableCollection<FuncGroup> FuncGroupList
        {
            get
            {
                if (_funcGroupList == null)
                {
                    var result = ApiUtil.HttpClientEx<ApiResult<ObservableCollection<FuncGroup>>>(
                        UAACRoute.Service(), UAACRoute.Func.GetFuncCatalogSortList,
                        method: HttpVerbs.Get,
                        queryParams: QueryParam);
                    _funcGroupList = result.Data;
                }
                return _funcGroupList;
            }
            set => Set(ref _funcGroupList, value);
        }


        private DelegateCommand _okCommand;
        public DelegateCommand OKCommand =>
            _okCommand ?? (_okCommand = new DelegateCommand
            (OnOK));
        private void OnOK()
        {
            List<FuncCatalog> funcCatalogs = new List<FuncCatalog>();
            short seq = 1;

            foreach (var funcGroup in FuncGroupList)
            {
                funcCatalogs.Add(new FuncCatalog
                {
                    FuncCatalogId = funcGroup.FuncCatalogId,
                    RootId = funcGroup.RootId,
                    CatalogId = funcGroup.CatalogId,
                    FuncId = funcGroup.FuncId,
                    Seq = seq++,
                    Activate = true,
                    MUserId = LoginViewModel.LoginUser.EmpId,
                    //MDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")
                }); ;
            }

            var result = ApiUtil.HttpClientEx<ApiResult<List<FuncCatalog>>>(
                UAACRoute.Service(), UAACRoute.FuncCatalog.BatchPatchSeq,
                method: HttpVerbs.Patch,
                body: funcCatalogs);

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
