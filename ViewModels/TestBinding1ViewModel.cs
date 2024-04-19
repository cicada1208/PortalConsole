using Lib.Wpf;
using Models;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace ViewModels
{
    public class TestBinding1ViewModel : BaseViewModel<TestBinding1ViewModel>
    {
        private string _description = "TestBinding1";
        public string Description
        {
            get => _description;
            set => Set(ref _description, value);
        }

        private DelegateCommand _modifyDespCommand;
        public DelegateCommand ModifyDespCommand =>
            _modifyDespCommand ?? (_modifyDespCommand = new DelegateCommand
            (OnModifyDesp));
        private void OnModifyDesp()
        {
            Description = "TestBinding1 Modified";
        }


        private string _date1 = "2022-01-02";
        /// <summary>
        /// yyyy-MM-dd
        /// </summary>
        public string Date1
        {
            get => _date1;
            set => Set(ref _date1, value);
        }

        private int? _date2 = 20220103;
        /// <summary>
        /// yyyyMMdd
        /// </summary>
        public int? Date2
        {
            get => _date2;
            set => Set(ref _date2, value);
        }

        private string _date3 = "2022/01/02 03:04:05";
        /// <summary>
        /// yyyy/MM/dd HH:mm:ss
        /// </summary>
        public string Date3
        {
            get => _date3;
            set => Set(ref _date3, value);
        }

        private long? _date4 = 20220102030405;
        /// <summary>
        /// yyyyMMddHHmmss
        /// </summary>
        public long? Date4
        {
            get => _date4;
            set => Set(ref _date4, value);
        }

        private string _date5 = "99/01/05";
        /// <summary>
        /// yyy/MM/dd
        /// </summary>
        public string Date5
        {
            get => _date5;
            set => Set(ref _date5, value);
        }

        private int? _date6 = 990105;
        /// <summary>
        /// yyyMMdd
        /// </summary>
        public int? Date6
        {
            get => _date6;
            set => Set(ref _date6, value);
        }

        private string _date7 = "99-01-02 03:04:05";
        /// <summary>
        /// yyy-MM-dd HH:mm:ss
        /// </summary>
        public string Date7
        {
            get => _date7;
            set => Set(ref _date7, value);
        }

        private long? _date8 = 990102030405;
        /// <summary>
        /// yyyMMddHHmmss
        /// </summary>
        public long? Date8
        {
            get => _date8;
            set => Set(ref _date8, value);
        }

        private string _date9 = "03:04";
        /// <summary>
        /// HH:mm
        /// </summary>
        public string Date9
        {
            get => _date9;
            set => Set(ref _date9, value);
        }

        private int? _date10 = 0506;
        /// <summary>
        /// HHmm
        /// </summary>
        public int? Date10
        {
            get => _date10;
            set => Set(ref _date10, value);
        }

    }
}