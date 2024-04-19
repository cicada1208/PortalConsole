using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using System.Windows.Markup;

namespace Lib.Wpf.ValueConverters
{
    public abstract class BaseConverter<ConverterType>
        : MarkupExtension, IValueConverter where ConverterType : class, new()
    {
        /// <summary>
        /// 轉換從Source傳遞到Target的資料
        /// </summary>
        public abstract object Convert(object value, Type targetType, object parameter, CultureInfo culture);

        /// <summary>
        /// 轉換從Target傳遞回Source的資料
        /// </summary>
        public virtual object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return DependencyProperty.UnsetValue;
        }

        public static readonly ConverterType Instance = new ConverterType();

        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            return Instance;
        }

    }
}
