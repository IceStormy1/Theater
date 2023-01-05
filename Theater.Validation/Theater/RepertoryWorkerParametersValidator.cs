using FluentValidation;
using Theater.Contracts.Theater;

namespace Theater.Validation.Theater
{
    public class RepertoryWorkerParametersValidator : AbstractValidator<RepertoryWorkerParameters>
    {
        public RepertoryWorkerParametersValidator()
        {
            RuleFor(worker => worker.RepertoryId)
                .ValidateGuid("Некорректный идентификатор репертуара");

            RuleFor(worker => worker.TheaterWorkerId)
                .ValidateGuid("Некорректный работника театра");
        }
    }
}