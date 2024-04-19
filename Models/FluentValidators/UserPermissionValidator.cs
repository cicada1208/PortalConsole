using FluentValidation;
using Lib;

namespace Models.FluentValidators
{
    public class UserPermissionValidator : AbstractValidator<UserPermission>
    {
        public UserPermissionValidator()
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(m => m.UserId)
                .NotEmpty()
                .MaxLen(m => m.GetPropertyMaxLength(nameof(m.UserId)))
                .WithName(m => m.GetPropertyDisplayName(nameof(m.UserId)));

            RuleFor(m => m.SysId)
                .NotEmpty()
                .MaxLen(m => m.GetPropertyMaxLength(nameof(m.SysId)))
                .WithName(m => m.GetPropertyDisplayName(nameof(m.SysId)));

            RuleFor(m => m.FuncId)
                .MaxLen(m => m.GetPropertyMaxLength(nameof(m.FuncId)))
                .WithName(m => m.GetPropertyDisplayName(nameof(m.FuncId)));

            RuleFor(m => m.QueryAuth)
                .Must((m, auth) => (m.QueryAuth.HasValue && m.AddAuth.HasValue && m.ModifyAuth.HasValue && m.DeleteAuth.HasValue && m.ExportAuth.HasValue && m.PrintAuth.HasValue) &&
                (m.QueryAuth.Value || m.AddAuth.Value || m.ModifyAuth.Value || m.DeleteAuth.Value || m.ExportAuth.Value || m.PrintAuth.Value)) // return true 才不會有訊息
                .WithMessage("權限需勾選一項");

            RuleFor(m => m.AddAuth)
                .Must((m, auth) => (m.QueryAuth.HasValue && m.AddAuth.HasValue && m.ModifyAuth.HasValue && m.DeleteAuth.HasValue && m.ExportAuth.HasValue && m.PrintAuth.HasValue) &&
                (m.QueryAuth.Value || m.AddAuth.Value || m.ModifyAuth.Value || m.DeleteAuth.Value || m.ExportAuth.Value || m.PrintAuth.Value))
                .WithMessage("權限需勾選一項");

            RuleFor(m => m.ModifyAuth)
                .Must((m, auth) => (m.QueryAuth.HasValue && m.AddAuth.HasValue && m.ModifyAuth.HasValue && m.DeleteAuth.HasValue && m.ExportAuth.HasValue && m.PrintAuth.HasValue) &&
                (m.QueryAuth.Value || m.AddAuth.Value || m.ModifyAuth.Value || m.DeleteAuth.Value || m.ExportAuth.Value || m.PrintAuth.Value))
                .WithMessage("權限需勾選一項");

            RuleFor(m => m.DeleteAuth)
                .Must((m, auth) => (m.QueryAuth.HasValue && m.AddAuth.HasValue && m.ModifyAuth.HasValue && m.DeleteAuth.HasValue && m.ExportAuth.HasValue && m.PrintAuth.HasValue) &&
                (m.QueryAuth.Value || m.AddAuth.Value || m.ModifyAuth.Value || m.DeleteAuth.Value || m.ExportAuth.Value || m.PrintAuth.Value))
                .WithMessage("權限需勾選一項");

            RuleFor(m => m.ExportAuth)
                .Must((m, auth) => (m.QueryAuth.HasValue && m.AddAuth.HasValue && m.ModifyAuth.HasValue && m.DeleteAuth.HasValue && m.ExportAuth.HasValue && m.PrintAuth.HasValue) &&
                (m.QueryAuth.Value || m.AddAuth.Value || m.ModifyAuth.Value || m.DeleteAuth.Value || m.ExportAuth.Value || m.PrintAuth.Value))
                .WithMessage("權限需勾選一項");

            RuleFor(m => m.PrintAuth)
                .Must((m, auth) => (m.QueryAuth.HasValue && m.AddAuth.HasValue && m.ModifyAuth.HasValue && m.DeleteAuth.HasValue && m.ExportAuth.HasValue && m.PrintAuth.HasValue) &&
                (m.QueryAuth.Value || m.AddAuth.Value || m.ModifyAuth.Value || m.DeleteAuth.Value || m.ExportAuth.Value || m.PrintAuth.Value))
                .WithMessage("權限需勾選一項");

        }
    }
}
