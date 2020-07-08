using FluentValidation;
using Goblin.Identity.Share.Models.UserModels;

namespace Goblin.Identity.Core.Validators.UserValidators
{
    public class ResetPasswordModelValidator : AbstractValidator<GoblinIdentityResetPasswordModel>
    {
        public ResetPasswordModelValidator()
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