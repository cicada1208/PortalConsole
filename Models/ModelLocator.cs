/*
  In App.xaml:
  <Application.Resources>
      <vm:ViewModelLocator xmlns:vm="clr-namespace:Models"
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
using Models.FluentValidators;
using System;

namespace Models
{
    /// <summary>
    /// This class contains static references to all the models in the
    /// application and provides an entry point for the bindings.
    /// </summary>
    public class ModelLocator
    {
        /// <summary>
        /// Initializes a new instance of the ModelLocator class.
        /// </summary>
        public ModelLocator()
        {
            ServiceLocator.SetLocatorProvider(() => SimpleIoc.Default);

            // Register Validator
            if (!SimpleIoc.Default.IsRegistered<IValidator<SysApp>>())
                SimpleIoc.Default.Register<IValidator<SysApp>, SysAppValidator>();

            if (!SimpleIoc.Default.IsRegistered<IValidator<Func>>())
                SimpleIoc.Default.Register<IValidator<Func>, FuncValidator>();

            if (!SimpleIoc.Default.IsRegistered<IValidator<Role>>())
                SimpleIoc.Default.Register<IValidator<Role>, RoleValidator>();

            if (!SimpleIoc.Default.IsRegistered<IValidator<RoleUser>>())
                SimpleIoc.Default.Register<IValidator<RoleUser>, RoleUserValidator>();

            if (!SimpleIoc.Default.IsRegistered<IValidator<RolePermission>>())
                SimpleIoc.Default.Register<IValidator<RolePermission>, RolePermissionValidator>();

            if (!SimpleIoc.Default.IsRegistered<IValidator<UserPermission>>())
                SimpleIoc.Default.Register<IValidator<UserPermission>, UserPermissionValidator>();
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

        public static void Cleanup()
        {
            // TODO Clear the ViewModels
        }
    }
}