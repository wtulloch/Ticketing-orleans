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
    /// <summary>
    /// Implementation of the IShowInfo interface.
    /// Provides information regarding the current shows available
    /// </summary>
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

        /// <summary>
        /// returns a list of all the current shows in the system
        /// </summary>
        /// <returns></returns>
        public Task<List<Ticketing.Models.ShowInformation>> GetShows()
        {
            return Task.FromResult(State.ShowInformation.ToList());
        }

        /// <summary>
        /// Gets the information regarding a specific show for a specific date
        /// including all the tickets and whether they have been booked or not.
        /// </summary>
        /// <param name="showId">The unique id for the show</param>
        /// <param name="date">the date of the show</param>
        /// <returns></returns>
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