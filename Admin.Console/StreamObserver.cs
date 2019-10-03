using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Orleans.Streams;
using Ticketing.Models;

namespace Admin.View
{
    public class StreamObserver : IAsyncObserver<ShowTicketLogMessage>
    {
        private readonly ILogger _logger;

        public StreamObserver(ILogger logger = null)
        {
            _logger = logger;
        }
        public Task OnNextAsync(ShowTicketLogMessage item, StreamSequenceToken token = null)
        {
            Console.WriteLine($"{item.ShowName} - {item.TicketId}");
            return Task.CompletedTask;
        }

        public Task OnCompletedAsync()
        {
            _logger.LogInformation("Message stream received stream completed event");
            return Task.CompletedTask;
        }

        public Task OnErrorAsync(Exception ex)
        {
            _logger.LogInformation($"Message stream is experiencing message delivery failure, ex :{ex}");
            return Task.CompletedTask;
        }
    }
}