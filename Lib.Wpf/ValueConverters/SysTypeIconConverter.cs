using MaterialDesignThemes.Wpf;
using System;
using static Params.SysAppParam;

namespace Lib.Wpf.ValueConverters
{
    public class SysTypeIconConverter : BaseConverter<SysTypeIconConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (value)
            {
                case SysType.Root:
                    return PackIconKind.Home;
                case SysType.Catalog:
                    return PackIconKind.None; // PackIconKind.FileTree
                case SysType.Site:
                    return PackIconKind.Web;
                case SysType.VersionExe:
                case SysType.CychMisExe:
                case SysType.HISExe:
                    return PackIconKind.ApplicationCogOutline;
                default:
                    return PackIconKind.None;
            }
        }
    }
}
