using Lib;
using Lib.Wpf;
using Lib.Wpf.Routes;
using Models;
using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using static Params.ApiParam;
using static Params.SysAppParam;

namespace ViewModels
{
    public class TestEventViewModel : BaseViewModel<TestEventViewModel>
    {
        public TestEventViewModel()
        {
            Init().FireAndForget();
        }

        private async Task Init()
        {
            var result = await ApiUtil.HttpClientExAsync<ApiResult<ObservableCollection<SysGroup>>>(
                UAACRoute.Service(), UAACRoute.SysApp.GetSysCatalogSortList,
                method: HttpVerbs.Get,
                queryParams: new SysCatalogQueryParam { RootId = SysId.BasicRoot, CatalogId = SysId.BasicRoot });

            SysGroupList = result.Data;
        }

        private ObservableCollection<SysGroup> _sysGroupList;
        public ObservableCollection<SysGroup> SysGroupList
        {
            get => _sysGroupList;
            set => Set(ref _sysGroupList, value);
        }

        private SysGroup _selectedSysGroup;
        public SysGroup SelectedSysGroup
        {
            get => _selectedSysGroup;
            set => Set(ref _selectedSysGroup, value);
        }

        private DelegateCommand _modifySysGroupCommand;
        public DelegateCommand ModifySysGroupCommand =>
            _modifySysGroupCommand ?? (_modifySysGroupCommand = new DelegateCommand
            (ModifySysGroup));
        private void ModifySysGroup()
        {
            Console.WriteLine($"Selected SysName: {SelectedSysGroup?.SysName}");
        }
    }
}
