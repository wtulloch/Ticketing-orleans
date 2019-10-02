using System.ComponentModel;
using Orleans.Concurrency;

namespace Ticketing.Models
{
   [Immutable]
    public class TicketBooking
    {
        public string ShowId { get; }
        public string TicketId { get; }

        public TicketBooking(string showId, string ticketId)
        {
            TicketId = ticketId;
            ShowId = showId;
        }
    }
}