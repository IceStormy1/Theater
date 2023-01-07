using FluentValidation;
using Theater.Contracts.Theater;

namespace Theater.Validation.Theater
{
    public class PieceDateParametersValidator : AbstractValidator<PieceDateParameters>
    {
        public PieceDateParametersValidator()
        {
            RuleFor(repertory => repertory.Date)
                .NotEmpty()
                .WithName("Дата начала");
        }
    }
}