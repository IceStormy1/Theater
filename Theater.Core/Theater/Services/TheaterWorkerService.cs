using AutoMapper;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Abstractions;
using Theater.Abstractions.FileStorage;
using Theater.Abstractions.TheaterWorker;
using Theater.Contracts;
using Theater.Contracts.Theater.TheaterWorker;
using Theater.Entities.Theater;

namespace Theater.Core.Theater.Services;

public sealed class TheaterWorkerService : BaseService<TheaterWorkerParameters, TheaterWorkerEntity>, ITheaterWorkerService
{
    private readonly ITheaterWorkerRepository _theaterWorkerRepository;
    private readonly IFileStorageService _fileStorageService;

    public TheaterWorkerService(
        IMapper mapper,
        IDocumentValidator<TheaterWorkerParameters> documentValidator,
        ITheaterWorkerRepository repository,
        IFileStorageService fileStorageService,
        ILogger<TheaterWorkerService> logger)
        : base(mapper, repository, documentValidator, logger)
    {
        _theaterWorkerRepository = repository;
        _fileStorageService = fileStorageService;
    }

    public async Task<TotalWorkersModel> GetTotalWorkers()
    {
        var totalWorkers = await _theaterWorkerRepository.GetTotalWorkers();

        return new TotalWorkersModel
        {
            TotalWorkersByPositionType = totalWorkers
        };
    }

    public async Task<IReadOnlyCollection<TheaterWorkerShortInformationModel>> GetShortInformationWorkersByPositionType(int positionType)
    {
        var workersShortInformation = await _theaterWorkerRepository.GetShortInformationWorkersByPositionType(positionType);

        return Mapper.Map<IReadOnlyCollection<TheaterWorkerShortInformationModel>>(workersShortInformation);
    }

    public async Task EnrichTheaterWorkerShortInfo(Page<TheaterWorkerShortInformationModel> theaterWorkerShortInformationModels)
    {
        foreach (var pieceShortInformationModel in theaterWorkerShortInformationModels.Items)
            await EnrichMainPicture(pieceShortInformationModel);
    }

    private async Task EnrichMainPicture(TheaterWorkerShortInformationModel shortInformationModel)
    {
        if (shortInformationModel.MainPhoto is null)
            return;

        shortInformationModel.MainPhoto = await _fileStorageService.GetStorageFileInfoById(shortInformationModel.MainPhoto.Id);
    }
}