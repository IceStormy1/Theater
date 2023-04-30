using System;

namespace Theater.Contracts.Theater.WorkersPosition
{
    public sealed class WorkersPositionModel : WorkersPositionParameters
    {
        /// <summary>
        /// Идентификатор должности
        /// </summary>
        public Guid Id { get; set; }
    }
}
