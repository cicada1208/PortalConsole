using FluentValidation;
using Lib;

namespace ViewModels.FluentValidators
{
    public class TestListBoxViewModelValidator : AbstractValidator<TestListBoxViewModel>
    {
        public TestListBoxViewModelValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(vm => vm.SelectedRecSt)
                .NotNull()
                .WithName(vm => vm.GetPropertyDisplayName(nameof(vm.SelectedRecSt)));

            RuleFor(vm => vm.SelectedRecStList)
                .NotEmpty()
                .WithName(vm => vm.GetPropertyDisplayName(nameof(vm.SelectedRecStList)));
        }
    }
}
