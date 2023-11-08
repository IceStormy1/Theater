using FluentValidation;
using Theater.Common.Constants;
using Theater.Common.Enums;
using Theater.Contracts.Rooms;

namespace Theater.Validation.Rooms;

public sealed class RoomParametersValidator : AbstractValidator<RoomParameters>
{
    public RoomParametersValidator()
    {
        When(x => x.Type == RoomType.Individual, () =>
        {
            RuleFor(x => x.Title)
                .Empty()
                .WithMessage("Название должно быть пустым при создании / обновлении чата");
        }).Otherwise(() =>
        {
            RuleFor(x => x.Title)
                .NotEmpty()
                .WithMessage("Название чата должно быть заполнено в групповом чате")
                .MaximumLength(RoomConstants.TitleMaxLength)
                .WithMessage($"Название чата не должно превышать {RoomConstants.TitleMaxLength}");
        });
    }
}