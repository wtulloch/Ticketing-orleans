
using System.Threading.Tasks;
using Orleans;
using Ticketing.Models;

namespace Grains.Interfaces
{
    public interface IMessageBatch : IGrainWithGuidKey
    {
        Task TicketNotification(ShowTicketLogMessage message);

    }
}