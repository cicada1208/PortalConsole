using FluentValidation;
using Lib;
using static Params.FuncParam;

namespace Models.FluentValidators
{
    public class FuncValidator : AbstractValidator<Func>
    {
        public FuncValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(m => m.FuncId)
                .NotEmpty()
                .MaxLen(m => m.GetPropertyMaxLength(nameof(m.FuncId)))
                .WithName(m => m.GetPropertyDisplayName(nameof(m.FuncId)));

            RuleFor(m => m.SysId)
                .NotEmpty()
                .MaxLen(m => m.GetPropertyMaxLength(nameof(m.SysId)))
                .WithName(m => m.GetPropertyDisplayName(nameof(m.SysId)));

            RuleFor(m => m.FuncName)
                .NotEmpty()
                .MaxLenUnicode(m => m.GetPropertyMaxLength(nameof(m.FuncName)))
                .WithName(m => m.GetPropertyDisplayName(nameof(m.FuncName)));

            RuleFor(m => m.FuncType)
                .NotNull()
                .WithName(m => m.GetPropertyDisplayName(nameof(m.FuncType)));

            RuleFor(m => m.BasePath)
                .Empty()
                .When(m => m.FuncType == FuncType.Root ||
                m.FuncType == FuncType.Catalog,
                ApplyConditionTo.CurrentValidator)
                .NotEmpty()
                .When(m => !(m.FuncType == FuncType.Root ||
                m.FuncType == FuncType.Catalog ||
                m.FuncType == FuncType.CychMisExe),
                ApplyConditionTo.CurrentValidator)
                .MaxLen(m => m.GetPropertyMaxLength(nameof(m.BasePath)))
                .WithName(m => m.GetPropertyDisplayName(nameof(m.BasePath)));

            RuleFor(m => m.SubPath)
                .Empty()
                .When(m => m.FuncType == FuncType.Root ||
                m.FuncType == FuncType.Catalog,
                ApplyConditionTo.CurrentValidator)
                .NotEmpty()
                .When(m => m.FuncType == FuncType.WpfMethod,
                ApplyConditionTo.CurrentValidator)
                .MaxLen(m => m.GetPropertyMaxLength(nameof(m.SubPath)))
                .WithName(m => m.GetPropertyDisplayName(nameof(m.SubPath)));

            RuleFor(m => m.Assembly)
                .Empty()
                .When(m => m.FuncType == FuncType.Root ||
                m.FuncType == FuncType.Catalog,
                ApplyConditionTo.CurrentValidator)
                .NotEmpty()
                .When(m => m.FuncType == FuncType.VersionExe,
                ApplyConditionTo.CurrentValidator)
                .MaxLen(m => m.GetPropertyMaxLength(nameof(m.Assembly)))
                .WithName(m => m.GetPropertyDisplayName(nameof(m.Assembly)));

            RuleFor(m => m.ViewName)
                .Empty()
                .When(m => m.FuncType == FuncType.Root ||
                m.FuncType == FuncType.Catalog,
                ApplyConditionTo.CurrentValidator)
                .NotEmpty()
                .When(m => m.FuncType == FuncType.WpfPage ||
                m.FuncType == FuncType.WpfWindow ||
                m.FuncType == FuncType.VuePage,
                ApplyConditionTo.CurrentValidator)
                .MaxLen(m => m.GetPropertyMaxLength(nameof(m.ViewName)))
                .WithName(m => m.GetPropertyDisplayName(nameof(m.ViewName)));

            RuleFor(m => m.ViewComponent)
                .Empty()
                .When(m => m.FuncType == FuncType.Root ||
                m.FuncType == FuncType.Catalog,
                ApplyConditionTo.CurrentValidator)
                .NotEmpty()
                .When(m => m.FuncType == FuncType.VuePage,
                ApplyConditionTo.CurrentValidator)
                .MaxLen(m => m.GetPropertyMaxLength(nameof(m.ViewComponent)))
                .WithName(m => m.GetPropertyDisplayName(nameof(m.ViewComponent)));
        }
    }
}
