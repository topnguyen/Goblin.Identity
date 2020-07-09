using FluentValidation;
using Goblin.Identity.Share.Models.UserModels;

namespace Goblin.Identity.Share.Validators.UserValidators
{
    public class GoblinIdentityConfirmEmailModelValidator : AbstractValidator<GoblinIdentityConfirmEmailModel>
    {
        public GoblinIdentityConfirmEmailModelValidator()
        {
            RuleFor(x => x.EmailConfirmToken)
                .NotEmpty();
        }
    }
}