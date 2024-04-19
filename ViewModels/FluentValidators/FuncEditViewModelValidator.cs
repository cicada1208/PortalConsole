using CommonServiceLocator;
using FluentValidation;
using Models;

namespace ViewModels.FluentValidators
{
    public class FuncEditViewModelValidator : AbstractValidator<FuncEditViewModel>
    {
        public FuncEditViewModelValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(vm => vm.EditedItem)
                .SetValidator(ServiceLocator.Current.GetInstance<IValidator<Func>>());
        }
    }
}
