using System;

namespace Theater.Contracts.Theater
{
    public class RepertoryWorkerParameters
    {
        /// <summary>
        /// Идентификатор работника театра 
        /// </summary>
        public Guid TheaterWorkerId { get; set; }

        /// <summary>
        /// Идентификатор репертуара
        /// </summary>
        public Guid RepertoryId { get; set; }
    }
}
