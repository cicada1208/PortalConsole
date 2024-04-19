namespace ViewModels
{
    public class TestBinding3ViewModel : BaseViewModel<TestBinding3ViewModel>
    {
        private string _description = "TestBinding3";
        public string Description
        {
            get => _description;
            set => Set(ref _description, value);
        }
    }
}
