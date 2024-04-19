using System;

namespace Lib.Wpf.ValueConverters
{
    public class ActivateConverter : BaseConverter<ActivateConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (value)
            {
                case true:
                    return "啟用";
                case false:
                    return "停用";
                default:
                    return value;
            }
        }
    }
}
