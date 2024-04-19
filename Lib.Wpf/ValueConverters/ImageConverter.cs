using Lib;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Media.Imaging;

namespace Lib.Wpf.ValueConverters
{
    public class ImageConverter : BaseConverter<ImageConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            byte[] imgByte;

            if (value == null)
                return value;
            else if (value.GetType() == typeof(Byte[])) // Byte Array
                imgByte = value as Byte[];
            else if (!value.ToString().TryBase64StringToByteArray(out imgByte)) // Base64String
                return value; // http or \\

            BitmapFrame img;
            using (MemoryStream stream = new MemoryStream(imgByte))
                img = BitmapFrame.Create(stream, BitmapCreateOptions.None, BitmapCacheOption.OnLoad);
            return img;
        }

    }
}
