using FluentValidation;
using Goblin.Identity.Share.Models.UserModels;

namespace Goblin.Identity.Share.Validators.UserValidators
{
    public class GoblinIdentityResetPasswordModelValidator : AbstractValidator<GoblinIdentityResetPasswordModel>
    {
        public GoblinIdentityResetPasswordModelValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .MaximumLength(200);

            RuleFor(x => x.SetPasswordToken)
                .NotEmpty();

            RuleFor(x => x.NewPassword)
                .MinimumLength(4)
                .MaximumLength(200);
        }
    }
}