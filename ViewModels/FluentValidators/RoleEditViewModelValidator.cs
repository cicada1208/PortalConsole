using CommonServiceLocator;
using FluentValidation;
using Models;

namespace ViewModels.FluentValidators
{
    public class RoleEditViewModelValidator : AbstractValidator<RoleEditViewModel>
    {
        public RoleEditViewModelValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(vm => vm.EditedItem)
                .SetValidator(ServiceLocator.Current.GetInstance<IValidator<Role>>());
        }
    }
}
