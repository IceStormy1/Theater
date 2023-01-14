using System.Collections.Generic;

namespace Theater.Contracts.Theater
{
    public class PieceShortInformationResponse
    {
        /// <summary>
        /// Краткая информация о пьесах театра
        /// </summary>
        public IReadOnlyCollection<PieceShortInformationModel> PiecesShortInformation { get; set; }
    }
}