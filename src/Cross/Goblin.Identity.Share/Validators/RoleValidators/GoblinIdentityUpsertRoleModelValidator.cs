using FluentValidation;
using Goblin.Identity.Share.Models.RoleModels;

namespace Goblin.Identity.Share.Validators.RoleValidators
{
    public class GoblinIdentityUpsertRoleModelValidator : AbstractValidator<GoblinIdentityUpsertRoleModel>
    {
        public GoblinIdentityUpsertRoleModelValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(500);
        }
    }
}