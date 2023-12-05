using FluentValidation;
using System.Text.RegularExpressions;
using Theater.Contracts.UserAccount;

namespace Theater.Validation.Authorization;

public sealed class UserParametersValidator : AbstractValidator<UserParameters>
{
    public UserParametersValidator()
    {
        When(x => string.IsNullOrWhiteSpace(x.Email), () =>
        {
            RuleFor(user => user.Phone)
                .NotEmpty()
                .WithMessage("Указан некорректный номер телефона");
        }).Otherwise((() =>
        {
            RuleFor(user => user.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(256)
                .WithName("Электронная почта");
        }));

        When(x => string.IsNullOrWhiteSpace(x.Phone), () =>
        {
            RuleFor(user => user.Email)
                .NotEmpty()
                .WithName("Электронная почта");
        }).Otherwise((() =>
        {
            RuleFor(user => user.Phone)
                .Must(phone => Regex.IsMatch(phone, "^\\d{11}$"))
                .WithMessage("Указан некорректный номер телефона");
        }));

        RuleFor(user => user.UserName)
            .NotEmpty()
            .MinimumLength(5)
            .MaximumLength(128)
            .WithName("Никнейм");
    }
}