using FluentValidation;
using Goblin.Identity.Share.Models.UserModels;

namespace Goblin.Identity.Core.Validators.UserValidators
{
    public class UpdateIdentityModelValidator : AbstractValidator<GoblinIdentityUpdateIdentityModel>
    {
        public UpdateIdentityModelValidator()
        {
            RuleFor(x => x.CurrentPassword)
                .NotEmpty()
                .MinimumLength(4)
                .MaximumLength(200);  
          
            RuleFor(x => x.NewEmail)
                .EmailAddress()
                .MaximumLength(200)
                .When(x => !string.IsNullOrWhiteSpace(x.NewEmail));

            RuleFor(x => x.NewUserName)
                .MinimumLength(3)
                .MaximumLength(200)
                .When(x => !string.IsNullOrWhiteSpace(x.NewUserName));
            
            RuleFor(x => x.NewPassword)
                .MinimumLength(4)
                .MaximumLength(200)
                .When(x => !string.IsNullOrWhiteSpace(x.NewPassword));          
        }
    }
}