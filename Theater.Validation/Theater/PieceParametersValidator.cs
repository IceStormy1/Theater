using FluentValidation;
using Theater.Abstractions.Piece.Constants;
using Theater.Contracts.Theater;

namespace Theater.Validation.Theater
{
    public sealed class PieceParametersValidator : AbstractValidator<PieceParameters>
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

            RuleFor(worker => worker.Description)
                .Description("Описание пьесы");

            RuleFor(worker => worker.ShortDescription)
                .Description("Краткое описание пьесы");

            When(x => x.PiecesTickets is { Count: > 0 }, () =>
            {
                RuleFor(x => x.PiecesTickets)
                    .Must(x => x.Count == PieceConstants.NumberOfSeatsInHall)
                    .WithMessage(x=> $"Необходимо заполнить все билеты. Вы ввели {x.PiecesTickets.Count} из {PieceConstants.NumberOfSeatsInHall}");
            });
        }
    }
}
