using System.Linq;
using FluentValidation;
using Theater.Abstractions.Piece.Constants;
using Theater.Contracts.Theater;

namespace Theater.Validation.Theater
{
    public sealed class PieceTicketCreateParametersValidator : AbstractValidator<PieceTicketCreateParameters>
    {
        public PieceTicketCreateParametersValidator(IValidator<PiecesTicketParameters> ticketValidator)
        {
            RuleFor(ticket => ticket.PieceDateId)
                .ValidateGuid("Некорректный идентификатор репертуара");

            RuleFor(x => x.PiecesTickets)
                .NotEmpty()
                .WithMessage("Билеты должны быть заполнены")
                .Must(x =>
                {
                    var hasDuplicates = x.GroupBy(c => new { c.TicketRow, c.TicketPlace })
                        .Select(c => c.Count())
                        .Any(c => c > 1);

                    return !hasDuplicates;
                })
                .WithMessage("Билеты пьесы не должны повторятся")
                .Must(x => x.Count >= PieceConstants.NumberOfSeatsInHall)
                .WithMessage(x => $"Необходимо заполнить все билеты. Вы ввели {x.PiecesTickets.Count} из {PieceConstants.NumberOfSeatsInHall}");

            RuleForEach(x => x.PiecesTickets)
                .SetValidator(ticketValidator);
        }
    }
}
