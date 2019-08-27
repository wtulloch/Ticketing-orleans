using System.Linq;
using System.Threading.Tasks;
using Grains.Interfaces;
using Orleans;
using Orleans.Providers;

namespace Grains
{
    [StorageProvider(ProviderName = "store1")]
    public class VipTickets : Grain<VipTicketState>,  IVipQueue
    {
        public async Task QueueTicket(string ticketId)
        {
            State.VipTickets.Enqueue(ticketId);

            await WriteStateAsync();
        }

        public Task<int> RemainingVipTickets()
        {
            return Task.FromResult(State.VipTickets.Count);
        }

        public Task<string> GetVipTicket()
        {
            return Task.FromResult(State.VipTickets.Any() ? State.VipTickets.Dequeue() : string.Empty);
        }
    }
}