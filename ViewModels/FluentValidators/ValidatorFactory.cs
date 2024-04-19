using FluentValidation;
using System;

namespace ViewModels.FluentValidators
{
    public class ValidatorFactory : ValidatorFactoryBase
    {
        //public override IValidator CreateInstance(Type validatorType) =>
        //    ServiceLocator.Current.GetInstance(validatorType) as IValidator;
        ////SimpleIoc.Default.GetInstance(validatorType) as IValidator;

        public override IValidator CreateInstance(Type validatorType) =>
            ViewModelLocator.Validator(validatorType);


    }
}
