using System;
using System.Collections.Generic;

namespace Theater.Contracts.Theater
{
    public class PieceShortInformationModel
    {
        /// <summary>
        /// Идентификатор пьесы
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Наименование пьесы
        /// </summary>
        public string PieceName { get; set; }

        /// <summary>
        /// Жанр пьесы
        /// </summary>
        public string PieceGenre { get; set; }

        /// <summary>
        /// Работники, которые принимали участие в пьесе
        /// </summary>
        public IReadOnlyCollection<TheaterWorkerShortInformationModel> WorkerShortInformation { get; set; }
    }
}
