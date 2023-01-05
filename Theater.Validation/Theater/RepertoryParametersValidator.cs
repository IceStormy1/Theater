using System;
using System.Text.RegularExpressions;
using FluentValidation;
using Theater.Contracts.Theater;

namespace Theater.Validation.Theater
{
    public class RepertoryParametersValidator : AbstractValidator<RepertoryParameters>
    {
        public RepertoryParametersValidator()
        {
            RuleFor(repertory => repertory.PieceId)
                .ValidateGuid("Некорректный идентификатор пьесы");

            RuleFor(repertory => repertory.StartDate)
                .NotEmpty()
                .WithName("Дата начала");

            RuleFor(repertory => repertory.EndDate)
                .NotEmpty()
                .WithName("Дата окончания");

            RuleFor(repertory => repertory.Time)
                .NotEmpty()
                .WithName("Время");

            When(x => !string.IsNullOrWhiteSpace(x.Time), () =>
            {
                RuleFor(x => x.Time)
                    .Must(x =>
                        Regex.IsMatch(x, @"^([0-1]?\d|2[0-3])(?::([0-5]?\d))?(?::([0-5]?\d))?$")
                        && TimeSpan.TryParse(x, out _))
                    .WithName("Время"); 
            });
        }
    }
}
