using System;
using static Params.EditParam;


namespace Lib.Wpf.ValueConverters
{
    public class IsUpdateModeConverter : BaseConverter<IsUpdateModeConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (value)
            {
                case EditMode.UPDATE:
                    return true;
                default:
                    return false;
            }
        }
    }
}
