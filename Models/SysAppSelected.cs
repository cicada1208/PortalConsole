namespace Models
{
    public class SysAppSelected : SysAppBase<SysAppSelected>
    {
        private bool? _Selected = false;
        /// <summary>
        /// 選取
        /// </summary>
        public bool? Selected
        {
            get => _Selected;
            set => Set(ref _Selected, value);
        }
    }
}
