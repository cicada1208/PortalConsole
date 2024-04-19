using FluentValidation;
using Lib;

namespace Models.FluentValidators
{
    public class SysAppValidator : AbstractValidator<SysApp>
    {
        public SysAppValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(m => m.SysId)
                .NotEmpty()
                .MaxLen(m => m.GetPropertyMaxLength(nameof(m.SysId)))
                .WithName(m => m.GetPropertyDisplayName(nameof(m.SysId)));

            RuleFor(m => m.SysName)
                .NotEmpty()
                .MaxLenUnicode(m => m.GetPropertyMaxLength(nameof(m.SysName)))
                .WithName(m => m.GetPropertyDisplayName(nameof(m.SysName)));

            RuleFor(m => m.SysType)
                .NotNull()
                .WithName(m => m.GetPropertyDisplayName(nameof(m.SysType)));

            RuleFor(m => m.BasePath)
                .Empty()
                .When(m => m.SysType == Params.SysAppParam.SysType.Root ||
                m.SysType == Params.SysAppParam.SysType.Catalog,
                ApplyConditionTo.CurrentValidator)
                .NotEmpty()
                .When(m => !(m.SysType == Params.SysAppParam.SysType.Root ||
                m.SysType == Params.SysAppParam.SysType.Catalog ||
                m.SysType == Params.SysAppParam.SysType.CychMisExe),
                ApplyConditionTo.CurrentValidator)
                .MaxLen(m => m.GetPropertyMaxLength(nameof(m.BasePath)))
                .WithName(m => m.GetPropertyDisplayName(nameof(m.BasePath)));

            RuleFor(m => m.SubPath)
                .Empty()
                .When(m => m.SysType == Params.SysAppParam.SysType.Root ||
                m.SysType == Params.SysAppParam.SysType.Catalog,
                ApplyConditionTo.CurrentValidator)
                .MaxLen(m => m.GetPropertyMaxLength(nameof(m.SubPath)))
                .WithName(m => m.GetPropertyDisplayName(nameof(m.SubPath)));

            RuleFor(m => m.Assembly)
                .Empty()
                .When(m => m.SysType == Params.SysAppParam.SysType.Root ||
                m.SysType == Params.SysAppParam.SysType.Catalog,
                ApplyConditionTo.CurrentValidator)
                .NotEmpty()
                .When(m => m.SysType == Params.SysAppParam.SysType.VersionExe,
                ApplyConditionTo.CurrentValidator)
                .MaxLen(m => m.GetPropertyMaxLength(nameof(m.Assembly)))
                .WithName(m => m.GetPropertyDisplayName(nameof(m.Assembly)));
        }
    }
}
