/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:ViewModels"
                           x:Key="Locator" />
  </Application.Resources>
  
  In the View:
  DataContext="{Binding Source={StaticResource Locator}, Path=ViewModelName}"

  You can also use Blend to do all this with the tool's support.
  See http://www.galasoft.ch/mvvm
*/

using CommonServiceLocator;
using FluentValidation;
using GalaSoft.MvvmLight.Ioc;
using System;
using ViewModels.FluentValidators;

namespace ViewModels
{
    /// <summary>
    /// This class contains static references to all the view models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ViewModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ViewModelLocator class.
        /// </summary>
        public ViewModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Register ViewModel
            //SimpleIoc.Default.Register<MainViewModel>();
            //SimpleIoc.Default.Register<MvvmViewModel>();

            // Register Validator
            if (!SimpleIoc.Default.IsRegistered<IValidator<LoginViewModel>>())
                SimpleIoc.Default.Register<IValidator<LoginViewModel>, LoginViewModelValidator>();

            if (!SimpleIoc.Default.IsRegistered<IValidator<SysAppEditViewModel>>())
                SimpleIoc.Default.Register<IValidator<SysAppEditViewModel>, SysAppEditViewModelValidator>();

            if (!SimpleIoc.Default.IsRegistered<IValidator<FuncEditViewModel>>())
                SimpleIoc.Default.Register<IValidator<FuncEditViewModel>, FuncEditViewModelValidator>();

            if (!SimpleIoc.Default.IsRegistered<IValidator<RoleEditViewModel>>())
                SimpleIoc.Default.Register<IValidator<RoleEditViewModel>, RoleEditViewModelValidator>();

            if (!SimpleIoc.Default.IsRegistered<IValidator<RoleUserEditViewModel>>())
                SimpleIoc.Default.Register<IValidator<RoleUserEditViewModel>, RoleUserEditViewModelValidator>();

            if (!SimpleIoc.Default.IsRegistered<IValidator<RolePermissionEditViewModel>>())
                SimpleIoc.Default.Register<IValidator<RolePermissionEditViewModel>, RolePermissionEditViewModelValidator>();

            if (!SimpleIoc.Default.IsRegistered<IValidator<UserPermissionEditViewModel>>())
                SimpleIoc.Default.Register<IValidator<UserPermissionEditViewModel>, UserPermissionEditViewModelValidator>();

            if (!SimpleIoc.Default.IsRegistered<IValidator<TestListBoxViewModel>>())
                SimpleIoc.Default.Register<IValidator<TestListBoxViewModel>, TestListBoxViewModelValidator>();

            ////if (ViewModelBase.IsInDesignModeStatic)
            ////{
            ////    // Create design time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DesignDataService>();
            ////}
            ////else
            ////{
            ////    // Create run time view services and models
            ////    SimpleIoc.Default.Register<IDataService, DataService>();
            ////}
        }

        public static IValidator Validator(Type validatorType)
        {
            try
            {
                return ServiceLocator.Current.GetInstance(validatorType) as IValidator;
            }
            catch (Exception)
            {
                return null;
            }
        }

        //public MainViewModel MainViewModel =>
        //    ServiceLocator.Current.GetInstance<MainViewModel>();

        //public MvvmViewModel MvvmViewModel =>
        //    ServiceLocator.Current.GetInstance<MvvmViewModel>();

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}