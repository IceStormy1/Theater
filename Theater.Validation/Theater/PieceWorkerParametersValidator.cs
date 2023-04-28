using FluentValidation;
using Theater.Contracts.Theater;

namespace Theater.Validation.Theater;

public sealed class PieceWorkerParametersValidator : AbstractValidator<PieceWorkerParameters>
{
    public PieceWorkerParametersValidator()
    {
        RuleFor(worker => worker.PieceId)
            .ValidateGuid("Некорректный идентификатор пьесы");

        RuleFor(worker => worker.TheaterWorkerId)
            .ValidateGuid("Некорректный работника театра");
    }
}