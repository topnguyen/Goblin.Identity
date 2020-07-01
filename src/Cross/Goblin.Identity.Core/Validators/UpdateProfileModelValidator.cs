using System;
using FluentValidation;
using Goblin.Identity.Share.Models;

namespace Goblin.Identity.Core.Validators
{
    public class UpdateProfileModelValidator : AbstractValidator<GoblinIdentityUpdateProfileModel>
    {
        public UpdateProfileModelValidator()
        {
            RuleFor(x => x.AvatarUrl)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _) && uri.Length < 1000)
                .When(x => !string.IsNullOrWhiteSpace(x.AvatarUrl));
            
            RuleFor(x => x.FullName)
                .NotEmpty()
                .MaximumLength(200);
            
            RuleFor(x => x.Bio)
                .MaximumLength(500);

            RuleFor(x => x.GithubId)
                .MaximumLength(200);
            
            RuleFor(x => x.SkypeId)
                .MaximumLength(200);
            
            RuleFor(x => x.FacebookId)
                .MaximumLength(200);
            
            RuleFor(x => x.WebsiteUrl)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _) && uri.Length < 1000)
                .When(x => !string.IsNullOrWhiteSpace(x.AvatarUrl));

            RuleFor(x => x.CompanyName)
                .MaximumLength(500);
 
            RuleFor(x => x.CompanyUrl)
                .Must(uri => Uri.TryCreate(uri, UriKind.Absolute, out _) && uri.Length < 1000)
                .When(x => !string.IsNullOrWhiteSpace(x.AvatarUrl));
        }
    }
}