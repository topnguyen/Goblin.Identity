using FluentValidation;
using Goblin.Identity.Share.Models;

namespace Goblin.Identity.Core.Validators
{
    public class GenerateAccessTokenModelValidator : AbstractValidator<GoblinIdentityGenerateAccessTokenModel>
    {
        public GenerateAccessTokenModelValidator()
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