namespace Theater.Contracts.Theater.WorkersPosition
{
    public sealed class PositionTypeItem
    {
        /// <summary>
        /// Идентификатор типа должности
        /// </summary>
        public ushort Id { get; set; }

        /// <summary>
        /// Наименование типа должности
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Наименование типа должности
        /// </summary>
        public string DisplayName { get; set; }
    }
}
