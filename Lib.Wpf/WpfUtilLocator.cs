using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace Lib.Wpf
{
    public class WpfUtilLocator
    {
        public WpfUtilLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (!SimpleIoc.Default.IsRegistered<BrowserUtil>())
                SimpleIoc.Default.Register<BrowserUtil>();

            if (!SimpleIoc.Default.IsRegistered<ColorUtil>())
                SimpleIoc.Default.Register<ColorUtil>();

            if (!SimpleIoc.Default.IsRegistered<CtrlUtil>())
                SimpleIoc.Default.Register<CtrlUtil>();

            if (!SimpleIoc.Default.IsRegistered<ExcelUtil>())
                SimpleIoc.Default.Register<ExcelUtil>();

            if (!SimpleIoc.Default.IsRegistered<FileUtil>())
                SimpleIoc.Default.Register<FileUtil>();

            if (!SimpleIoc.Default.IsRegistered<HostUtil>())
                SimpleIoc.Default.Register<HostUtil>();

            if (!SimpleIoc.Default.IsRegistered<MediaUtil>())
                SimpleIoc.Default.Register<MediaUtil>();
        }

        public BrowserUtil Browser =>
            ServiceLocator.Current.GetInstance<BrowserUtil>();

        public ColorUtil Color =>
            ServiceLocator.Current.GetInstance<ColorUtil>();

        public CtrlUtil Ctrl =>
            ServiceLocator.Current.GetInstance<CtrlUtil>();

        public ExcelUtil Excel =>
            ServiceLocator.Current.GetInstance<ExcelUtil>();

        public FileUtil File =>
            ServiceLocator.Current.GetInstance<FileUtil>();

        public HostUtil Host =>
            ServiceLocator.Current.GetInstance<HostUtil>();

        public MediaUtil Media =>
            ServiceLocator.Current.GetInstance<MediaUtil>();

    }
}
