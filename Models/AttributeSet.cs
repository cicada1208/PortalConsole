namespace Models
{
    public class AttributeSet : BaseModel<AttributeSet>
    {
        private string _Attribute;
        public string Attribute
        {
            get => _Attribute;
            set => Set(ref _Attribute, value);
        }

        private string _Attribute1;
        public string Attribute1
        {
            get => _Attribute1;
            set => Set(ref _Attribute1, value);
        }

        private string _Attribute2;
        public string Attribute2
        {
            get => _Attribute2;
            set => Set(ref _Attribute2, value);
        }

        private string _Attribute3;
        public string Attribute3
        {
            get => _Attribute3;
            set => Set(ref _Attribute3, value);
        }

    }
}
