using System;

namespace Theater.Contracts.Theater.WorkersPosition
{
    public sealed class WorkersPositionModel : WorkersPositionParameters
    {
        /// <summary>
        /// Идентификатор должности
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Наименование типа должности
        /// </summary>
        public string PositionTypeName { get; set; }
    }
}
