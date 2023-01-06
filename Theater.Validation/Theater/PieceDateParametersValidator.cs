using FluentValidation;
using Theater.Contracts.Theater;

namespace Theater.Validation.Theater
{
    public class PieceDateParametersValidator : AbstractValidator<PieceDateParameters>
    {
        public PieceDateParametersValidator()
        {
            RuleFor(repertory => repertory.PieceId)
                .ValidateGuid("Некорректный идентификатор пьесы");

            RuleFor(repertory => repertory.Date)
                .NotEmpty()
                .WithName("Дата начала");
        }
    }
}