using System.Collections.Generic;

namespace Theater.Contracts.Theater.PiecesTicket
{
    public sealed class PieceTicketList
    {
        public PieceTicketList(IReadOnlyCollection<PiecesTicketModel> items)
        {
            Items = items;
        }

        /// <summary>
        /// Билеты пьесы
        /// </summary>
        public IReadOnlyCollection<PiecesTicketModel> Items { get; set; }

        /// <summary>
        /// Количество мест в ряду
        /// </summary>
        public ushort Cols => 12;

        /// <summary>
        /// Количество рядов
        /// </summary>
        public ushort Rows => 12;
    }
}
