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

        [HttpPost("{showId}")]
        public async Task<ActionResult<TicketInfo>> AddTicket(string showId, [FromBody] TicketInfo ticket)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var ticketsReserved = _client.GetGrain<ITicketsReserved>(showId);

            try
            {
                await ticketsReserved.SetTicket(new TicketBooking(showId,ticket.TicketId));
            }
            catch (Exception e)
            {
                return BadRequest();
            }

            return Ok(ticket);
        }
    }
}
