using System.Collections.Generic;
using System.Threading.Tasks;
using Grains.Interfaces;
using Orleans;
using Orleans.Concurrency;

namespace Grains
{
    /// <summary>
    /// This grain is just for generating a set of fake ticket ids for a given show
    /// </summary>
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