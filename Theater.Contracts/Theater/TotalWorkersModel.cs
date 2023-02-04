using System.Collections.Generic;
using System.Linq;

namespace Theater.Contracts.Theater
{
    public sealed class TotalWorkersModel
    {
        /// <summary>
        /// Количество работников по каждому типу должности
        /// </summary>
        /// <remarks>
        /// Key - тип должности; Value - количество работников
        /// </remarks>
        public IReadOnlyDictionary<int, int> TotalWorkersByPositionType { get; set; } = new Dictionary<int, int>();

        /// <summary>
        /// Суммарное количество работников театра
        /// </summary>
        public int TotalWorkersCount => TotalWorkersByPositionType.Values.Sum(x => x);
    }
}