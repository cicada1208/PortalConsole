using FluentValidation;
using Lib;

namespace Models.FluentValidators
{
    public class RoleUserValidator : AbstractValidator<RoleUser>
    {
        public RoleUserValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(m => m.RoleId)
                .NotEmpty()
                .MaxLen(m => m.GetPropertyMaxLength(nameof(m.RoleId)))
                .WithName(m => m.GetPropertyDisplayName(nameof(m.RoleId)));

            RuleFor(m => m.CpnyId)
                .MaxLen(m => m.GetPropertyMaxLength(nameof(m.CpnyId)))
                .WithName(m => m.GetPropertyDisplayName(nameof(m.CpnyId)));

            RuleFor(m => m.DeptNo)
                .MaxLen(m => m.GetPropertyMaxLength(nameof(m.DeptNo)))
                .WithName(m => m.GetPropertyDisplayName(nameof(m.DeptNo)));

            RuleFor(m => m.Possie)
                .MaxLen(m => m.GetPropertyMaxLength(nameof(m.Possie)))
                .WithName(m => m.GetPropertyDisplayName(nameof(m.Possie)));

            RuleFor(m => m.Attribute)
                .MaxLen(m => m.GetPropertyMaxLength(nameof(m.Attribute)))
                .WithName(m => m.GetPropertyDisplayName(nameof(m.Attribute)));

            RuleFor(m => m.UserId)
                .MaxLen(m => m.GetPropertyMaxLength(nameof(m.UserId)))
                .WithName(m => m.GetPropertyDisplayName(nameof(m.UserId)));
        }
    }
}
