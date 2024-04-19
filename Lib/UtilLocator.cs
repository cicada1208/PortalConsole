using CommonServiceLocator;
using GalaSoft.MvvmLight.Ioc;

namespace Lib
{
    public class UtilLocator
    {
        public UtilLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            if (!SimpleIoc.Default.IsRegistered<ApiUtil>())
                SimpleIoc.Default.Register<ApiUtil>();

            if (!SimpleIoc.Default.IsRegistered<CodecUtil>())
                SimpleIoc.Default.Register<CodecUtil>();

            if (!SimpleIoc.Default.IsRegistered<CommonUtil>())
                SimpleIoc.Default.Register<CommonUtil>();

            if (!SimpleIoc.Default.IsRegistered<ConfigUtil>())
                SimpleIoc.Default.Register<ConfigUtil>();

            if (!SimpleIoc.Default.IsRegistered<DataTableUtil>())
                SimpleIoc.Default.Register<DataTableUtil>();

            if (!SimpleIoc.Default.IsRegistered<DateTimeUtil>())
                SimpleIoc.Default.Register<DateTimeUtil>();

            if (!SimpleIoc.Default.IsRegistered<HostUtil>())
                SimpleIoc.Default.Register<HostUtil>();

            if (!SimpleIoc.Default.IsRegistered<LinqUtil>())
                SimpleIoc.Default.Register<LinqUtil>();

            if (!SimpleIoc.Default.IsRegistered<MedicalUtil>())
                SimpleIoc.Default.Register<MedicalUtil>();

            if (!SimpleIoc.Default.IsRegistered<ModelUtil>())
                SimpleIoc.Default.Register<ModelUtil>();

            if (!SimpleIoc.Default.IsRegistered<RuleUtil>())
                SimpleIoc.Default.Register<RuleUtil>();

            if (!SimpleIoc.Default.IsRegistered<SqlBuildUtil>())
                SimpleIoc.Default.Register<SqlBuildUtil>();

            if (!SimpleIoc.Default.IsRegistered<StrUtil>())
                SimpleIoc.Default.Register<StrUtil>();

            if (!SimpleIoc.Default.IsRegistered<TaskUtil>())
                SimpleIoc.Default.Register<TaskUtil>();
        }

        public ApiUtil Api =>
            ServiceLocator.Current.GetInstance<ApiUtil>();

        public CodecUtil Codec =>
            ServiceLocator.Current.GetInstance<CodecUtil>();

        public CommonUtil Common =>
            ServiceLocator.Current.GetInstance<CommonUtil>();

        public ConfigUtil Config =>
            ServiceLocator.Current.GetInstance<ConfigUtil>();

        public DataTableUtil DataTable =>
            ServiceLocator.Current.GetInstance<DataTableUtil>();

        public DateTimeUtil DateTime =>
            ServiceLocator.Current.GetInstance<DateTimeUtil>();

        public HostUtil Host =>
            ServiceLocator.Current.GetInstance<HostUtil>();

        public LinqUtil Linq =>
            ServiceLocator.Current.GetInstance<LinqUtil>();

        public MedicalUtil Medical =>
            ServiceLocator.Current.GetInstance<MedicalUtil>();

        public ModelUtil Model =>
            ServiceLocator.Current.GetInstance<ModelUtil>();

        public RuleUtil Rule =>
            ServiceLocator.Current.GetInstance<RuleUtil>();

        public SqlBuildUtil SqlBuild =>
            ServiceLocator.Current.GetInstance<SqlBuildUtil>();

        public StrUtil Str =>
            ServiceLocator.Current.GetInstance<StrUtil>();

        public TaskUtil Task =>
            ServiceLocator.Current.GetInstance<TaskUtil>();

    }
}
