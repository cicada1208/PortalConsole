using System;
using System.Collections.ObjectModel;

namespace Models
{
    public class Item : BaseModel<Item>
    {
        private object _value;
        public object Value
        {
            get => _value;
            set => Set(ref _value, value);
        }

        private string _text;
        public string Text
        {
            get => _text;
            set => Set(ref _text, value);
        }
    }

    public class ActivateData
    {
        public static ObservableCollection<Item> GetCollection()
        {
            return new ObservableCollection<Item>
            {
                new Item { Value=true, Text="啟用" },
                new Item { Value=false, Text="停用" }
            };
        }

        public static ObservableCollection<Item> GetFilterCollection()
        {
            ObservableCollection<Item> items = GetCollection();
            items.Insert(0, new Item { Value = null, Text = "全部" });
            return items;
        }
    }

    public class EnumData<TEnum>
    {
        public static ObservableCollection<Item> GetCollection()
        {
            ObservableCollection<Item> items = new ObservableCollection<Item>();

            foreach (TEnum enumType in Enum.GetValues(typeof(TEnum)))
            {
                items.Add(new Item
                {
                    Value = enumType,
                    Text = Enum.GetName(typeof(TEnum), enumType)
                });
            }

            return items;
        }

        public static ObservableCollection<Item> GetFilterCollection()
        {
            ObservableCollection<Item> items = GetCollection();
            items.Insert(0, new Item { Value = null, Text = "全部" });
            return items;
        }
    }

}
