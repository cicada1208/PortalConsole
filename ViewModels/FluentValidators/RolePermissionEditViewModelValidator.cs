using CommonServiceLocator;
using FluentValidation;
using Models;

namespace ViewModels.FluentValidators
{
    public class RolePermissionEditViewModelValidator : AbstractValidator<RolePermissionEditViewModel>
    {
        public RolePermissionEditViewModelValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(vm => vm.EditedItem)
                .SetValidator(ServiceLocator.Current.GetInstance<IValidator<RolePermission>>());
        }
    }
}
