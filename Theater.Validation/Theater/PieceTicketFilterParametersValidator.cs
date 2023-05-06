using FluentValidation;
using System;
using Theater.Contracts.Filters;

namespace Theater.Validation.Theater
{
    public sealed class PieceTicketFilterParametersValidator : AbstractValidator<PieceTicketFilterParameters>
    {
        public PieceTicketFilterParametersValidator()
        {
            RuleFor(x => x.UserId)
                .NotEqual(Guid.Empty)
                .WithMessage("Идентификатор пользователя должен быть заполнен");
        }
    }
}
