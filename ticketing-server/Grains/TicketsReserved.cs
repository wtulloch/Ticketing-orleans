using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grains.Interfaces;
using Orleans;
using Orleans.Providers;
using Ticketing.Models;
using Utils;

namespace Grains
{
    [StorageProvider(ProviderName = TicketingConstants.StorageProviderName)]
    public class TicketsReserved : Grain<TicketsReservedState>, ITicketsReserved
    {

        private IMessageBatch _messageBatchGrain;

        public override Task OnActivateAsync()
        {
            _messageBatchGrain = GrainFactory.GetGrain<IMessageBatch>(Guid.Empty);
          
            return base.OnActivateAsync();
        }

        public async Task SetTicket(TicketBooking ticketBooking)
        {
            if (State.ReservedTickets.ContainsKey(ticketBooking.TicketId))
            {
                State.ReservedTickets[ticketBooking.TicketId] = true;
                var message = new ShowTicketLogMessage(ticketBooking.ShowId, "", ticketBooking.TicketId);
                await _messageBatchGrain.TicketNotification(message);
            }

            await WriteStateAsync();

            return;
        }

        public Task<List<TicketStatus>> GetAllTickets()
        {
            var allTickets = State.ReservedTickets.Select(kv => new TicketStatus{TicketId = kv.Key,Sold = kv.Value}).ToList();

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

        public async Task InitialiseTickets(int seatsToAllocate)
        {
            var primaryKey = this.GetPrimaryKeyString();
            var grain = GrainFactory.GetGrain<ITicketGenerator>(0);
            State.ReservedTickets = await grain.CreateTickets(primaryKey, seatsToAllocate);

          await base.WriteStateAsync();
        }
    }
}