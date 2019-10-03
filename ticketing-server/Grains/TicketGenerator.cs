using System.Collections.Generic;
using System.Threading.Tasks;
using Grains.Interfaces;
using Orleans;
using Orleans.Concurrency;

namespace Grains
{
    [StatelessWorker]
    public class TicketGenerator : Grain, ITicketGenerator
    {
        public Task<Dictionary<string, bool>> CreateTickets(string prefix, int seatsToAllocate)
        {
            var tickets = new Dictionary<string,bool>();
            for (var i = 1; i <= seatsToAllocate; i++)
            {
                var seatNo = i.ToString("0000");
               tickets.Add($"{prefix}-{seatNo}", false);
            }

            return Task.FromResult(tickets);
        }
    }
}