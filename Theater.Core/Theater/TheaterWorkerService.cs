using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Theater.Abstractions;
using Theater.Abstractions.TheaterWorker;
using Theater.Contracts.Theater;
using Theater.Entities.Theater;

namespace Theater.Core.Theater
{
    public sealed class TheaterWorkerService : ServiceBase<TheaterWorkerParameters, TheaterWorkerEntity>, ITheaterWorkerService
    {
        private readonly ITheaterWorkerRepository _theaterWorkerRepository;

        public TheaterWorkerService(
            IMapper mapper,
            IDocumentValidator<TheaterWorkerParameters> documentValidator,
            ITheaterWorkerRepository repository) 
            : base(mapper, repository, documentValidator)
        {
            _theaterWorkerRepository = repository;
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
    }
}