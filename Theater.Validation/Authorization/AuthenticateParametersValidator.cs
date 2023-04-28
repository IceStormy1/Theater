using FluentValidation;
using Theater.Contracts.Authorization;

namespace Theater.Validation.Authorization;

public sealed class AuthenticateParametersValidator : AbstractValidator<AuthenticateParameters>
{
    /// <param name="userBaseValidator"><see cref="UserBase"/></param>
    public AuthenticateParametersValidator(IValidator<UserBase> userBaseValidator)
    {
        Include(userBaseValidator);
    }
}