using System.Collections.Generic;
using System.Threading.Tasks;
using Orleans;

namespace Grains.Interfaces
{
    public interface ITicketGenerator : IGrainWithIntegerKey
    {
        Task<Dictionary<string, bool>> CreateTickets(string prefix, int seatsToAllocate);
    }
}