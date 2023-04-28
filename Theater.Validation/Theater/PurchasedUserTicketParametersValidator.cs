using FluentValidation;
using System;
using Theater.Contracts.Theater;

namespace Theater.Validation.Theater;

public sealed class PurchasedUserTicketParametersValidator : AbstractValidator<PurchasedUserTicketParameters>
{
    public PurchasedUserTicketParametersValidator()
    {
        RuleFor(repertory => repertory.UserId)
            .ValidateGuid("Некорректный идентификатор пользователя");

        RuleFor(repertory => repertory.DateOfPurchase.Date)
            .Equal(DateTime.UtcNow.Date);

        RuleFor(repertory => repertory.TicketPriceEventsId)
            .ValidateGuid("Некорректный идентификатор билета");

        RuleFor(repertory => repertory.TicketPriceEventsVersion)
            .GreaterThanOrEqualTo(1)
            .WithMessage("Некорректная версия билета");
    }
}