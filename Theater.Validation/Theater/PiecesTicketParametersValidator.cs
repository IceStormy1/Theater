using FluentValidation;
using Theater.Contracts.Theater;

namespace Theater.Validation.Theater
{
    public class PiecesTicketParametersValidator : AbstractValidator<PiecesTicketParameters>
    {
        private const ushort MinimumValue = 1;

        public PiecesTicketParametersValidator()
        {
            RuleFor(ticket => ticket.PieceDateId)
                .ValidateGuid("Некорректный идентификатор репертуара");

            RuleFor(ticket => ticket.TicketPlace)
                .GreaterThanOrEqualTo(MinimumValue)
                .WithName("Место");

            RuleFor(ticket => ticket.TicketRow)
                .GreaterThanOrEqualTo(MinimumValue)
                .WithName("Ряд");
        }
    }
}