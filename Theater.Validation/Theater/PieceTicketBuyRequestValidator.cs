using FluentValidation;
using Theater.Contracts.Theater.PiecesTicket;

namespace Theater.Validation.Theater;

public sealed class PieceTicketBuyRequestValidator : AbstractValidator<PieceTicketBuyRequest>
{
    public PieceTicketBuyRequestValidator()
    {
        RuleFor(x => x.TicketIds)
            .NotEmpty()
            .WithMessage("Необходимо заполнить хотя бы один билет для покупки/бронирования");
    }
}