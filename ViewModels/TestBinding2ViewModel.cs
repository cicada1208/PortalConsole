namespace ViewModels
{
    public class TestBinding2ViewModel : BaseViewModel<TestBinding2ViewModel>
    {
        private string _description = "TestBinding2";
        public string Description
        {
            get => _description;
            set => Set(ref _description, value);
        }
    }
}
