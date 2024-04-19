using CommonServiceLocator;
using FluentValidation;
using Models;

namespace ViewModels.FluentValidators
{
    public class RoleUserEditViewModelValidator : AbstractValidator<RoleUserEditViewModel>
    {
        public RoleUserEditViewModelValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(vm => vm.EditedItem)
                .SetValidator(ServiceLocator.Current.GetInstance<IValidator<RoleUser>>());
        }
    }
}
