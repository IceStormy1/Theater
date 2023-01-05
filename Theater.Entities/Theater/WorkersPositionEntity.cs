using System.Collections.Generic;

namespace Theater.Entities.Theater
{
    public class WorkersPositionEntity
    {
        /// <summary>
        /// Идентификатор роли
        /// </summary>
        public ushort Id { get; set; }

        /// <summary>
        /// Наименование роли
        /// </summary>
        public string PositionName { get; set; }

        public List<TheaterWorkerEntity> TheaterWorker { get; set; }
    }
}
