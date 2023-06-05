using AutoMapper;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Theater.Abstractions;
using Theater.Abstractions.Errors;
using Theater.Abstractions.FileStorage;
using Theater.Abstractions.Piece;
using Theater.Abstractions.PieceDates;
using Theater.Common;
using Theater.Contracts;
using Theater.Contracts.FileStorage;
using Theater.Contracts.Theater.Piece;
using Theater.Entities.Theater;

namespace Theater.Core.Theater.Services;

public sealed class PieceService : ServiceBase<PieceParameters, PieceEntity>, IPieceService
{
    private readonly IPieceRepository _pieceRepository;
    private readonly IFileStorageService _fileStorageService;

    public PieceService(
        IMapper mapper,
        IPieceRepository repository,
        IDocumentValidator<PieceParameters> documentValidator,
        IFileStorageService fileStorageService,
        ILogger<PieceService> logger) : base(mapper, repository, documentValidator, logger)
    {
        _pieceRepository = repository;
        _fileStorageService = fileStorageService;
    }

    public async Task<WriteResult<PieceModel>> GetPieceById(Guid pieceId)
    {
        var pieceEntity = await _pieceRepository.GetByEntityId(pieceId);

        if (pieceEntity is null)
            return WriteResult<PieceModel>.FromError(PieceErrors.NotFound.Error);

        var pieceResult = Mapper.Map<PieceModel>(pieceEntity);
        await EnrichPieceModel(pieceResult);

        return WriteResult.FromValue(pieceResult);
    }

    public async Task EnrichPieceShortInformation(Page<PieceShortInformationModel> shortInformationModel)
    {
        foreach (var pieceShortInformationModel in shortInformationModel.Items)
            await EnrichMainPicture(pieceShortInformationModel);
    }

    private async Task EnrichPieceModel(PieceModel pieceModel)
    {
        await EnrichMainPicture(pieceModel);
        await EnrichAdditionalPhotos(pieceModel);
    }

    private async Task EnrichAdditionalPhotos(PieceModel pieceModel)
    {
        if (pieceModel.AdditionalPhotos.Count == default)
            return;

        var additionalPhotos = new List<StorageFileInfo>();

        foreach (var pieceModelAdditionalPhoto in pieceModel.AdditionalPhotos)
        {
            var fileInfo = await _fileStorageService.GetStorageFileInfoById(pieceModelAdditionalPhoto.Id);
            additionalPhotos.Add(fileInfo);
        }

        pieceModel.AdditionalPhotos = additionalPhotos;
    }

    private async Task EnrichMainPicture(PieceShortInformationModel shortInformationModel)
    {
        if (shortInformationModel.MainPicture is null)
            return;

        shortInformationModel.MainPicture = await _fileStorageService.GetStorageFileInfoById(shortInformationModel.MainPicture.Id);
    }
}