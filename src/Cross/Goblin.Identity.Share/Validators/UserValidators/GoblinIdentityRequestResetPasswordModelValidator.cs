using FluentValidation;
using Goblin.Identity.Share.Models.UserModels;

namespace Goblin.Identity.Share.Validators.UserValidators
{
    public class GoblinIdentityRequestResetPasswordModelValidator : AbstractValidator<GoblinIdentityRequestResetPasswordModel>
    {
        public GoblinIdentityRequestResetPasswordModelValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .MaximumLength(200)
                .When(x => !string.IsNullOrWhiteSpace(x.Email));
        }
    }
}