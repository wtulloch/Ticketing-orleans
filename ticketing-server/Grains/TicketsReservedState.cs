using System.Collections.Generic;

namespace Grains
{
    public class TicketsReservedState
    {
        public Dictionary<string, bool> ReservedTickets { get; set; } = new Dictionary<string, bool>();
    }
}