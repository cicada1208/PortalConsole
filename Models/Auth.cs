namespace Models
{
    public class Auth : AuthBase<Auth>
    {
        private string _Password;
        /// <summary>
        /// 使用者密碼
        /// </summary>
        public string Password
        {
            get => _Password;
            set => Set(ref _Password, value);
        }
    }
}
