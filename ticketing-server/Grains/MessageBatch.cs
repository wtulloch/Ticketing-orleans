﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Grains.Interfaces;
using Orleans;
using Orleans.Providers;
using Ticketing.Models;

namespace Grains
{

    public class MessageBatchState
    {
        public List<ShowTicketLogMessage> Messages { get; set; } = new List<ShowTicketLogMessage>();
    }
    [StorageProvider(ProviderName = "store1")]
    public class MessageBatch: Grain<MessageBatchState>, IMessageBatch
    {
        private IDisposable _timer;
        public override Task OnActivateAsync()
        {
            _timer = RegisterTimer(ProcessMessages, null, TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));
            return base.OnActivateAsync();
        }

        public override Task OnDeactivateAsync()
        {
            _timer.Dispose();
            return base.OnDeactivateAsync();
        }

        public Task TicketNotification(ShowTicketLogMessage message)
        {
            State.Messages.Add(message);
            
            return Task.CompletedTask;
        }

        private Task ProcessMessages(object thing)
        {
            //State.Messages.Clear();
            return Task.CompletedTask;
        }
    }
}