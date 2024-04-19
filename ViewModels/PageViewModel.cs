namespace ViewModels
{
    public class PageViewModel : BaseViewModel<PageViewModel>
    {
        private FuncListViewModel _funcListViewModel;
        public FuncListViewModel FuncListViewModel
        {
            get => _funcListViewModel;
            set => Set(ref _funcListViewModel, value);
        }

    }
}
