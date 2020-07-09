using FluentValidation;
using Goblin.Identity.Share.Models.UserModels;

namespace Goblin.Identity.Share.Validators.UserValidators
{
    public class GoblinIdentityGenerateAccessTokenModelValidator : AbstractValidator<GoblinIdentityGenerateAccessTokenModel>
    {
        public GoblinIdentityGenerateAccessTokenModelValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(200);
            
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(200);          
        }
    }
}