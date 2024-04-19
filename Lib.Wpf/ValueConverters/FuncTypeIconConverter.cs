using MaterialDesignThemes.Wpf;
using System;
using static Params.FuncParam;

namespace Lib.Wpf.ValueConverters
{
    public class FuncTypeIconConverter : BaseConverter<FuncTypeIconConverter>
    {
        public override object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            switch (value)
            {
                case FuncType.Root:
                    return PackIconKind.Home;
                case FuncType.Catalog:
                    return PackIconKind.None; // PackIconKind.FileTree
                case FuncType.Site:
                    return PackIconKind.Web;
                case FuncType.VersionExe:
                case FuncType.CychMisExe:
                case FuncType.HISExe:
                    return PackIconKind.ApplicationCogOutline;
                case FuncType.WpfPage:
                    return PackIconKind.FileEditOutline;
                case FuncType.WpfWindow:
                    return PackIconKind.Monitor;
                case FuncType.VuePage:
                    return PackIconKind.Vuejs;
                case FuncType.WpfMethod:
                    return PackIconKind.Function;
                default:
                    return PackIconKind.None;
            }
        }
    }
}
