using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grains.Interfaces;
using Orleans;
using Orleans.Providers;
using Repositories;
using Ticketing.Models;
using Utils;

namespace Grains
{
    [StorageProvider(ProviderName = TicketingConstants.StorageProviderName)]
    public class TheatreShows : Grain<TheatreShowState> , IShowInfo
    {
        private readonly IShowRepository _repository;

        public TheatreShows(IShowRepository repository)
        {
            _repository = repository;
        }

        public override async Task OnActivateAsync()
        {
            if (State.ShowInformation.Count > 0)
            {
                await base.OnActivateAsync();
                return;
            }
           
            State.ShowInformation = new HashSet<ShowInformation>();
            foreach (var showInfo in _repository.GetShows())
            {
                State.ShowInformation.Add(showInfo);
            }

            await WriteStateAsync();

            await base.OnActivateAsync();
        }

        public Task<List<Ticketing.Models.ShowInformation>> GetShows()
        {
            return Task.FromResult(State.ShowInformation.ToList());
        }

        public async Task<ShowData> GetShow(string showId, string date)
        {
            var showInfo =
                State.ShowInformation.FirstOrDefault(si => si.BaseShowId == showId && si.Dates.Contains(date));
            if (showInfo == null)
            {
                return null;
            }

            var id = $"{showId}:{date.Replace("/", "")}";

            var grain = this.GrainFactory.GetGrain<ITicketsReserved>(id);
            var ticketCount = await grain.GetTicketCount();
            if (ticketCount == 0)
            {
               await grain.InitialiseTickets(showInfo.SeatingAllocation);
            }

            var tickets = await grain.GetAllTickets();
            var showData = new ShowData
            {
                ShowName = showInfo.Name,
                ShowId = id,
                Date = date,
                SeatAllocation = showInfo.SeatingAllocation,
                Tickets = tickets
            };


            return showData;
        }
    }
}