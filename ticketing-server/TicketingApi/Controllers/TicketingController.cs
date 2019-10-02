using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grains.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Orleans;
using Ticketing.Models;
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
           
        }

        [HttpGet("showinfo")]
        public async Task<List<ShowInformation>> GetShows()
        {
            var showGrain = _client.GetGrain<IShowInfo>(TheatreId);
            return await showGrain.GetShows();
        }

        [HttpGet("tickets/{showId}")]
        public async Task<ActionResult<ShowData>> GetTickets(string showId, [FromQuery] string date)
        {
            var theatreGrain = _client.GetGrain<IShowInfo>(TheatreId);
            var showInfo = await theatreGrain.GetShow(showId, date);

            return showInfo;
        }

        [HttpGet("ticketsunreserved/{showId}")]
        public async Task<ActionResult<ShowData>> GetUnreservedTickets(string showId)
        {
            var ticketsGrain = _client.GetGrain<ITicketsReserved>(showId);

            var unreservedTickets = await ticketsGrain.GetUnreservedTickets();

            var showInfo = new ShowData
            {
                ShowId = showId
            };

            return showInfo ;
        }

        [HttpPost("{showId}")]
        public async Task<ActionResult<TicketInfo>> AddTicket(string showId, [FromBody] TicketInfo ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var ticketsReserved = _client.GetGrain<ITicketsReserved>(showId);
            await ticketsReserved.SetTicket(new TicketBooking(showId,ticket.TicketId));

            return Ok(ticket);
        }

        [HttpGet("setupshow/{showId}")]
        public async Task<ActionResult> SetupShow(string showId)
        {
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
