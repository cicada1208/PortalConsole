using CommonServiceLocator;
using FluentValidation;
using Models;

namespace ViewModels.FluentValidators
{
    public class SysAppEditViewModelValidator : AbstractValidator<SysAppEditViewModel>
    {
        public SysAppEditViewModelValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(vm => vm.EditedItem)
                .SetValidator(ServiceLocator.Current.GetInstance<IValidator<SysApp>>());
        }
    }
}
