using FluentValidation;
using System.Text.RegularExpressions;
using Theater.Contracts;
using Theater.Contracts.UserAccount;

namespace Theater.Validation.Authorization;

public sealed class UserParametersValidator : AbstractValidator<UserParameters>
{
    /// <param name="userValidator"><see cref="UserValidator"/></param>
    /// <param name="userBaseValidator"><see cref="UserBase"/></param>
    public UserParametersValidator(
        IValidator<IUser> userValidator,
        IValidator<UserBase> userBaseValidator)
    {
        Include(userValidator);
        Include(userBaseValidator);

        RuleFor(user => user.Email)
            .NotEmpty()
            .EmailAddress()
            .MaximumLength(256)
            .WithName("Электронная почта");

        RuleFor(user => user.UserName)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(128)
            .WithName("Никнейм");

        RuleFor(user => user.Phone)
            .Must(user=> !string.IsNullOrWhiteSpace(user) && Regex.IsMatch(user, "^\\d{11}$"))
            .WithMessage("Указан некорректный номер телефона");
    }
}