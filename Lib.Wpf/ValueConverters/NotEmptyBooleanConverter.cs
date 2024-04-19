using System;

namespace Lib.Wpf.ValueConverters
{
    public class NotEmptyBooleanConverter : BaseConverter<NotEmptyBooleanConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.NullableToStr() == string.Empty)
                return false;
            else
                return true;
        }
    }
}
