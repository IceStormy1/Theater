using FluentValidation;
using Theater.Contracts.Theater;

namespace Theater.Validation.Theater
{
    public class BookedTicketParametersValidator : AbstractValidator<BookedTicketParameters>
    {
        public BookedTicketParametersValidator()
        {
            RuleFor(ticket=>ticket.UserId)
                .ValidateGuid("Некорректный идентификатор пользователя");

            RuleFor(ticket => ticket.PiecesTicketId)
                .ValidateGuid("Некорректный идентификатор билета");

            RuleFor(ticket => ticket.Timestamp)
                .NotEmpty();
        }
    }
}