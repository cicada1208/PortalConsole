using FluentValidation;
using Lib;

namespace ViewModels.FluentValidators
{
    public class LoginViewModelValidator : AbstractValidator<LoginViewModel>
    {
        public LoginViewModelValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(vm => vm.UserId)
                .NotEmpty()
                .WithName(vm => vm.GetPropertyDisplayName(nameof(vm.UserId)));
        }

    }
}
