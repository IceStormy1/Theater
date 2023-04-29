using System;

namespace Theater.Contracts.Theater
{
    public sealed class WorkersPositionModel : WorkersPositionParameters
    {
        /// <summary>
        /// Идентификатор должности
        /// </summary>
        public Guid Id { get; set; }
    }
}
