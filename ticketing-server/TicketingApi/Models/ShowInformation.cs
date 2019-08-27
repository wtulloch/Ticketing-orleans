using System.Collections.Generic;

namespace TicketingApi.Models
{
    public class ShowInformation
    {
        public string ShowId { get; set; }
        public List<TicketInfo> Tickets { get; set; } = new List<TicketInfo>();
        public List<string> UnsoldTickets { get; set; } = new List<string>();
        
    }
}