using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Abstractions.Piece.Models;
using Theater.Entities.Theater;

namespace Theater.Abstractions.TheaterWorker
{
    public interface ITheaterWorkerRepository : ICrudRepository<TheaterWorkerEntity>
    {
        /// <summary>
        /// Получить количество работников театра по каждому из типов должности 
        /// </summary>
        Task<IReadOnlyDictionary<int, int>> GetTotalWorkers();

        /// <summary>
        /// Получить краткую информацию о работниках театра по типу должности 
        /// </summary>
        Task<IReadOnlyCollection<TheaterWorkerShortInformationDto>> GetShortInformationWorkersByPositionType(int positionType);
    }
}