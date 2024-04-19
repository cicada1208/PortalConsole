using FluentValidation;
using Lib;

namespace Models.FluentValidators
{
    public class RoleValidator : AbstractValidator<Role>
    {
        public RoleValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(m => m.RoleId)
                .NotEmpty()
                .MaxLen(m => m.GetPropertyMaxLength(nameof(m.RoleId)))
                .WithName(m => m.GetPropertyDisplayName(nameof(m.RoleId)));

            RuleFor(m => m.RoleName)
                .NotEmpty()
                .MaxLenUnicode(m => m.GetPropertyMaxLength(nameof(m.RoleName)))
                .WithName(m => m.GetPropertyDisplayName(nameof(m.RoleName)));

            RuleFor(m => m.Description)
                .MaxLenUnicode(m => m.GetPropertyMaxLength(nameof(m.Description)))
                .WithName(m => m.GetPropertyDisplayName(nameof(m.Description)));
        }
    }
}
