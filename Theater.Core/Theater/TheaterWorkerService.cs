using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Theater.Abstractions.TheaterWorker;
using Theater.Common;
using Theater.Contracts.Theater;

namespace Theater.Core.Theater
{
    public sealed class TheaterWorkerService : ServiceBase<ITheaterWorkerRepository>, ITheaterWorkerService
    {
        public TheaterWorkerService(IMapper mapper, ITheaterWorkerRepository repository) 
            : base(mapper, repository)
        {
        }

        public async Task<TotalWorkersModel> GetTotalWorkers()
        {
            var totalWorkers = await Repository.GetTotalWorkers();

            return new TotalWorkersModel
            {
                TotalWorkersByPositionType = totalWorkers
            };
        }

        public async Task<IReadOnlyCollection<TheaterWorkerShortInformationModel>> GetShortInformationWorkersByPositionType(int positionType)
        {
            var workersShortInformation = await Repository.GetShortInformationWorkersByPositionType(positionType);

            return Mapper.Map<IReadOnlyCollection<TheaterWorkerShortInformationModel>>(workersShortInformation);
        }

        public async Task<WriteResult<TheaterWorkerModel>> GetTheaterWorkerById(Guid id)
        {
           var theaterWorker = await Repository.GetTheaterWorkerById(id);

           return Mapper.Map<WriteResult<TheaterWorkerModel>>(theaterWorker);
        }
    }
}