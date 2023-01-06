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

            RuleFor(position => position.PositionType)
                .IsInEnum()
                .WithMessage("Некорректный тип должности");
        }
    }
}