using Lib;
using Lib.Wpf;
using Lib.Wpf.Routes;
using Models;
using Params;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using static Params.SysAppParam;

namespace ViewModels
{
    public class TestComboBoxViewModel : BaseViewModel<TestComboBoxViewModel>
    {
        public TestComboBoxViewModel()
        {
            Init().FireAndForget();
        }

        private async Task Init()
        {
            var result = await ApiUtil.HttpClientExAsync<ApiResult<ObservableCollection<SysApp>>>(
                UAACRoute.Service(), UAACRoute.SysApp.Controller,
                method: ApiParam.HttpVerbs.Get);

            SysIdList = new ObservableCollection<SysApp>(result.Data
                .Where(s => !(s.SysType == SysType.Root || s.SysType == SysType.Catalog))
                .OrderByDescending(s => s.Activate).ThenBy(s => s.SysId));
        }

        private ObservableCollection<SysApp> _sysIdList;
        /// <summary>
        /// 系統代碼(除 Root & Catalog)
        /// </summary>
        public ObservableCollection<SysApp> SysIdList
        {
            get => _sysIdList;
            set => Set(ref _sysIdList, value);
        }

        private SysApp _selectedSysApp;
        public SysApp SelectedSysApp
        {
            get => _selectedSysApp;
            set => Set(ref _selectedSysApp, value);
        }

        private string _selectedSysId;
        public string SelectedSysId
        {
            get => _selectedSysId;
            set => Set(ref _selectedSysId, value);
        }

        private string _selectedSysId2;
        public string SelectedSysId2
        {
            get => _selectedSysId2;
            set => Set(ref _selectedSysId2, value);
        }

        private ObservableCollection<Func> _viewNameList;
        /// <summary>
        /// 視圖名稱(篩選系統代碼的子項)
        /// </summary>
        public ObservableCollection<Func> ViewNameList
        {
            get => _viewNameList;
            set => Set(ref _viewNameList, value);
        }

        private string _selectedViewName2;
        public string SelectedViewName2
        {
            get => _selectedViewName2;
            set => Set(ref _selectedViewName2, value);
        }

        private string _selectedSysId3;
        public string SelectedSysId3
        {
            get => _selectedSysId3;
            set => Set(ref _selectedSysId3, value);
        }

        private Func<Task> _queryViewNameListDebounce;
        private Func<Task> QueryViewNameListDebounce => _queryViewNameListDebounce ??
            (_queryViewNameListDebounce = ((Func<CancellationToken, Task>)QueryViewNameList).Debounce(2000));


        private DelegateCommand _selectionChangedCommand;
        public DelegateCommand SelectionChangedCommand =>
            _selectionChangedCommand ?? (_selectionChangedCommand = new DelegateCommand
            (OnSelectionChanged));
        private void OnSelectionChanged()
        {
            Console.WriteLine($"{nameof(OnSelectionChanged)}: {SelectedSysId3}");
        }

        private DelegateCommand _filterSysIdCommand;
        public DelegateCommand FilterSysIdCommand =>
            _filterSysIdCommand ?? (_filterSysIdCommand = new DelegateCommand
            (OnFilterSysId));
        /// <summary>
        /// 選取系統代碼，查詢視圖名稱
        /// </summary>
        private void OnFilterSysId()
        {
            ViewNameList?.Clear();
            QueryViewNameListDebounce();
        }

        private async Task QueryViewNameList(CancellationToken cancellationToken = default)
        {
            Console.WriteLine(nameof(QueryViewNameList));

            if (!SelectedSysId2.IsNullOrWhiteSpace())
            {
                var result = await ApiUtil.HttpClientExAsync<ApiResult<ObservableCollection<Func>>>(
                 UAACRoute.Service(), UAACRoute.Func.Controller,
                 method: ApiParam.HttpVerbs.Get,
                 queryParams: new Func { SysId = SelectedSysId2 },
                 cancellationToken: cancellationToken);

                if (result.Succ)
                    ViewNameList = new ObservableCollection<Func>(result.Data
                        .Where(f => !f.ViewName.IsNullOrWhiteSpace())
                        .OrderByDescending(f => f.Activate).ThenBy(f => f.ViewName));
            }
        }

    }
}
