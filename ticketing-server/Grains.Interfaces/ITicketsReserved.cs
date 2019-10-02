using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Orleans;
using Ticketing.Models;

namespace Grains.Interfaces
{

    public  interface ITicketsReserved : IGrainWithStringKey
  {
      Task SetTicket(TicketBooking ticketBooking);
      Task<List<TicketStatus>> GetAllTickets();
      Task<bool> GetTicketState(string ticketId);
      Task<List<string>> GetUnreservedTickets();
      Task<int> GetTicketCount();
      Task InitialiseTickets(int seatsToAllocate);




  }
}
