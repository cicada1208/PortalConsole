using System;

namespace Lib.Wpf.ValueConverters
{
    public class BooleanInvertedConverter : BaseConverter<BooleanInvertedConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value.Equals(true))
                return false;
            else if (value.Equals(false))
                return true;
            else
                return value;
        }
    }
}
