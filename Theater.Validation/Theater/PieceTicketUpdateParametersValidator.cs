using FluentValidation;
using System.Linq;
using Theater.Contracts.Theater;

namespace Theater.Validation.Theater
{
    public sealed class PieceTicketUpdateParametersValidator : AbstractValidator<PieceTicketUpdateParameters>
    {
        public PieceTicketUpdateParametersValidator(IValidator<PiecesTicketParameters> ticketValidator)
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
                        .Any(c => c > 2);

                    return !hasDuplicates;
                })
                .WithMessage("Билеты пьесы не должны повторятся");

            RuleForEach(x => x.PiecesTickets)
                .SetValidator(ticketValidator);
        }
    }
}
