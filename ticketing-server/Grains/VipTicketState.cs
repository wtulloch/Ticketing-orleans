using System.Collections.Generic;

namespace Grains
{
    public class VipTicketState
    {
        public Queue<string> VipTickets { get;  } = new Queue<string>();
    }
}