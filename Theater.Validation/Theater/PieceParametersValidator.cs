using FluentValidation;
using System;
using Theater.Contracts.FileStorage;
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
        
        RuleFor(piece => piece.MainPhoto)
            .NotEmpty()
            .WithName("Основное изображение пьесы");

        When(x => x.MainPhoto != null, () =>
        {
            RuleFor(x => x.MainPhoto).ChildRules(ValidatePhoto);
        });

        When(x => x.AdditionalPhotos != null, () =>
        {
            RuleForEach(x => x.AdditionalPhotos).NotEmpty().ChildRules(ValidatePhoto);
        });

        RuleFor(worker => worker.Description)
            .Description("Описание пьесы");

        RuleFor(worker => worker.ShortDescription)
            .Description("Краткое описание пьесы");
    }

    private void ValidatePhoto(InlineValidator<StorageFileListItem> context)
    {
        context.RuleFor(x => x.Id).NotEqual(Guid.Empty);
        context.RuleFor(x => x.FileName).NotEmpty();
        context.RuleFor(x => x.UploadAt).NotEmpty();
    }
}