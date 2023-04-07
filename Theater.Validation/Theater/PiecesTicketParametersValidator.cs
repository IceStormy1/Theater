using FluentValidation;
using Theater.Contracts.Theater;

namespace Theater.Validation.Theater
{
    public sealed class PiecesTicketParametersValidator : AbstractValidator<PiecesTicketParameters>
    {
        private const ushort MinimumValue = 1;

        public PiecesTicketParametersValidator()
        {
            RuleFor(ticket => ticket.TicketPlace)
                .GreaterThanOrEqualTo(MinimumValue)
                .WithName("Место");

            RuleFor(ticket => ticket.TicketRow)
                .GreaterThanOrEqualTo(MinimumValue)
                .WithName("Ряд");
        }
    }
}