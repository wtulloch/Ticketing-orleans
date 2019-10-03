using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grains.Interfaces;
using Orleans;
using Orleans.Providers;
using Orleans.Streams;
using Ticketing.Models;
using Utils;

namespace Grains
{

    public class MessageBatchState
    {
        public ConcurrentQueue<ShowTicketLogMessage> Messages { get; set; } = new ConcurrentQueue<ShowTicketLogMessage>();
    }
    [StorageProvider(ProviderName = "store1")]
    public class MessageBatch: Grain<MessageBatchState>, IMessageBatch
    {
        private IDisposable _timer;
        private IAsyncStream<ShowTicketLogMessage> _stream;
        public override Task OnActivateAsync()
        {
            var streamProvider = GetStreamProvider(TicketingConstants.LogStreamProvider);
            _stream = streamProvider.GetStream<ShowTicketLogMessage>(Guid.Empty, TicketingConstants.LogStreamNamespace);
            _timer = RegisterTimer(ProcessMessages, null, TimeSpan.FromSeconds(10), TimeSpan.FromSeconds(10));
            return base.OnActivateAsync();
        }

        public override Task OnDeactivateAsync()
        {
            _timer.Dispose();
            return base.OnDeactivateAsync();
        }

        public Task TicketNotification(ShowTicketLogMessage message)
        {
            State.Messages.Enqueue(message);
            
            return Task.CompletedTask;
        }

        private async Task ProcessMessages(object thing)
        {
            if (State.Messages.IsEmpty)
            {
                return;
            }
            
            while(!State.Messages.IsEmpty)
            {
                if (State.Messages.TryDequeue(out var message))
                  await  _stream.OnNextAsync(message);
            }

        }
    }
}