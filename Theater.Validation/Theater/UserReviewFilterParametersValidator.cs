using FluentValidation;
using Theater.Contracts.Filters;

namespace Theater.Validation.Theater;

public sealed class UserReviewFilterParametersValidator : AbstractValidator<UserReviewFilterParameters>
{
    public UserReviewFilterParametersValidator()
    {
        RuleFor(x => x.PieceId)
            .NotEmpty()
            .When(x => !x.UserId.HasValue)
            .WithMessage("Идентификатор пьесы должен быть заполнен, если не заполнен идентификатор пользователя");
    }
}