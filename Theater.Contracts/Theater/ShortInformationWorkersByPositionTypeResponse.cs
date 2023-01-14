using System.Collections.Generic;

namespace Theater.Contracts.Theater
{
    public class ShortInformationWorkersByPositionTypeResponse
    {
        /// <inheritdoc cref="TheaterWorkerShortInformationModel"/>
        public IReadOnlyCollection<TheaterWorkerShortInformationModel> TheaterWorkersShortInformation { get; set; }
    }
}