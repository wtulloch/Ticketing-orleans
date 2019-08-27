using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grains.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using TicketingApi.Models;

namespace TicketingApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TicketingController : ControllerBase
    {
        private readonly IClusterClient _client;
        private const string TheatreId = "My Theatre";

        public TicketingController(IClusterClient client)
        {
            _client = client;

            //this is just for testing do not do in real life

            //Task.Run(async() => await this.SetupDummyShows());
        }

        [HttpGet("showinfo")]
        public async Task<List<string>> GetShows()
        {
            var showGrain = _client.GetGrain<IShowInfo>(TheatreId);
            return await showGrain.GetShows();
        }

        [HttpGet("tickets/{showId}")]
        public async Task<ActionResult<ShowInformation>> GetTickets(string showId)
        {
            var showTickets = _client.GetGrain<ITicketsReserved>(showId);

            var allTickets = await showTickets.GetAllTickets();

            var showInfo = new ShowInformation
            {
                ShowId = showId,
                Tickets = allTickets.Select((s) => new TicketInfo {TicketId = s.Item1, Sold = s.Item2})
                    .ToList()
            };

            return showInfo;
        }

        [HttpGet("ticketsunreserved/{showId}")]
        public async Task<ActionResult<ShowInformation>> GetUnreservedTickets(string showId)
        {
            var ticketsGrain = _client.GetGrain<ITicketsReserved>(showId);

            var unreservedTickets = await ticketsGrain.GetUnreservedTickets();

            var showInfo = new ShowInformation
            {
                ShowId = showId,
                UnsoldTickets = unreservedTickets
            };

            return showInfo ;
        }

        [HttpPost("{showId}")]
        public async Task<ActionResult<TicketInfo>> AddTicket(string showId, [FromBody] TicketInfo ticket)
        {
            var ticketsReserved = _client.GetGrain<ITicketsReserved>(showId);
            await ticketsReserved.SetTicket(ticket.TicketId, ticket.Sold);

            return Ok(ticket);
        }

        [HttpGet("setupshow/{showId}")]
        public async Task<ActionResult> SetupShow(string showId)
        {
            await AddGeneralTickets(showId);
            await AddVipTickets(showId);

            return Ok();
        }

        [HttpGet("viptickets/{showId}")]
        public async Task<ActionResult<string>> GetVipTicket(string showId)
        {
            var vipTicket = await _client.GetGrain<IVipQueue>(showId).GetVipTicket();
            if (string.IsNullOrEmpty(vipTicket))
            {
                return new OkObjectResult("no vip tickets remaining");
            }

            return new OkObjectResult(vipTicket);
        }

        [HttpGet("viptickets/remaining/{showId}")]
        public async Task<ActionResult<int>> RemainingVipTickets(string showId)
        {
            var remainingTickets = await _client.GetGrain<IVipQueue>(showId).RemainingVipTickets();

            return new OkObjectResult(remainingTickets);
        }

        /// <summary>
        /// This is just to set up dummy shows for testing
        /// </summary>
        /// <returns></returns>
        private async Task SetupDummyShows()
        {
            var shows = await _client.GetGrain<IShowInfo>(TheatreId).GetShows();
            foreach (var show in shows)
            {
                await AddGeneralTickets(show);
            }
        }

    private async Task AddGeneralTickets(string showId)
        {
            var ticketsGrain = _client.GetGrain<ITicketsReserved>(showId);
            var ticketCount = await ticketsGrain.GetTicketCount();

            if (ticketCount > 0)
            {
                return;
            }

            for (var i = 1; i <= 100; i++)
            {
                await ticketsGrain.SetTicket($"{showId} - {i}", false);
            }
        }

        private async Task AddVipTickets(string showId)
        {
            var vipTicketsGrain = _client.GetGrain<IVipQueue>(showId);
            {
                for (var i = 1; i <= 15 ; i++)
                {
                    await vipTicketsGrain.QueueTicket($"{showId} - V{i}");
                }
            }
        }

    }
}
