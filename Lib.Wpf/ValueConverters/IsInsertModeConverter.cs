using System;
using static Params.EditParam;

namespace Lib.Wpf.ValueConverters
{
    public class IsInsertModeConverter : BaseConverter<IsInsertModeConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (value)
            {
                case EditMode.INSERT:
                    return true;
                default:
                    return false;
            }
        }
    }
}
