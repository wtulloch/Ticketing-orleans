using System.Collections.Generic;

namespace Ticketing.Models
{
    public class ShowData
    {
        public ShowData()
        {
            
        }
        public string ShowId { get; set; }
        public string ShowName { get; set; }
        public  string Date { get; set; }
        public int SeatAllocation { get; set; }
        public List<TicketStatus> Tickets { get; set; }
    }
}
