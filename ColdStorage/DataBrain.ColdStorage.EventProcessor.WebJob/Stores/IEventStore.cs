using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBrain.ColdStorage.EventProcessor.WebJob.Stores
{
    public interface IEventStore : IDisposable
    {
        int Level { get; }

        string ReceivedAtHour { get; }

        string PartitionId { get; }

        void Initialise(string partitionId, string receivedAtHour);

        void Write(byte[] value);

        void Flush();

        bool IsFlushOverdue();
    }
}
