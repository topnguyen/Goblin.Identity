using FluentValidation;
using Goblin.Identity.Share.Models;

namespace Goblin.Identity.Core.Validators
{
    public class CreateSampleModelValidator : AbstractValidator<GoblinIdentityCreateSampleModel>
    {
        public CreateSampleModelValidator()
        {
            RuleFor(x => x.SampleData)
                .NotEmpty()
                .WithMessage("Please Input Sample Data");
        }
    }
}