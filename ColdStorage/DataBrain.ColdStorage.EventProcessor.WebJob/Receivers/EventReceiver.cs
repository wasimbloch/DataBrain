using DataBrain.ColdStorage.EventProcessor.WebJob.Processors;
using DataBrain.Core;
using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrain.ColdStorage.EventProcessor.WebJob.Receivers
{
    public class EventReceiver
    {
        private readonly EventProcessorHost _host;

        public EventReceiver()
        {
            _host = new EventProcessorHost(Environment.MachineName, Config.Get("ColdStorage.EventHubName"),
                Config.Get("ColdStorage.ConsumerGroupName"), Config.Get("ColdStorage.InputConnectionString"),
                Config.Get("ColdStorage.CheckpointConnectionString"));
        }

        public async Task RegisterProcessorAsync()
        {
            var processorOptions = new EventProcessorOptions
            {
                //MaxBatchSize = 5000,
                //PrefetchCount = 1000
                MaxBatchSize = 500,
                PrefetchCount = 100
            };
            await _host.RegisterEventProcessorAsync<ColdStorageEventProcessor>();
        }

        public async Task UnregisterProcessorAsync()
        {
            if (_host != null)
            {
                await _host.UnregisterEventProcessorAsync();
            }
        }
    }
}
