using System.Threading.Tasks;
using Orleans;
using Orleans.Streams;

namespace Grains.Interfaces
{
    public interface ITicketReservers : IGrainWithStringKey
    {
        Task Add(string ticketId, string userId);
        Task Remove(string oldTicketId);
    }
}