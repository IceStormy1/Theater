using System.Collections.Generic;
using System.Threading.Tasks;
using Theater.Contracts.Theater;

namespace Theater.Abstractions.TheaterWorker;

public interface ITheaterWorkerService : ICrudService<TheaterWorkerParameters>
{
    /// <summary>
    /// Получить количество работников театра по каждому из типов должности 
    /// </summary>
    Task<TotalWorkersModel> GetTotalWorkers();

    /// <summary>
    /// Получить краткую информацию о работниках театра по типу должности 
    /// </summary>
    Task<IReadOnlyCollection<TheaterWorkerShortInformationModel>> GetShortInformationWorkersByPositionType(int positionType);
}