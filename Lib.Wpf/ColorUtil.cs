using ColorHelper;
using Params;
using System;
using System.Collections.Generic;
using System.Windows.Media;

namespace Lib.Wpf
{
    public class ColorUtil
    {
        public System.Drawing.Color GetRandomColor()
        {
            Random randomNum_1 = new Random(Guid.NewGuid().GetHashCode());
            System.Threading.Thread.Sleep(randomNum_1.Next(1));
            int int_Red = randomNum_1.Next(255);

            Random randomNum_2 = new Random((int)DateTime.Now.Ticks);
            int int_Green = randomNum_2.Next(255);

            Random randomNum_3 = new Random(Guid.NewGuid().GetHashCode());
            int int_Blue = randomNum_3.Next(255);
            int_Blue = (int_Red + int_Green > 380) ? int_Red + int_Green - 380 : int_Blue;
            int_Blue = (int_Blue > 255) ? 255 : int_Blue;

            return GetDarkerColor(System.Drawing.Color.FromArgb(int_Red, int_Green, int_Blue));
        }

        public System.Drawing.Color GetDarkerColor(System.Drawing.Color color)
        {
            const int max = 255;
            int increase = new Random(Guid.NewGuid().GetHashCode()).Next(30, 255);// 可依需求調整此處的值

            int r = Math.Abs(Math.Min(color.R - increase, max));
            int g = Math.Abs(Math.Min(color.G - increase, max));
            int b = Math.Abs(Math.Min(color.B - increase, max));

            return System.Drawing.Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// 建立不重樣顏色集合
        /// </summary>
        public HashSet<string> CreateColorsStr(int colorsNum, ColorParam.ColorType colorType = ColorParam.ColorType.Normal)
        {
            HashSet<string> colors = new HashSet<string>();
            string color = string.Empty;

            do
            {
                switch (colorType)
                {
                    case ColorParam.ColorType.Light:
                        color = "#" + ColorGenerator.GetLightRandomColor<HEX>().Value;
                        break;
                    case ColorParam.ColorType.Dark:
                        color = "#" + ColorGenerator.GetDarkRandomColor<HEX>().Value;
                        break;
                    default:
                        color = "#" + ColorGenerator.GetRandomColor<HEX>().Value;
                        break;
                }
                colors.Add(color);
            } while (colors.Count < colorsNum);

            return colors;
        }

        /// <summary>
        /// 建立不重樣顏色集合
        /// </summary>
        public HashSet<SolidColorBrush> CreateColorsBrush(int colorsNum, ColorParam.ColorType colorType = ColorParam.ColorType.Normal)
        {
            HashSet<string> colors = CreateColorsStr(colorsNum, colorType);
            HashSet<SolidColorBrush> brushs = new HashSet<SolidColorBrush>();
            BrushConverter converter = new BrushConverter();

            foreach (var color in colors)
                brushs.Add(converter.ConvertFromString(color) as SolidColorBrush);

            return brushs;
        }

    }
}
