using FluentValidation;
using Goblin.Identity.Share.Models.RoleModels;

namespace Goblin.Identity.Core.Validators.RoleValidators
{
    public class UpsertRoleModelValidator : AbstractValidator<GoblinIdentityUpsertRoleModel>
    {
        public UpsertRoleModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(500);
        }
    }
}