using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Lib.Wpf.ValueConverters
{
    public class RadioConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //value是radio接到binding變數的值後，來呼叫converter
            //converter負責判定接到的值是代表true還是false
            if (value == null || parameter == null )
                return false;
            string checkvalue = value.ToString();
            string targetvalue = parameter.ToString();
            
            bool r = checkvalue.Equals(targetvalue,
                StringComparison.InvariantCultureIgnoreCase);
            return r;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //value 是目前 radiobutton 的 true/false
            //在這裡把 parameter 傳回 View-Model
            if (value == null || parameter == null)
                return null;
            bool usevalue = (bool)value;

            if (usevalue)
                return parameter.ToString();

            return null;
        }
    }
}
