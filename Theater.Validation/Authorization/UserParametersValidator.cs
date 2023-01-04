using FluentValidation;
using Theater.Contracts.Authorization;

namespace Theater.Validation.Authorization
{
    public class UserParametersValidator : AbstractValidator<UserParameters>
    {
        public UserParametersValidator()
        {
            RuleFor(user => user.Email)
                .NotEmpty()
                .EmailAddress()
                .MaximumLength(70);
        }
    }
}
