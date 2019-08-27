using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Orleans;

namespace Grains.Interfaces
{

    public  interface ITicketsReserved : IGrainWithStringKey
  {
      Task SetTicket(string ticketId, bool state);
      Task<List<(string, bool)>> GetAllTickets();
      Task<bool> GetTicketState(string ticketId);
      Task<List<string>> GetUnreservedTickets();
      Task<int> GetTicketCount();




  }
}
