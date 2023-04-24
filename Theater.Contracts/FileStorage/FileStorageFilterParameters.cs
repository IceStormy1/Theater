namespace Theater.Contracts.FileStorage
{
    public class FileStorageFilterParameters : FileStorageParameters
    {
        /// <summary>
        /// Максимальное количество записей, которое должен вернуть запрос.
        /// </summary>
        public int Limit { get; set; }

        /// <summary>
        /// Максимально допустимоео количество дней для хранения файла
        /// </summary>
        public short? NumberOfDaysToRemove { get; set; }
    }
}
