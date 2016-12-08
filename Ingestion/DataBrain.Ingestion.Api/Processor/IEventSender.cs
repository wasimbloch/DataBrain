using Newtonsoft.Json.Linq;
using System.Threading.Tasks;

namespace DataBrain.Ingestion.Api.Processor
{
    public interface IEventSender
    {
        Task SendEventsAsync(JArray events, string deviceId);
    }
}