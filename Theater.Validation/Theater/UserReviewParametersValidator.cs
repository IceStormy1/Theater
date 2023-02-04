using FluentValidation;
using Theater.Contracts.Theater;

namespace Theater.Validation.Theater
{
    public sealed class UserReviewParametersValidator : AbstractValidator<UserReviewParameters>
    {
        public UserReviewParametersValidator()
        {
            RuleFor(review => review.Description)
                .Description("Текст рецензии");

            RuleFor(review => review.UserId)
                .ValidateGuid("Некорректный идентификатор пользователя");

            RuleFor(review => review.PieceId)
                .ValidateGuid("Некорректный идентификатор пьесы");

            RuleFor(review => review.Title)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(256)
                .WithName("Заголовок рецензии");
        }
    }
}