using FluentValidation;
using Theater.Contracts.Rooms;

namespace Theater.Validation.Rooms;

public sealed class InviteUsersModelValidator : AbstractValidator<InviteUsersModel>
{
    public InviteUsersModelValidator()
    {
        RuleFor(x => x.InvitedUsersIds)
            .NotEmpty()
            .WithMessage("Необходимо выбрать хотя бы одного пользователя");
    }
}