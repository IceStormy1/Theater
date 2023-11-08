using FluentValidation;
using Theater.Common.Constants;
using Theater.Contracts.Rooms;

namespace Theater.Validation.Rooms;

public sealed class NewMessageModelValidator : AbstractValidator<NewMessageModel>
{
    public NewMessageModelValidator()
    {
        RuleFor(x => x.Text)
            .NotEmpty()
            .WithMessage("Сообщение не может быть пустым")
            .MaximumLength(MessageConstants.MessageMaxLength)
            .WithMessage("Максимальная длина сообщения должна быть {MessageConstants.MessageMaxLength}");
    }
}