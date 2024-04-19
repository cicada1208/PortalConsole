using System;
using System.Globalization;

namespace Lib.Wpf.ValueConverters
{
    public class DateTimeFormatConverter : BaseConverter<DateTimeFormatConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            //if (value.NullableToStr() == string.Empty) return null;
            //return DateTime.Parse(value.NullableToStr());

            string valueStr = value.NullableToStr();
            valueStr = valueStr == "0" ? string.Empty : valueStr;
            string format = parameter.NullableToStr();

            if (valueStr == string.Empty)
                return null;
            else if (format == string.Empty)
                return DateTime.Parse(valueStr);
            else if (format.StartsWith("yyy") && !format.StartsWith("yyyy")) // ROC
                return DateTime.Parse(DateTimeUtil.ConvertROC(valueStr,
                    inFormat: format, outFormat: "yyyy/MM/dd HH:mm:ss"));
            else // AD
            {
                if (format.StartsWith("HH") || format.StartsWith("mm"))
                    valueStr = valueStr.PadLeft(format.Length, '0');
                return DateTime.ParseExact(valueStr, format, null);
            }
        }

        public override object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        { // string 可自動轉型為 targetType
            //if (value.NullableToStr() == string.Empty) return string.Empty;
            //return ((DateTime)value).ToString("yyyy/MM/dd HH:mm:ss");

            string format = parameter.NullableToStr();

            if (value.IsNullOrWhiteSpace())
            {
                if (targetType == typeof(string))
                    return string.Empty;
                else if (targetType.Name == typeof(Nullable<>).Name)
                    return null;
                else
                    return 0;
            }
            else if (format == string.Empty)
            {
                if (targetType == typeof(string))
                    return ((DateTime)value).ToString("yyyy/MM/dd HH:mm:ss");
                else
                    return ((DateTime)value).ToString("yyyyMMddHHmmss");
            }
            else if (format.StartsWith("yyy") && !format.StartsWith("yyyy")) // ROC
            {
                CultureInfo twCulture = new CultureInfo("zh-TW");
                twCulture.DateTimeFormat.Calendar = new TaiwanCalendar();
                return ((DateTime)value).ToString(format, twCulture);
            }
            else // AD
                return ((DateTime)value).ToString(format);
        }

    }
}
