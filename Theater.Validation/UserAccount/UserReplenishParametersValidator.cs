using FluentValidation;
using Theater.Contracts.UserAccount;

namespace Theater.Validation.UserAccount;

public sealed class UserReplenishParametersValidator : AbstractValidator<UserReplenishParameters>
{
    private const decimal MinimalAmountValue = 0;
    private const decimal MaximumAmountValue = 1_000_000;

    public UserReplenishParametersValidator()
    {
        RuleFor(x => x.ReplenishmentAmount)
            .GreaterThanOrEqualTo(MinimalAmountValue)
            .LessThanOrEqualTo(MaximumAmountValue);
    }
}