using CommonServiceLocator;
using FluentValidation;
using Models;

namespace ViewModels.FluentValidators
{
    public class UserPermissionEditViewModelValidator : AbstractValidator<UserPermissionEditViewModel>
    {
        public UserPermissionEditViewModelValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(vm => vm.EditedItem)
                .SetValidator(ServiceLocator.Current.GetInstance<IValidator<UserPermission>>());
        }
    }
}
