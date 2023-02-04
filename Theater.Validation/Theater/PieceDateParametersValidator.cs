using FluentValidation;
using Theater.Contracts.Theater;

namespace Theater.Validation.Theater
{
    public sealed class PieceDateParametersValidator : AbstractValidator<PieceDateParameters>
    {
        public PieceDateParametersValidator()
        {
            RuleFor(repertory => repertory.Date)
                .NotEmpty()
                .WithName("Дата начала");
        }
    }
}