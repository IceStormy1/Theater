using System;

namespace Theater.Contracts.Theater
{
    public class RepertoryParameters
    {
        /// <summary>
        /// Дата начала репертуара
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Дата окончания репертуара
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Время
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Идентификатор пьесы
        /// </summary>
        public Guid PieceId { get; set; }
    }
}