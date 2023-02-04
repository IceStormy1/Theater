using System;

namespace Theater.Contracts.Theater
{
    public sealed class PieceDateModel : PieceDateParameters
    {
        /// <summary>
        /// Идентификатор даты пьесы
        /// </summary>
        public Guid Id { get; set; }
    }
}