using Microsoft.ServiceBus.Messaging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrain.ColdStorage.EventProcessor.WebJob.Processors
{
    public class ColdStorageEventProcessor : IEventProcessor
    {
        public Task CloseAsync(PartitionContext context, CloseReason reason)
        {
            throw new NotImplementedException();
        }

        public Task OpenAsync(PartitionContext context)
        {
            throw new NotImplementedException();
        }

        public Task ProcessEventsAsync(PartitionContext context, IEnumerable<EventData> messages)
        {
            throw new NotImplementedException();
        }
    }
}
