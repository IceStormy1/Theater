using FluentValidation;
using Theater.Contracts.Authorization;

namespace Theater.Validation.Authorization
{
    public sealed class UserBaseValidator : AbstractValidator<UserBase>
    {
        public UserBaseValidator()
        {
            RuleFor(user => user.UserName)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(128)
                .WithName("Никнейм");

            RuleFor(user => user.Password)
                .NotEmpty()
                .MaximumLength(100);
        }
    }
}
