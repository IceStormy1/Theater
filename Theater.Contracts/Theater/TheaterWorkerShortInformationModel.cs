using System;

namespace Theater.Contracts.Theater
{
    public class TheaterWorkerShortInformationModel
    {
        /// <summary>
        /// Идентификатор работника театра
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Полное имя работника театра
        /// </summary>
        public string FullName { get; set; }

        /// <summary>
        /// Наименование должности работника театра
        /// </summary>
        public string PositionName { get; set; }

        /// <summary>
        /// Наименование типа должности
        /// </summary>
        public string PositionTypeName { get; set; }
    }
}
