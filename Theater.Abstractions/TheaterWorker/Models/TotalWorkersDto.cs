using System.Collections.Generic;

namespace Theater.Abstractions.TheaterWorker.Models
{
    public sealed class TotalWorkersDto
    {
        /// <summary>
        /// Количество работников по каждому типу должности
        /// </summary>
        /// <remarks>
        /// Key - тип должности; Value - количество работников
        /// </remarks>
        public IReadOnlyDictionary<string, short> TotalWorkersByPositionType { get; set; } = new Dictionary<string, short>();
    }
}
