using Lib;
using Lib.Wpf.Routes;
using Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Data;
using static Params.ApiParam;
using static Params.SysAppParam;

namespace ViewModels
{
    public class TestListBoxViewModel : BaseViewModel<TestListBoxViewModel>
    {
        public TestListBoxViewModel()
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

            // 設定 Collection 在 View 中如何分組顯示
            CollectionView groupView = CollectionViewSource.GetDefaultView(SysGroupList) as CollectionView;
            PropertyGroupDescription groupDesp = new PropertyGroupDescription(nameof(SysGroup.SysType));
            groupView?.GroupDescriptions.Clear();
            groupView?.GroupDescriptions.Add(groupDesp);
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

        private ObservableCollection<SysGroup> _selectedSysGroupList = new ObservableCollection<SysGroup>();
        public ObservableCollection<SysGroup> SelectedSysGroupList
        {
            get => _selectedSysGroupList;
            set => Set(ref _selectedSysGroupList, value);
        }


        private ObservableCollection<Item> _recStList;
        public ObservableCollection<Item> RecStList
        {
            get
            {
                if (_recStList == null)
                {
                    _recStList = new ObservableCollection<Item>
                    {
                        new Item { Value = string.Empty, Text = "作用中" },
                        new Item { Value = "D", Text = "刪除" },
                        new Item { Value = "test", Text = "測試測試測試測試測試測試測試測試測試測試測試測試測試End" }
                    };
                }
                return _recStList;
            }
            set => Set(ref _recStList, value);
        }

        private string _selectedRecSt;
        [Display(Name = "單選狀態")]
        public string SelectedRecSt
        {
            get => _selectedRecSt;
            set => Set(ref _selectedRecSt, value);
        }

        private string _selectedRecSt2;
        public string SelectedRecSt2
        {
            get => _selectedRecSt2;
            set => Set(ref _selectedRecSt2, value);
        }

        // 搭配 mah:MultiSelectorHelper.SelectedItems 需先 new ObservableCollection<Item>()
        //private ObservableCollection<Item> _selectedRecStList = new ObservableCollection<Item>();
        private ObservableCollection<Item> _selectedRecStList;
        [Display(Name = "多選狀態")]
        public ObservableCollection<Item> SelectedRecStList
        {
            get
            {
                if (_selectedRecStList == null)
                {
                    // 預選項目
                    var defaultSelected = RecStList.Where(
                        r => r.Value.ToString() == "D" || r.Value.ToString() == "test");
                    _selectedRecStList = new ObservableCollection<Item>(defaultSelected);
                    _selectedRecStList.CollectionChanged += new NotifyCollectionChangedEventHandler(SelectedRecStList_CollectionChanged);
                }
                return _selectedRecStList;
            }
            set => Set(ref _selectedRecStList, value);
        }

        /// <summary>
        /// validation：when CollectionChanged, using RaisePropertyChanged let IDataErrorInfo know
        /// </summary>
        void SelectedRecStList_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e) =>
            RaisePropertyChanged(nameof(SelectedRecStList));

    }
}
