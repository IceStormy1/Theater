using System.Collections.Generic;
using Theater.Common;

namespace Theater.Entities.Theater
{
    public class WorkersPositionEntity
    {
        /// <summary>
        /// Идентификатор должности
        /// </summary>
        public ushort Id { get; set; }

        /// <summary>
        /// Наименование должности
        /// </summary>
        public string PositionName { get; set; }

        /// <summary>
        /// Тип должности
        /// </summary>
        public PositionType PositionType { get; set; }

        public List<TheaterWorkerEntity> TheaterWorker { get; set; }
    }
}
