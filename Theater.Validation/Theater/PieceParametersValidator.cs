using System;
using FluentValidation;
using Theater.Contracts.Theater;

namespace Theater.Validation.Theater;

public sealed class PieceParametersValidator : AbstractValidator<PieceParameters>
{
    public PieceParametersValidator()
    {
        RuleFor(piece => piece.GenreId)
            .NotEqual(Guid.Empty)
            .WithMessage("Указан некорректный жанр");

        RuleFor(piece => piece.PieceName)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(128)
            .WithName("Наименование пьесы");

        RuleFor(worker => worker.Description)
            .Description("Описание пьесы");

        RuleFor(worker => worker.ShortDescription)
            .Description("Краткое описание пьесы");
    }
}