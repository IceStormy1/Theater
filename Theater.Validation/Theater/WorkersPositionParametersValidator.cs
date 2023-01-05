using FluentValidation;
using Theater.Contracts.Theater;

namespace Theater.Validation.Theater
{
    public class WorkersPositionParametersValidator : AbstractValidator<WorkersPositionParameters>
    {
        public WorkersPositionParametersValidator()
        {
            RuleFor(position => position.PositionName)
                .NotEmpty()
                .MaximumLength(128)
                .MinimumLength(5)
                .WithName("Должность");
        }
    }
}