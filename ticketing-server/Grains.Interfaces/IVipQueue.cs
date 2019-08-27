using System.Threading.Tasks;
using Orleans;

namespace Grains.Interfaces
{
    public interface IVipQueue: IGrainWithStringKey
    {
        Task QueueTicket(string ticketId);

        Task<int> RemainingVipTickets();

        Task<string> GetVipTicket();
    }
}