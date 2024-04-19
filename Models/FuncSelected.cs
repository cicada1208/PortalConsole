namespace Models
{
    public class FuncSelected : FuncBase<FuncSelected>
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
