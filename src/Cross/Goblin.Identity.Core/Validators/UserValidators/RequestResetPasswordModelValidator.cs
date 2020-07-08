using FluentValidation;
using Goblin.Identity.Share.Models.UserModels;

namespace Goblin.Identity.Core.Validators.UserValidators
{
    public class RequestResetPasswordModelValidator : AbstractValidator<GoblinIdentityRequestResetPasswordModel>
    {
        public RequestResetPasswordModelValidator()
        {
            RuleFor(x => x.Email)
                .EmailAddress()
                .MaximumLength(200)
                .When(x => !string.IsNullOrWhiteSpace(x.Email));
        }
    }
}