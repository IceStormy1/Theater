using FluentValidation;
using Theater.Contracts.Theater;

namespace Theater.Validation.Theater
{
    public sealed class PiecesGenreParametersValidator : AbstractValidator<PiecesGenreParameters>
    {
        public PiecesGenreParametersValidator()
        {
            RuleFor(genre => genre.GenreName)
                .NotEmpty()
                .MinimumLength(3)
                .MaximumLength(64)
                .WithName("Жанр");
        }
    }
}
