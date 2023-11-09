using System;
using FluentValidation;
using Theater.Contracts;

namespace Theater.Validation.UserAccount;

public sealed class UserValidator : AbstractValidator<IUser>
{
    private const ushort MinimumUserYears = 14;
    private static readonly DateTime MinimumBirthDate = new(1930, 01, 01);

    public UserValidator()
    {
        RuleFor(user => user.BirthDate)
            .GreaterThanOrEqualTo(MinimumBirthDate)
            .WithMessage("Год рождения должен быть больше 1930 года")
            .LessThanOrEqualTo(DateTime.UtcNow.AddYears(-MinimumUserYears))
            .WithMessage($"На момент регистрации необходимо быть старше {MinimumUserYears} лет");

        RuleFor(user => user.FirstName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(128)
            .WithName("Имя");

        RuleFor(user => user.LastName)
            .NotEmpty()
            .MinimumLength(2)
            .MaximumLength(128)
            .WithName("Фамилия");

        When(user => string.IsNullOrWhiteSpace(user.MiddleName), () =>
        {
            RuleFor(user => user.MiddleName)
                .MinimumLength(2)
                .MaximumLength(128)
                .WithName("Отчество");
        });

        RuleFor(user => user.Gender)
            .IsInEnum()
            .WithMessage("Некорректный пол пользователя");
    }
}