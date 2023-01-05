using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Theater.Entities.Theater
{
    public class TicketPriceEventsEntity
    {
        public Guid PiecesTicketId { get; set; }

        public int Version { get; set; }

        public PiecesTicketEntity Model { get; set; }

        public DateTime Timestamp { get; set; }

        public PiecesTicketEntity PiecesTicket { get; set; }

        public List<PurchasedUserTicketEntity> PurchasedUserTicket { get; set; }
    }
}
