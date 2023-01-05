﻿using FluentValidation;
using Theater.Contracts.Theater;

namespace Theater.Validation.Theater
{
    public class PieceParametersValidator : AbstractValidator<PieceParameters>
    {
        public PieceParametersValidator()
        {
            RuleFor(piece => piece.GenreId)
                .GreaterThan((ushort)default)
                .WithMessage("Указан некорректный жанр");

            RuleFor(piece => piece.PieceName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(128)
                .WithName("Наименование пьесы");

            When(worker => !string.IsNullOrWhiteSpace(worker.Description), () =>
            {
                RuleFor(worker => worker.Description)
                    .Description("Описание пьесы");
            });
        }
    }
}
