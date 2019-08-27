using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grains.Interfaces;
using Orleans;
using Orleans.Providers;

namespace Grains
{
    [StorageProvider(ProviderName = "store1")]
    public class TicketsReserved : Grain<TicketsReservedState>, ITicketsReserved
    {

        public Task SetTicket(string ticketId, bool state)
        {
            if (State.ReservedTickets.ContainsKey(ticketId))
            {
                State.ReservedTickets[ticketId] = state;
            }
            else
            {
                State.ReservedTickets.Add(ticketId, state);
            }


            return base.WriteStateAsync();
        }

        public Task<List<(string, bool)>> GetAllTickets()
        {
            var allTickets = State.ReservedTickets.Select(kv => (kv.Key, kv.Value)).ToList();

            return Task.FromResult(allTickets);
        }

        public Task<bool> GetTicketState(string ticketId)
        {
            return Task.FromResult(State.ReservedTickets[ticketId]);
        }

        public Task<List<string>> GetUnreservedTickets()
        {
            var unreserved = State.ReservedTickets.Where(kv => kv.Value == false)
                .Select(kv => kv.Key).ToList();

            return Task.FromResult(unreserved);
        }

        public Task<int> GetTicketCount()
        {
            return Task.FromResult(State.ReservedTickets.Count);
        }
    }
}