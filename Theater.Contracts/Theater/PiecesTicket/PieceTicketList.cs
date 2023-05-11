using System.Collections.Generic;
using System.Linq;

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
        public ushort Cols => Items.Count == default ? default : Items.Max(x => x.TicketPlace);

        /// <summary>
        /// Количество рядов
        /// </summary>
        public ushort Rows => Items.Count == default ? default : Items.Max(x => x.TicketRow);
    }
}
