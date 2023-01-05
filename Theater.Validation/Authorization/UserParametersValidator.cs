using FluentValidation;
using System;
using System.Text.RegularExpressions;
using Theater.Contracts.Authorization;

namespace Theater.Validation.Authorization
{
    public class UserParametersValidator : AbstractValidator<UserParameters>
    {
        private const ushort MinimumUserYears = 14;
        private static readonly DateTime MinimumBirthDate = new(1930, 01, 01);

        public UserParametersValidator()
        {
            RuleFor(user => user.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(256)
                .WithName("Электронная почта");

            RuleFor(user => user.BirthDate)
                .GreaterThanOrEqualTo(MinimumBirthDate)
                .WithMessage("Год рождения должен быть больше 1930 года")
                .LessThanOrEqualTo(DateTime.UtcNow.AddYears(-MinimumUserYears))
                .WithMessage($"На момент регистрации необходимо быть старше {MinimumUserYears} лет");

            RuleFor(user => user.UserName)
                .NotEmpty()
                .MinimumLength(5)
                .MaximumLength(128)
                .WithName("Никнейм");

            RuleFor(user => user.Phone)
                .Must(user=> !string.IsNullOrWhiteSpace(user) && Regex.IsMatch(user, "^\\d{11}$"))
                .WithMessage("Указан некорректный номер телефона");

            RuleFor(user=>user.FirstName)
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
}
